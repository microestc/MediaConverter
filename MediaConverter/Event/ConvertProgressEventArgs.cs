using System;

namespace MediaConverter.Event
{
    public class ConvertProgressEventArgs : EventArgs
    {
        public TimeSpan TotalDuration { get; private set; }

        public TimeSpan Processed { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsGainedDuration { get; private set; }

        public ConvertProgressEventArgs(TimeSpan totalDuration, TimeSpan processed, bool isGainedDuration, bool isCompleted = false)
        {
            TotalDuration = totalDuration;
            Processed = processed;
            IsGainedDuration = isGainedDuration;
            IsCompleted = isCompleted;
        }

        public int GetPctCompleted()
        {
            double num = Processed.TotalMilliseconds * 100.0;
            double totalMilliseconds = TotalDuration.TotalMilliseconds;
            return Convert.ToInt32(num / totalMilliseconds);
        }
    }
}
