using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NetStandard.MediaConverter.Event;

namespace NetStandard.MediaConverter
{
    public class FFMpegEncoderTask
    {
        private readonly EnvironmentConfig _environmentConfig;
        private readonly MediaEncodingSetup _encodingSetup;
        private readonly FFMpegProgress _progress;
        private readonly string _workingDirectory;
        private DateTime _startTime;

        public Guid Id { get; set; }

        public string MediaId { get; protected set; }

        public bool IsFinished { get; private set; } = false;

        public VideoHeight VideoHeight => _encodingSetup.VideoHeight;

        internal Action TaskFinishedCallback { get; set; }

        public event EventHandler<ConvertProgressEventArgs> ConvertProgressEvent;

        public event EventHandler<ConvertFinishedEventArgs> ConvertFinishedEvent;

        public FFMpegEncoderTask(MediaEncodingSetup encodingSetup, MediaEncodingTaskInfo taskInfo)
        {
            FFMpegEncoderTask ffMpegEncoderTask = this;
            Id = Guid.NewGuid();
            MediaId = taskInfo.Id;
            _encodingSetup = encodingSetup;
            _progress = new FFMpegProgress(args => ConvertProgressEvent(this, args));
            _workingDirectory = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName;
            _environmentConfig = new EnvironmentConfig(_workingDirectory);
            if (!_encodingSetup.NotEncoding)
                TaskFinishedCallback += () => ffMpegEncoderTask.OnFinished(ffMpegEncoderTask, new ConvertFinishedEventArgs(DateTime.Now - ffMpegEncoderTask._startTime, taskInfo));
        }

        public void Convert()
        {
            _startTime = DateTime.Now;
            if (IsFinished)
                throw new Exception("Cannot start task when it's finished");
            try
            {
                Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        private void Start()
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = _environmentConfig.FFMpegExecutable;
            startInfo.Arguments = _encodingSetup.GetCmdLineParams();
            startInfo.WorkingDirectory = _workingDirectory;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            using (var process = new Process())
            {
                process.StartInfo = startInfo;
                process.ErrorDataReceived += new DataReceivedEventHandler(OnErrorDataReceived);
                process.OutputDataReceived += new DataReceivedEventHandler(OnOutputDataReceived);
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                //_ffmpegProcess.PriorityClass = ProcessPriorityClass.Normal;
                process.WaitForExit();
                IsFinished = true;
                if (TaskFinishedCallback == null) return;
                TaskFinishedCallback();
            }
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _progress.ParseLine(e.Data);
        }

        private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            _progress.ParseLine(e.Data);
        }

        private void OnProgress(object sender, ConvertProgressEventArgs eargs)
        {
            if (ConvertProgressEvent == null) return;
            ConvertProgressEvent(sender, eargs);
        }

        private void OnFinished(object sender, ConvertFinishedEventArgs eargs)
        {
            if (ConvertFinishedEvent == null) return;
            ConvertFinishedEvent(sender, eargs);
        }

        public static bool ProgressCompleted(object sender, ConvertProgressEventArgs eargs, out string mediaId)
        {
            if (eargs.IsCompleted)
            {
                if (sender is FFMpegEncoderTask)
                {
                    mediaId = (sender as FFMpegEncoderTask).MediaId;
                    return true;
                }
                mediaId = null;
                return true;
            }
            mediaId = null;
            return false;
        }

        public static bool GainedDuration(object sender, ConvertProgressEventArgs eargs, out string mediaId)
        {
            if (eargs.IsGainedDuration)
            {
                if (sender is FFMpegEncoderTask)
                {
                    mediaId = (sender as FFMpegEncoderTask).MediaId;
                    return true;
                }
                mediaId = null;
                return true;
            }
            mediaId = null;
            return false;
        }
    }
}
