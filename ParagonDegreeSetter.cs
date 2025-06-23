using MelonLoader;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppSystem;
using ParagonDegreeSetter;

[assembly: MelonInfo(typeof(ParagonDegreeSetter.ParagonDegreeSetter), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace ParagonDegreeSetter;

public class ParagonDegreeSetter : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<ParagonDegreeSetter>("ParagonDegreeSetter loaded!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (InGame.instance == null || !InGameData.CurrentGame.IsSandbox || PopupScreen.instance.IsPopupActive()) return;

        if (Settings.setParagonDegreeHotkey.JustPressed())
        {
            var selectedTower = InGame.instance.inputManager?.SelectedTower;
            
            if (selectedTower == null || !selectedTower.IsParagon) return;
                
            PopupScreen.instance.SafelyQueue(screen =>
                screen.ShowSetValuePopup(
                    "Paragon Degree Setter",
                    "Set Paragon degree:",
                    (Action<int>)SetParagonDegree,
                    selectedTower.GetParagonDegreeMutator().degree
                )
            );
        }
    }

    private void SetParagonDegree(int degree)
    {
        var paragon = InGame.instance?.inputManager?.SelectedTower?.tower;
        
        if (paragon == null)
        {
            ModHelper.Msg<ParagonDegreeSetter>("Uh oh. The paragon went missing!");
            return;
        }
        
        var paragonTower = paragon.GetTowerBehavior<ParagonTower>();
        
        var info = paragonTower.investmentInfo;
        var y = Math.Min(InGame.instance.GetGameModel().paragonDegreeDataModel.degreeCount, degree);
        var z = Math.Max(1, y);

        if (z <= Game.instance.model.paragonDegreeDataModel.powerDegreeRequirements.Length)
        {
            info.totalInvestment = Game.instance.model.paragonDegreeDataModel.powerDegreeRequirements[z - 1];
            paragonTower.investmentInfo = info;
            paragonTower.UpdateDegree();
            return;
        }

        info.totalInvestment = CalculateInvestment(z);
        paragonTower.investmentInfo = info;
        paragonTower.UpdateDegree();
    }
    
    private static float CalculateInvestment(float degree)
    {
        var investment = 50f * (float)Math.Pow(degree, 3);
        investment += 5025f * (float)Math.Pow(degree, 2);
        investment += 168324f * degree;
        investment += 843000f;
        investment /= 590f;
        return investment;
    }
}