using Dalamud.Game;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ECommons.DalamudServices;
using wtool.tool;
using wtool.Windows;
using xwwhook;

namespace wtool;

public sealed class Plugin : IDalamudPlugin
{
    private readonly eventhook e;

    public readonly WindowSystem WindowSystem = new("xww");
    public string ospath;
    public SettingUi Settingwindow;

    public Plugin(IDalamudPluginInterface pi)
    {
        Svc.Init(pi);
        Setting.init(pi);
        Mp3.p();
        MainWindow = new MainWindow(this);
        Settingwindow = new SettingUi();
        WindowSystem.AddWindow(MainWindow);
        WindowSystem.AddWindow(Settingwindow);
        Svc.PluginInterface.UiBuilder.Draw += DrawUI;
        Svc.PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
        e = new eventhook();
        Svc.Log.Debug(Svc.ClientState.LocalPlayer.ClassJob.GetWithLanguage(ClientLanguage.ChineseSimplified).Name);
        e.init();
    }

    private MainWindow MainWindow { get; init; }

    public void Dispose()
    {
        WindowSystem.RemoveWindow(MainWindow);
        MainWindow.Dispose();
        Settingwindow.Dispose();
        e.Dispose();
    }


    private void DrawUI()
    {
        WindowSystem.Draw();
    }

    public void ToggleMainUI()
    {
        MainWindow.Toggle();
    }

    public void setdrwu()
    {
        Settingwindow.Toggle();
    }
}
