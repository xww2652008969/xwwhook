using System.Text;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace wtool.tool;

public static class wtool
{
    public static unsafe void Toast(string msg, int s, int time)
    {
        if (string.IsNullOrEmpty(msg))
            msg = "空文本";
        if (time < 500)
        {
            msg = "提示时间过短";
            time = 1000;
        }

        if (time > 10000)
        {
            msg = "提示时间过长";
            time = 3000;
        }

        time /= 100;
        var style = RaptureAtkModule.TextGimmickHintStyle.Info;
        if (s == 2)
            style = RaptureAtkModule.TextGimmickHintStyle.Warning;
        var bytes = Encoding.UTF8.GetBytes(msg);
        RaptureAtkModule.Instance()->ShowTextGimmickHint((ReadOnlySpan<byte>)bytes, style, time);
    }
}
