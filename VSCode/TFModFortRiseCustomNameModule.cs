using System;
using FortRise;
using System.Diagnostics;
using MonoMod.ModInterop;
using MonoMod.Utils;

namespace TFModFortRiseCustomName
{
  [Fort("com.ebe1.kenobi.tfmodfortriscustomname", "TFModFortRiseCustomNameModule")]
  public class TFModFortRiseCustomNameModule : FortModule
  {
    public static TFModFortRiseCustomNameModule Instance;

    public override Type SettingsType => typeof(TFModFortRiseCustomNameSettings);
    public static TFModFortRiseCustomNameSettings Settings => (TFModFortRiseCustomNameSettings)Instance.InternalSettings;


    public TFModFortRiseCustomNameModule() 
    {
      //if (!Debugger.IsAttached)
      //{
        //Debugger.Launch(); // Proposera d’attacher Visual Studio
      //}
      Instance = this;
      //Logger.Init("ModCustomName");
    }

    public override void LoadContent()
    {
    }

    public override void Load()
    {
      MyTFGame.Load();
      MyRollcallElement.Load();
      MyPlayerIndicator.Load();
      MyVersusRoundResults.Load();
      typeof(ModExports).ModInterop();
    }

    public override void Unload()
    {
      MyTFGame.Unload();
      MyRollcallElement.Unload();
      MyPlayerIndicator.Unload();
      MyVersusRoundResults.Unload();
    }
  }
}

[ModExportName("com.fortrise.TFModFortRiseCustomName")]
public static class ModExports
{
  public static void SetPlayerName(int playerIndex, String playerName)
  {
    TFModFortRiseCustomName.MyRollcallElement.SetPlayerName(playerIndex, playerName);
  }

  public static String GetPlayerName(int playerIndex)
  {
    return TFModFortRiseCustomName.MyRollcallElement.GetPlayerName(playerIndex);
  }
}
