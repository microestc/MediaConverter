namespace NetStandard.MediaConverter
{
    public class MediaEncodingTaskInfo
    {
        public MediaEncodingTaskInfo(string dir, string id, string format)
        {
            Dir = dir;
            Id = id;
            Format = format;
        }

        public string Dir { get; private set; }

        public string Id { get; private set; }

        public string Format { get; private set; }
    }
}
