using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Monocle;
using MonoMod.Utils;
using FortRise;
using TowerFall;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;
//using System.Drawing;
using System.Text;
using System.Globalization;

namespace TFModFortRiseCustomName
{
  internal class MyTFGame
  {
    internal static void Load()
    {
      On.TowerFall.TFGame.Load += Load_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.TFGame.Load -= Load_patch;
    }

    private static void Load_patch(On.TowerFall.TFGame.orig_Load orig)
    {
      orig();



      TaskHelper.Run("LOAD CONFIG FILE WITH PLAYER NAME", () =>
      {
        try
        {
          string filePath = @".\Mods\tf-mod-fortrise-custom-name\playerName.json";

          if (!File.Exists(filePath))
          {
            MyRollcallElement.playerNamesAvailable = new List<string>();
            return;
          }

          string jsonContent = File.ReadAllText(filePath);

          // Désérialisation avec Newtonsoft.Json
          var names = JsonConvert.DeserializeObject<List<string>>(jsonContent);

          // Nettoyage : lettres, chiffres, espaces
          for (int i = 0; i < names.Count; i++)
          {
            if (names[i] != null)
            {
              names[i] = RemoveDiacritics(names[i]);
              // Retire tout caractère qui n'est pas lettre, chiffre ou espace
              names[i] = Regex.Replace(names[i], @"[^A-Za-z0-9 '-_?!:\(\)\\/]", "");

              // Optionnel : trim pour enlever espaces en début/fin
              names[i] = names[i].Trim();
              names[i] = names[i].ToUpperInvariant();
            }
          }

          if (names == null)
          {
            MyRollcallElement.playerNamesAvailable = new List<string>();
          }
          else
          {
            MyRollcallElement.playerNamesAvailable = names;
            names.Insert(0, "P"); // Add default name which will be display like P1 P2.. P8
          }
        }
        catch (Exception ex)
        {
          TFGame.Log(ex, true);
          TFGame.OpenLog();
          Engine.Instance.Exit();
        }
      });
    }

    public static string RemoveDiacritics(string text)
    {
      if (string.IsNullOrEmpty(text))
        return text;

      var normalizedString = text.Normalize(NormalizationForm.FormD);
      var stringBuilder = new StringBuilder();

      foreach (var c in normalizedString)
      {
        var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
        if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
        {
          stringBuilder.Append(c);
        }
      }

      return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

  }
}