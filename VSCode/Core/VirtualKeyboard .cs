using Microsoft.Xna.Framework;
using TowerFall;
using Monocle;
using System;
using System.Collections.Generic;

namespace TFModFortRiseCustomName
{
  public class VirtualKeyboard : Entity
  {
    private int playerIndex;
    private string currentName = "";
    public static bool KeyboardActive = false;
    //private List<Text> existingNameTexts = new List<Text>();

    private List<Button> buttons = new List<Button>();
    private Text title;
    private Text inputDisplay;

    private bool initialized = false;

    private int selected = 0;       // index du bouton sélectionné
    private float stickDelay = 0.15f;
    private float stickTimer = 0f;

    private class Button
    {
      public string Label;
      public Vector2 Pos;
      public Action Action;
      public Text Text;

      public Button(string label, Vector2 pos, Action action)
      {
        Label = label;
        Pos = pos;
        Action = action;

        Text = new Text(
            TFGame.Font,
            label,
            pos,
            Color.White,
            Text.HorizontalAlign.Center,
            Text.VerticalAlign.Center
        );
      }

      public void Highlight(bool active)
      {
        Text.Color = active ? Color.Yellow : Color.White;
        Text.Scale = active ? new Vector2(1.3f, 1.3f) : Vector2.One;
      }

      public void Render()
      {
        Text.Render();
      }
    }

    public VirtualKeyboard(int playerIndex)
    {
      //Tag = Tags.PauseUpdate;
      Depth = -100000;
      this.playerIndex = playerIndex;
      VirtualKeyboard.KeyboardActive = true;  // active le blocage
    }

    private void Init()
    {
      if (initialized) return;
      initialized = true;

      title = new Text(TFGame.Font, "ENTREZ UN NOM", new Vector2(160, 40), Color.White);
      inputDisplay = new Text(TFGame.Font, "> ", new Vector2(160, 60), Color.Yellow);

      Add(title);
      Add(inputDisplay);

      GenerateKeyboard();
      UpdateDisplay();
      UpdateHighlight();
    }

    private void GenerateKeyboard()
    {
      float startX = 100;
      float startY = 80;
      float spacing = 14;

      string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 .'-_?!:()\\/";

      for (int i = 0; i < letters.Length; i++)
      {
        int col = i % 10;
        int row = i / 10;
        char c = letters[i];
        Vector2 pos = new Vector2(startX + col * spacing, startY + row * spacing);

        buttons.Add(new Button(c.ToString(), pos, () =>
        {
          currentName += c;
          UpdateDisplay();
        }));
      }

      buttons.Add(new Button("DEL", new Vector2(100, 160), () =>
      {
        if (currentName.Length > 0)
          currentName = currentName.Substring(0, currentName.Length - 1);
        UpdateDisplay();
      }));

      buttons.Add(new Button("OK", new Vector2(230, 160), () =>
      {
        if (NameAlreadyExists(currentName))
        {
          // Prevent validation, maybe play a sound later
          return;  // Name is invalid
        }

        if (currentName.Length == 0)
          // Prevent validation, maybe play a sound later
          return;  // Name is invalid

        MyRollcallElement.playerNamesAvailable.Add(currentName);
        MyRollcallElement.SetPlayerName(playerIndex, currentName);
        VirtualKeyboard.KeyboardActive = false; // libère les inputs
        RemoveSelf();
      }));
    }

    private void UpdateDisplay()
    {
      if (inputDisplay == null)
        return;

      string baseText = "> " + currentName;

      if (currentName.Length > 0 && NameAlreadyExists(currentName))
      {
        // Red warning suffix
        baseText += "  (ALREADY EXISTS)";
        inputDisplay.DrawText = baseText;
      } 
      else if (currentName.Length == 0)
      {
        // Red warning suffix
        inputDisplay.DrawText = baseText;
      }
      else
      {
        inputDisplay.DrawText = baseText;
      }
    }


    private void UpdateHighlight()
    {
      for (int i = 0; i < buttons.Count; i++)
        buttons[i].Highlight(i == selected);
    }

    public override void Update()
    {
      if (!initialized && Scene != null)
        Init();

      base.Update();

      if (!initialized)
        return;

      var input = TFGame.PlayerInputs[playerIndex];

      stickTimer -= Engine.DeltaTime;

      // >>> Navigation horizontale
      if (stickTimer <= 0f)
      {
        if (input.MenuLeft)
        {
          selected = Math.Max(0, selected - 1);
          stickTimer = stickDelay;
        }
        else if (input.MenuRight)
        {
          selected = Math.Min(buttons.Count - 1, selected + 1);
          stickTimer = stickDelay;
        }
        else if (input.MenuUp)
        {
          selected -= 10;
          if (selected < 0) selected = 0;
          stickTimer = stickDelay;
        }
        else if (input.MenuDown)
        {
          selected += 10;
          if (selected >= buttons.Count) selected = buttons.Count - 1;
          stickTimer = stickDelay;
        }
      }

      UpdateHighlight();

      // >>> Valider bouton
      if (input.MenuConfirm)
      {
        buttons[selected].Action?.Invoke();
      }

      //// >>> Annuler
      if (input.MenuBack)
      {
        VirtualKeyboard.KeyboardActive = false; // libère les inputs
        RemoveSelf();
      }
    }

    private bool NameAlreadyExists(string name)
    {
      return MyRollcallElement.playerNamesAvailable.Contains(name);
    }


    public override void Render()
    {
      Draw.Rect(0, 0, 320, 240, new Color(0, 0, 0, 180));
      base.Render();

      //foreach (var t in existingNameTexts)
      //  t.Render();

      foreach (var b in buttons)
        b.Render();
    }
  }
}
