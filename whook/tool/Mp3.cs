using CSCore.Codecs.MP3;
using CSCore.SoundOut;
using ECommons.DalamudServices;

namespace wtool.tool;

public static class Mp3
{
    public static void p()
    {
        if (!File.Exists(Setting.Instance.ospath + @"1.mp3"))
        {
            Svc.Log.Debug(Setting.Instance.ospath + @"1.mp3");
            Svc.Log.Debug("文件不存在");
            return;
        }

        using (var f = File.OpenRead(Setting.Instance.ospath + @"Data\1.mp3"))
        {
            var audiodata = new Mp3MediafoundationDecoder(f);
            var soundOutDevice = new WasapiOut();
            soundOutDevice.Initialize(audiodata);
            soundOutDevice.Volume = 0.5f;
            soundOutDevice.Play();
        }
    }
}
