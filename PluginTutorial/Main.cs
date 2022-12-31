using PluginTutorial;
using robotManager.Helpful;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using wManager.Plugin;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

public class Main : IPlugin
{
    private bool _isRunning;
    private List<WoWUnit> _attackableUnits = new List<WoWUnit>();

    public void Initialize()
    {
        TutorialPluginSettings.Load();
        Logging.Write("PluginTutorial initialized");
        _isRunning = true;
        EventsLuaWithArgs.OnEventsLuaStringWithArgs += PluginEventHandler;
        if (TutorialPluginSettings.CurrentSettings.EnableRadar)
        {
            Radar3D.OnDrawEvent += DrawRadar;
            Radar3D.Pulse();
        }
        PluginLoop();
    }

    public void Dispose()
    {
        Logging.Write("PluginTutorial disposed");
        EventsLuaWithArgs.OnEventsLuaStringWithArgs -= PluginEventHandler;
        if (TutorialPluginSettings.CurrentSettings.EnableRadar)
        {
            Radar3D.OnDrawEvent -= DrawRadar;
        }
        _isRunning = false;
    }

    private void PluginEventHandler(string eventid, List<string> args)
    {
        if (eventid == "AUTOEQUIP_BIND_CONFIRM")
        {
            Lua.LuaDoString("StaticPopup1Button1:Click();");
        }
    }

    private void DrawRadar()
    {
        foreach (WoWUnit unit in _attackableUnits)
        {
            Radar3D.DrawCircle(unit.Position, 1.5f, Color.Red);
            Radar3D.DrawLine(ObjectManager.Me.Position, unit.Position, Color.Red, 100);
        }
    }

    private void PluginLoop()
    {
        while (_isRunning)
        {
            if (Conditions.InGameAndConnectedAndProductStartedNotInPause)
            {
                _attackableUnits = ObjectManager.GetWoWUnitAttackables();
            }

            Thread.Sleep(1000);
        }
    }

    public void Settings()
    {
        TutorialPluginSettings.Load();
        TutorialPluginSettings.CurrentSettings.ToForm();
        TutorialPluginSettings.CurrentSettings.Save();
    }
}
