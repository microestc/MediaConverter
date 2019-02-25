using System;

namespace NetStandard.MediaConverter.Event
{
    public class ConvertFinishedEventArgs : EventArgs
    {
        public TimeSpan ElapsedTime { get; set; }

        public MediaEncodingTaskInfo TaskInfo { get; set; }

        public ConvertFinishedEventArgs(TimeSpan elapsedTime, MediaEncodingTaskInfo taskInfo)
        {
            ElapsedTime = elapsedTime;
            TaskInfo = taskInfo;
        }
    }
}
