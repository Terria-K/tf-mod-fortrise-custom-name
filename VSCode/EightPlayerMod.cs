using System;
using MonoMod.ModInterop;

namespace TFModFortRiseCustomName {
  [ModImportName("com.fortrise.EightPlayerMod")]
  public class EightPlayerImport
  {
      public static Func<bool> IsEightPlayer;
      public static Func<bool> LaunchedEightPlayer;
  }
}
