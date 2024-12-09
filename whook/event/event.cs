using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using ECommons.DalamudServices;
using wtool;
using xwwhook.Game;

namespace xwwhook;

public class eventhook
{
    [Signature("40 55 56 57 41 54 41 55 41 56 48 8D AC 24 68 FF FF FF 48 81 EC 98 01 00 00",
               DetourName = nameof(ProcessPacketActionEffectDetour))]
    private readonly Hook<ProcessPacketActionEffectDelegate> processPacketActionEffectHook = null!;


    [Signature("E8 ?? ?? ?? ?? 0F B7 0B 83 E9 64", DetourName = nameof(ProcessPacketActorControlDetour))]
    private readonly Hook<ProcessPacketActorControlDelegate> processPacketActorControlHook = null!;


    [Signature("48 8B C4 44 88 40 18 89 48 08", DetourName = nameof(ProcessPacketEffectResultDetour))]
    private readonly Hook<ProcessPacketEffectResultDelegate> processPacketEffectResultHook = null!;

    public void init()
    {
        Svc.Hook.InitializeFromAttributes(this);
        processPacketActionEffectHook.Enable();
        processPacketActorControlHook.Enable();
        processPacketEffectResultHook.Enable();
    }

    public void Dispose()
    {
        processPacketActionEffectHook.Dispose();
        processPacketEffectResultHook.Dispose();
        processPacketActorControlHook.Dispose();
    }

    private unsafe void ProcessPacketActionEffectDetour(
        int sourceId, IntPtr sourceCharacter, IntPtr pos, ActionEffectHeader* effectHeader, ActionEffect* effectArray,
        ulong* effectTrail)
    {
        processPacketActionEffectHook.Original(sourceId, sourceCharacter, pos, effectHeader, effectArray, effectTrail);
        SendMes.send(sourceId, sourceCharacter, pos, effectHeader, effectArray);
        // var targets = effectHeader->EffectCount;  //施法后的目标
        // if (targets <1)
        // {
        //     return;
        // }
        //
        var o = Svc.Objects.SearchById((uint)sourceId); //获取目标对象
        // var actiomid=effectHeader->ActionId;  //技能id
        // // Svc.Log.Debug(ExcelActionHelper.GetActionName(effectHeader->ActionId));
        // Svc.Log.Debug(effectArray->Value.ToString());   //技能释放的效果
        //
    }

    private void ProcessPacketActorControlDetour(
        uint entityId, uint type, uint statusId, uint amount, uint a5, uint source, uint a7, uint a8, ulong a9,
        byte flag)
    {
        processPacketActorControlHook.Original(entityId, type, statusId, amount, a5, source, a7, a8, a9, flag);
        //entityId     这个是角色id  用=Svc.Objects.SearchById  返回一个角色对象

        //处理持续伤害或者dot的
        // try
        // {
        //     
        //     Svc.Log.Debug(type.ToString());
        //     
        // }
        // catch (Exception e)
        // {
        //     Svc.Log.Debug(e.ToString());
        // }
    }

    private void ProcessPacketEffectResultDetour(uint targetId, IntPtr actionIntegrityData, bool isReplay)
    {
        processPacketEffectResultHook.Original(targetId, actionIntegrityData, isReplay);
        //好像是触发buff的先不弄了
    }


    private unsafe delegate void ProcessPacketActionEffectDelegate(
        int sourceId, IntPtr sourceCharacter, IntPtr pos, ActionEffectHeader* effectHeader, ActionEffect* effectArray,
        ulong* effectTrail);

    private delegate void ProcessPacketActorControlDelegate(
        uint entityId, uint type, uint statusId, uint amount, uint a5, uint source, uint a7, uint a8, ulong a9,
        byte flag);

    private delegate void ProcessPacketEffectResultDelegate(uint targetId, IntPtr actionIntegrityData, bool isReplay);
}
