namespace MediaConverter
{
    public class MediaEncodingSetup
    {
        /// <summary>
        /// ��Դ��Ƶ�ľ���·��
        /// </summary>
        public string Source { get; private set; }

        /// <summary>
        /// Ŀ����Ƶ�ľ���·��
        /// </summary>
        public string Dest { get; private set; }

        /// <summary>
        /// ����ͼ�ľ���·��(����������ͼ�ļ���ʽ��׺.jpeg)
        /// </summary>
        public string Thumb { get; private set; }

        public VideoHeight VideoHeight { get; private set; }

        public bool NotEncoding { get; private set; }

        public MediaEncodingSetup(string source, string thumb)
        {
            Source = source;
            Thumb = thumb;
            NotEncoding = true;
        }

        public MediaEncodingSetup(string source, string dest, string thumb, VideoHeight videoHeight)
        {
            Source = source;
            Dest = dest;
            Thumb = thumb;
            VideoHeight = videoHeight;
            NotEncoding = false;
        }

        internal string GetCmdLineParams()
        {
            if (NotEncoding) return $"-y -loglevel info -i {Source} -vframes 1 -f mjpeg {Thumb}.jpeg";
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
            return $"-y -loglevel info -i {Source} -c:v libx264 -vf scale=\"trunc(oh * a / 2) * 2:{videoHeight}\" {Dest} -vframes 1 -f mjpeg {Thumb}.jpeg";
        }

    }
}
