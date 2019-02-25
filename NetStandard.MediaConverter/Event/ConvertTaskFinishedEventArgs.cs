using System;

namespace NetStandard.MediaConverter.Event
{
    public class ConvertTaskFinishedEventArgs : EventArgs
    {
        public FFMpegEncoderTask FinishedTask { get; set; }

        public ConvertTaskFinishedEventArgs(FFMpegEncoderTask task)
        {
            FinishedTask = task;
        }
    }
}
