using Microsoft.Xna.Framework;
using System;

namespace TFModFortRiseCustomName;

public sealed class ApiImplementation : ICustomNameModApi
{
    public ApiImplementation() {}

    public void SetPlayerName(int playerIndex, String playerName)
    {
      TFModFortRiseCustomName.MyRollcallElement.SetPlayerName(playerIndex, playerName);
    }

    public String GetPlayerName(int playerIndex)
    {
      return TFModFortRiseCustomName.MyRollcallElement.GetPlayerName(playerIndex);
    }
}
