using Dalamud.Interface.Windowing;
using ECommons.DalamudServices;
using ECommons.ExcelServices;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;

namespace wtool.Windows;

public class SettingUi : Window, IDisposable
{
    public int action;
    public string CustomActionName;
    private int opentime;

    public SettingUi() : base("sh")
    {
        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(232, 232);
        SizeCondition = ImGuiCond.Always;
        opentime = 0;
        action = 0;
        CustomActionName = "";
    }

    public void Dispose() { }

    public override void Draw()
    {
        if (action == null || CustomActionName == null)
        {
            if (IsOpen) Toggle();
            return;
        }

        ImGui.InputText("技能别称", ref CustomActionName, 10);
        ImGui.InputInt("技能id", ref action);
        if (action is < 0 or > 41000)
        {
            if (action > 0) action = 41000;

            if (action < 0) action = 0;
        }


        ImGui.Text(ExcelActionHelper.GetActionName((uint)action));

        ImGui.NewLine();
        if (ImGui.Button("保存"))
        {
            if (Setting.Instance.ActionId.Contains((uint)action)) return; //通知一下
            if (CustomActionName != "")
            {
                try
                {
                    Setting.Instance.CustomActionName.Add((uint)action, CustomActionName);
                    Setting.Instance.ActionId.Add((uint)action);
                    Toggle();
                    opentime = 0;
                    Setting.Instance.Save();
                    return;
                }
                catch (Exception e)
                {
                    Svc.Log.Error(e.ToString());
                    throw;
                }
            }

            Setting.Instance.ActionId.Add((uint)action);
            Setting.Instance.Save();
            Toggle();
            opentime = 0;
        }

        if (!IsFocused)
        {
            if (opentime > 100)
            {
                Toggle();
                opentime = 0;
            }
        }

        opentime++;
    }
}
