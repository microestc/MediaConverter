using System;
using System.Text.RegularExpressions;
using MediaConverter.Event;

namespace MediaConverter
{
    public class FFMpegProgress
    {
        private static readonly Regex _durationRegex = new Regex("Duration:\\s(?<Duration>[0-9:.]+)([,]|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex _progressRegex = new Regex("time=(?<progress>[0-9:.]+)\\s", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
        private TimeSpan _totalDuration = TimeSpan.FromMilliseconds(1.0);
        private TimeSpan _progress = TimeSpan.FromMilliseconds(0.0);
        private readonly Action<ConvertProgressEventArgs> ProgressCallback;

        public FFMpegProgress(Action<ConvertProgressEventArgs> progressCallback)
        {
            ProgressCallback = progressCallback;
        }

        public void ParseLine(string line)
        {
            if (string.IsNullOrEmpty(line))
                return;
            FetchIsComplete(line);
            if (_totalDuration == TimeSpan.FromMilliseconds(1.0))
                TryGetDuration(line);
            FetchProgress(line);
        }

        private void TryGetDuration(string line)
        {
            if (!line.Contains("Duration"))
                return;
            Match match = _durationRegex.Match(line);
            if (!match.Success)
                return;
            if (!TimeSpan.TryParse(match.Groups["Duration"].Value, out var result))
                return;
            _totalDuration = result;
            FetchDuration();
        }

        private void FetchProgress(string line)
        {
            Match match = _progressRegex.Match(line);
            if (!match.Success || !TimeSpan.TryParse(match.Groups["progress"].Value, out _progress))
                return;
            ProgressCallback(new ConvertProgressEventArgs(_totalDuration, _progress, false));
        }

        private void FetchDuration()
        {
            ProgressCallback(new ConvertProgressEventArgs(_totalDuration, _progress, true));
        }

        private void FetchIsComplete(string line)
        {
            if (!line.Contains("Qavg"))
                return;
            ProgressCallback(new ConvertProgressEventArgs(_totalDuration, _progress, false, true));
        }
    }
}
