namespace NetStandard.MediaConverter
{
    public class MediaEncodingSetup
    {
        public string SourceFile { get; private set; }

        public string DestinationFile { get; private set; }

        public VideoHeight VideoHeight { get; private set; }

        public bool NotEncoding { get; private set; }

        public MediaEncodingSetup(string sourceFile)
        {
            SourceFile = sourceFile;
            NotEncoding = true;
        }

        public MediaEncodingSetup(string sourceFile, string destinationFile, VideoHeight videoHeight)
        {
            SourceFile = sourceFile;
            DestinationFile = destinationFile;
            VideoHeight = videoHeight;
            NotEncoding = false;
        }

        internal string GetCmdLineParams()
        {
            if (NotEncoding) return $"ffmpeg -loglevel info -i {SourceFile}";
            string videoHeight = "240";
            switch (VideoHeight)
            {
                case VideoHeight.P240:
                    videoHeight = "240";
                    break;
                case VideoHeight.P480:
                    videoHeight = "480";
                    break;
                case VideoHeight.P720:
                    videoHeight = "720";
                    break;
                case VideoHeight.P1080:
                    videoHeight = "1080";
                    break;
            }
            return string.Format("-y -loglevel info -i {0} -c:v libx264 -vf scale=\"trunc(oh * a / 2) * 2:{1}\" {2}", SourceFile, videoHeight, DestinationFile);
        }

    }
}
