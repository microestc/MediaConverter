namespace MediaConverter
{
    public class MediaEncodingSetup
    {
        /// <summary>
        /// 资源视频的绝对路劲
        /// </summary>
        public string Source { get; private set; }

        /// <summary>
        /// 目标视频的绝对路径(不包含文件格式)
        /// </summary>
        public string Dest { get; private set; }

        /// <summary>
        /// 目标视频的文件格式
        /// </summary>
        public string Format { get; private set; }

        public VideoHeight VideoHeight { get; private set; }

        public bool NotEncoding { get; private set; }

        public MediaEncodingSetup(string source, string dest)
        {
            Source = source;
            Dest = dest;
            NotEncoding = true;
        }

        public MediaEncodingSetup(string source, string dest, string format, VideoHeight videoHeight)
        {
            Source = source;
            Dest = dest;
            Format = format;
            VideoHeight = videoHeight;
            NotEncoding = false;
        }

        internal string GetCmdLineParams()
        {
            if (NotEncoding) return $"ffmpeg -y -loglevel info -i {Source} -vframes 1 -f mjpeg {Dest}.jpg";
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
            return $"-y -loglevel info -i {Source} -c:v libx264 -vf scale=\"trunc(oh * a / 2) * 2:{videoHeight}\" {Dest}{Format} -vframes 1 -f mjpeg {Dest}.jpg";
        }

    }
}
