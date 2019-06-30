
//using NAudio.Wave;
using System.IO;

namespace IntelReader
{
    public static class PlaySound
    {
        

        public static void playAlert(string n)
        {
            if (n == "0")
                return;

            string fn = Directory.GetCurrentDirectory() + $@"\sounds\{n}.wav";
            if (File.Exists(fn))
            {
                // TODO  change this for other platforms
                System.Diagnostics.Process.Start(@"powershell", $@"-c (New-Object Media.SoundPlayer '{fn}').PlaySync();");
            }

            //var waveFileReader = new WaveFileReader(Directory.GetCurrentDirectory() + $@"\sounds\{n}.wav");
            //WaveOutEvent player = new WaveOutEvent();
            //player.Init(waveFileReader);
            //player.PlaySync();
          
        }
    }
}