using System.IO;
using System.Runtime.InteropServices;

namespace NetStandard.MediaConverter
{
    public class EnvironmentConfig
    {
        public string FFMpegExecutable { get; }

        public EnvironmentConfig(string workingDirectory)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FFMpegExecutable = Path.Combine(workingDirectory, "ffmpeg.exe");
                if (!File.Exists(FFMpegExecutable))
                    throw new FileNotFoundException("FFmpeg is required to run video encoding tasks. Download it at http://ffmpeg.zeranoe.com/builds/");
            }
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return;
            FFMpegExecutable = "ffmpeg";
        }
    }
}
