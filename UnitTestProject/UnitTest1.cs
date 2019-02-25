using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetStandard.MediaConverter;
using NetStandard.MediaConverter.Event;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            var queue = new FFmpegQueue();

            var source = @"1.MOV";
            var dest = @"1.mp4";
          
            var task = new FFMpegEncoderTask(new MediaEncodingSetup(source, dest, VideoHeight.P720));
            task.ConvertFinishedEvent += ConvertFinished;
            task.ConvertProgressEvent += ConvertProgress;

            queue.EnqueueTask(task);
            System.Threading.Thread.Sleep(100000);
        }

        protected void ConvertFinished(object sender, ConvertFinishedEventArgs args)
        {
            var e = args.ElapsedTime;
        }

        protected void ConvertProgress(object sender, ConvertProgressEventArgs args) {
            if (args.IsCompleted)
            {
                var duration = args.TotalDuration;
                var d = args.Processed;
            }
        }

    }
}
;