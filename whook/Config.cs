using Dalamud.Plugin;
using ECommons.DalamudServices;
using Newtonsoft.Json;

namespace wtool;

public class Setting
{
    public static Setting Instance = new();
    public static string path;
    public List<uint> ActionId;                       //播报的技能列表
    public Dictionary<uint, string> CustomActionName; //自定义名字
    public bool isduplicate;                          //是否在副本播报
    public bool isHighduplicate;                      //是否高难本
    public bool Isrun;                                //总开关
    public bool Istom;                                //tom播报
    public bool IsVoicebroadcast;                     //是否语音播报
    public string Myname;                             //玩家自定义名字

    public string ospath;


    public string version = "1.0.0"; //版本号

    public static void init(IDalamudPluginInterface pi)
    {
        Instance = new Setting();
        path = Path.Combine(pi.ConfigDirectory.ToString(), "config.json");
        var s = pi.AssemblyLocation.ToString().Split(@"\");
        for (var i = 0; i < s.Length - 1; i++) Instance.ospath += s[i] + @"\";
        if (!File.Exists(path))
        {
            Instance.CustomActionName = new Dictionary<uint, string>();
            Instance.ActionId = new List<uint>();
            Instance.Save();
            return;
        }

        try
        {
            var j = File.ReadAllText(path);
            Instance = JsonConvert.DeserializeObject<Setting>(j);
        }
        catch (Exception e)
        {
            Svc.Log.Debug(e.ToString());
            throw;
        }
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, JsonConvert.SerializeObject(Instance, Formatting.Indented));
    }
}
