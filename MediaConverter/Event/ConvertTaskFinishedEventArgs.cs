using System;

namespace MediaConverter.Event
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
