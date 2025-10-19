using System;
using FortRise;
using System.Diagnostics;

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
      if (!Debugger.IsAttached)
      {
        Debugger.Launch(); // Proposera d’attacher Visual Studio
      }
      Instance = this;
      Logger.Init("ModCustomNameModul");
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
