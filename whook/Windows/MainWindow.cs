using System.Numerics;
using Dalamud.Interface.Windowing;
using ECommons.ExcelServices;
using ImGuiNET;

namespace wtool.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly Plugin plugin;

    public MainWindow(Plugin plugin) : base("main", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        this.plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Checkbox("总开关", ref Setting.Instance.Isrun);

        if (!Setting.Instance.Isrun) return;

        ImGui.Checkbox("tom播报", ref Setting.Instance.Istom);


        if (ImGui.Button("添加数据"))
        {
            if (plugin.Settingwindow.IsOpen) return;
            ImGui.SetNextWindowPos(ImGui.GetWindowPos());
            plugin.setdrwu();
        }

        ImGui.Checkbox("选择", ref Setting.Instance.isHighduplicate);
        if (ImGui.CollapsingHeader("已添加的技能"))
        {
            if (ImGui.BeginTable(
                    "setsp", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable))
            {
                // 表头
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text("技能名字");
                ImGui.TableNextColumn();
                ImGui.Text("技能id");
                ImGui.TableNextColumn();
                ImGui.Text("自定义技能名字");
                ImGui.TableNextColumn();
                ImGui.Text("操作");
                ImGui.TableNextColumn();
                // 表格内容
                for (var i = 0; i < Setting.Instance.ActionId.Count; i++)
                {
                    ImGui.TableNextRow();

                    ImGui.TableNextColumn();
                    ImGui.Text(ExcelActionHelper.GetActionName(Setting.Instance.ActionId[i]));

                    ImGui.TableNextColumn();
                    ImGui.Text(Setting.Instance.ActionId[i].ToString());

                    ImGui.TableNextColumn();
                    if (Setting.Instance.CustomActionName.ContainsKey(Setting.Instance.ActionId[i]))
                        ImGui.Text(Setting.Instance.CustomActionName[Setting.Instance.ActionId[i]]);
                    else
                        ImGui.Text(" ");
                    ImGui.TableNextColumn();
                    if (ImGui.Button("删除"))
                    {
                        Setting.Instance.ActionId.RemoveAt(i);
                        if (Setting.Instance.CustomActionName.ContainsKey(Setting.Instance.ActionId[i]))
                            Setting.Instance.CustomActionName.Remove(Setting.Instance.ActionId[i]);
                        Setting.Instance.Save();
                    }
                }

                ImGui.EndTable();
            }
        }


        if (ImGui.Button("保存")) Setting.Instance.Save();
    }
}
