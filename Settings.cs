using BTD_Mod_Helper.Api.Data;
using BTD_Mod_Helper.Api.ModOptions;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using UnityEngine;

namespace ParagonDegreeSetter;

public class Settings : ModSettings
{
    public static readonly ModSettingHotkey setParagonDegreeHotkey = new(KeyCode.P, HotkeyModifier.Ctrl)
    {
        displayName = "Set Paragon Degree Hotkey"
    };
}