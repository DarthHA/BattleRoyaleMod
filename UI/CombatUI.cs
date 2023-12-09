using BattleRoyaleMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace BattleRoyaleMod.UI
{
    public class CombatUI : UIState
    {
        public bool VisibleList = false;
        private string result = "";
        private bool[] LastTeam = new bool[] { false, false, false, false, false, false, false, false };
        public override void OnInitialize()
        {
        }

        public override void OnDeactivate()
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            result = Language.GetTextValue("Mods.BattleRoyaleMod.EnemyList") + "\n";
            result += (BattleRoyaleMod.BattleStart ? Language.GetTextValue("Mods.BattleRoyaleMod.BattleStatusOn") : Language.GetTextValue("Mods.BattleRoyaleMod.BattleStatusOff")) + "\n";
            Dictionary<string, int> CombatList = new();
            bool[] CurrentTeam = new bool[] { false, false, false, false, false, false, false, false };
            bool Decreased = false;


            for (int i = 1; i <= 8; i++)
            {
                CombatList.Clear();
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && npc.GetFraction() == i && !npc.IsNPCBullet())
                    {
                        CurrentTeam[i - 1] = true;
                        if (CombatList.ContainsKey(Lang.GetNPCNameValue(npc.type)))
                        {
                            CombatList[Lang.GetNPCNameValue(npc.type)]++;
                        }
                        else
                        {
                            CombatList.Add(Lang.GetNPCNameValue(npc.type), 1);
                        }
                    }
                }


                if (CombatList.Count > 0)
                {
                    result += string.Format(Language.GetTextValue("Mods.BattleRoyaleMod.Team"), i.ToString());
                    result += "\n";
                    string ListStr = "";
                    foreach (string name in CombatList.Keys)
                    {
                        if (CombatList[name] > 1)
                        {
                            ListStr += name + " x" + CombatList[name];
                        }
                        else
                        {
                            ListStr += name;
                        }
                        ListStr += ", ";
                    }
                    int length = Language.ActiveCulture.LegacyId == (int)GameCulture.CultureName.Chinese ? 50 : 90;
                    ListStr = Wrap(ListStr, length);
                    result += ListStr;
                    result += "\n";
                }

                if (!CurrentTeam[i - 1] && LastTeam[i - 1])
                {
                    if (BattleRoyaleMod.BattleStart)
                    {
                        Main.NewText(string.Format(Language.GetTextValue("Mods.BattleRoyaleMod.TeamDefeated"), i.ToString()), Color.PaleVioletRed);
                    }
                    Decreased = true;
                }
                LastTeam[i - 1] = CurrentTeam[i - 1];
            }

            bool Single = false;
            int winteam = 0;
            for (int i = 0; i < 8; i++)
            {
                if (!Single && LastTeam[i])
                {
                    Single = true;
                    winteam = i + 1;
                }
                else if (Single && LastTeam[i])
                {
                    Single = false;
                    break;
                }
            }
            if (Single && Decreased)
            {
                if (BattleRoyaleMod.BattleStart)
                {
                    Main.NewText(string.Format(Language.GetTextValue("Mods.BattleRoyaleMod.TeamWin"), winteam.ToString()), Color.Green);
                }
            }

        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (VisibleList)
            {
                if (!Main.gameMenu && Main.myPlayer != -1 && Main.LocalPlayer.active && Main.LocalPlayer.GetModPlayer<YiYanDingZhenPlayer>().DingZhen)
                {
                    Vector2 DrawPos = new(60, 200);
                    Utils.DrawBorderString(spriteBatch, result, DrawPos, Color.White);
                }
            }


            if (!Main.gameMenu && Main.myPlayer != -1 && Main.LocalPlayer.active)
            {

                if (BattleRoyaleMod.Gconfig.UseBorderSystem && Main.LocalPlayer.GetModPlayer<YiYanDingZhenPlayer>().ShowBorder)
                {
                    float Left = BattleRoyaleMod.TopLeft.X;
                    float Right = BattleRoyaleMod.BottomRight.X;
                    float Top = BattleRoyaleMod.TopLeft.Y;
                    float Bottom = BattleRoyaleMod.BottomRight.Y;
                    DrawLineInScreen(true, Top, Left, Right);
                    DrawLineInScreen(true, Bottom, Left, Right);
                    DrawLineInScreen(false, Left, Top, Bottom);
                    DrawLineInScreen(false, Right, Top, Bottom);
                }
            }
        }

        internal static string Wrap(string v, int size)
        {
            v = v.TrimStart();
            if (v.Length <= size) return v;
            var nextspace = v.LastIndexOf(' ', size);
            if (-1 == nextspace) nextspace = Math.Min(v.Length, size);
            return v[..nextspace] + ((nextspace >= v.Length) ?
            "" : "\n" + Wrap(v[nextspace..], size));
        }


        internal void DrawLineInScreen(bool X, float value, float start, float end)
        {
            start = Math.Min(start, end);
            end = Math.Max(start, end);

            if (X)
            {
                if (value < Main.screenPosition.Y || value > Main.screenPosition.Y + Main.screenHeight) return;
                if (start < Main.screenPosition.X) start = Main.screenPosition.X;
                if (end > Main.screenPosition.X + Main.screenWidth) end = Main.screenPosition.X + Main.screenWidth;

                Matrix UIMatrix = GetMatrix();
                AnotherDraw(Main.GameViewMatrix.ZoomMatrix);
                Utils.DrawLine(Main.spriteBatch, new Vector2(start, value), new Vector2(end, value), Color.Red, Color.Red, 4);
                AnotherDraw(UIMatrix);
            }
            else
            {
                if (value < Main.screenPosition.X || value > Main.screenPosition.X + Main.screenWidth) return;
                if (start < Main.screenPosition.Y) start = Main.screenPosition.Y;
                if (end > Main.screenPosition.Y + Main.screenHeight) end = Main.screenPosition.Y + Main.screenHeight;
                Matrix UIMatrix = GetMatrix();
                AnotherDraw(Main.GameViewMatrix.ZoomMatrix);
                Utils.DrawLine(Main.spriteBatch, new Vector2(value, start), new Vector2(value, end), Color.Red, Color.Red, 4);
                AnotherDraw(UIMatrix);
            }

        }

        internal static void AnotherDraw(Matrix matrix)
        {

            Main.spriteBatch.End();

            FieldInfo fieldInfo = Main.spriteBatch.GetType().GetField("samplerState", BindingFlags.NonPublic | BindingFlags.Instance);
            SamplerState samplerState = (SamplerState)fieldInfo.GetValue(Main.spriteBatch);

            fieldInfo = Main.spriteBatch.GetType().GetField("blendState", BindingFlags.NonPublic | BindingFlags.Instance);
            BlendState blendState = (BlendState)fieldInfo.GetValue(Main.spriteBatch);

            fieldInfo = Main.spriteBatch.GetType().GetField("depthStencilState", BindingFlags.NonPublic | BindingFlags.Instance);
            DepthStencilState depthStencilState = (DepthStencilState)fieldInfo.GetValue(Main.spriteBatch);

            fieldInfo = Main.spriteBatch.GetType().GetField("rasterizerState", BindingFlags.NonPublic | BindingFlags.Instance);
            RasterizerState rasterizerState = (RasterizerState)fieldInfo.GetValue(Main.spriteBatch);

            fieldInfo = Main.spriteBatch.GetType().GetField("customEffect", BindingFlags.NonPublic | BindingFlags.Instance);
            Effect effect = (Effect)fieldInfo.GetValue(Main.spriteBatch);

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }

        internal static Matrix GetMatrix()
        {
            FieldInfo fieldInfo = Main.spriteBatch.GetType().GetField("transformMatrix", BindingFlags.NonPublic | BindingFlags.Instance);
            return (Matrix)fieldInfo.GetValue(Main.spriteBatch);
        }
    }


    public class UISystem : ModSystem
    {
        public CombatUI _CombatUI;
        public UserInterface _CombatUIUserInterface;

        public override void Load()
        {
            _CombatUI = new CombatUI();
            _CombatUI.Activate();
            _CombatUIUserInterface = new UserInterface();
            _CombatUIUserInterface.SetState(_CombatUI);
        }


        public override void UpdateUI(GameTime gameTime)
        {
            UpdatePlayerUI();

            _CombatUIUserInterface?.Update(gameTime);

            base.UpdateUI(gameTime);
        }

        public void UpdatePlayerUI()
        {
            if (Main.playerInventory)
            {
                _CombatUI.VisibleList = false;
            }
            else
            {
                _CombatUI.VisibleList = true;
            }

        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (inventoryIndex != -1)
            {
                layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
                    "SuperSmash : _CombatUI",
                    delegate
                    {
                        _CombatUIUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
            base.ModifyInterfaceLayers(layers);
        }
    }
}