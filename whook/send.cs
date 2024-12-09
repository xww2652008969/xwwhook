using ECommons.DalamudServices;
using ECommons.ExcelServices;
using wtool.tool;
using xwwhook.Game;

namespace wtool;

public static class SendMes
{
    public static unsafe void send(
        int sourceId, IntPtr sourceCharacter, IntPtr pos, ActionEffectHeader* effectHeader, ActionEffect* effectArray)
    {
        if (true)
        {
            try
            {
                var targets = effectHeader->EffectCount;
                if (targets < 1) return;
                if (Svc.Objects.SearchById((uint)sourceId).GameObjectId == Svc.ClientState.LocalPlayer.GameObjectId)
                {
                    for (var i = 0; i < targets; i++)
                    {
                        var actionEffect = effectArray[i * 8];
                        to(effectHeader->ActionId, effectArray->Param0, effectArray->Value);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Svc.Log.Debug(e.ToString());
            }
        }
    }


    public static void to(uint actionid, int flag, ushort value)
    {
        if (!Setting.Instance.ActionId.Contains(actionid)) return;

        var acname = "";
        if (Setting.Instance.CustomActionName.ContainsKey(actionid))
            acname = Setting.Instance.CustomActionName[actionid];
        else
            acname = ExcelActionHelper.GetActionName(actionid);

        if (flag == 64)
        {
            Mp3.p();
            tool.wtool.Toast("爷的" + acname + "直了", 1, 1000);
            //直
        }

        if (flag == 32)
        {
            Mp3.p();
            tool.wtool.Toast("爷的" + acname + "暴了", 1, 1000);
            //暴
        }

        if (flag == 96)
        {
            Mp3.p();
            tool.wtool.Toast("爷的" + acname + "直暴了", 1, 1000);
            //直暴
        }
    }
}
