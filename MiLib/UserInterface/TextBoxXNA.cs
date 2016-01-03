using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiLib.CoreTypes;

namespace MiLib.UserInterface
{
    public class TextBoxXNA : UIComponent
    {
        public Texture2D texture;

        Texture2D pixel;
        public Color SelectionColor = Color.Lerp(Color.Blue, Color.Transparent, 0.5f);

        SpriteFont font;
        public string Text;
        public Color TextColor = Color.Black;

        int selectionDistance = 0;

        public int SideMargin = 10;
        int cursorPosition = 0;
        TimeSpan elaspedBlinkTime = new TimeSpan();
        TimeSpan blinkTime = TimeSpan.FromMilliseconds(500);
        bool blinkOn = false;
        Keys currentKey;
        Keys lastKey;
        TimeSpan elaspedKeyPressed = new TimeSpan();
        TimeSpan keyPressedTime = TimeSpan.FromMilliseconds(500);
        Keys[] currentKeys;
        Keys[] lastKeys;
        bool isCaps = false;
        bool isInsert = false;
        bool isMultiLine = false;
        int cursorEndPosition = 0;
        int viewablePosition = 0;

        public bool isPassword = false;

        RenderTarget2D renderTarget;


        public TextBoxXNA(Texture2D image, Rectangle bounds, SpriteFont font)
            : base(bounds)
        {
            texture = image;
            pixel = new Texture2D(texture.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            renderTarget = new RenderTarget2D(image.GraphicsDevice, bounds.Width, bounds.Height);
            Text = "";
            this.font = font;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsFocused)
            {
                lastKey = currentKey;
                currentKeys = InputManager.KeysPressed;

                if (currentKeys.Length > 0 && lastKeys != null)
                {
                    if (currentKeys.Contains(Keys.CapsLock) && !lastKeys.Contains(Keys.CapsLock))
                    {
                        isCaps = !isCaps;
                    }
                    else if (currentKeys.Contains(Keys.Insert) && !lastKeys.Contains(Keys.Insert))
                    {
                        isInsert = !isInsert;
                    }
                    bool foundkey = false;
                    for (int i = 0; i < currentKeys.Length; i++)
                    {
                        if (!lastKeys.Contains(currentKeys[i]))
                        {
                            currentKey = currentKeys[i];
                            foundkey = true;
                            break;
                        }
                    }
                    if (!foundkey)
                    {
                        if (!currentKeys.Contains(currentKey))
                        {
                            currentKey = Keys.None;
                        }
                    }
                    bool pressed = false;
                    if (lastKey != currentKey)
                    {
                        keyPressedTime = TimeSpan.FromMilliseconds(500);
                        elaspedKeyPressed = new TimeSpan();
                        pressed = true;
                    }
                    else if (elaspedKeyPressed > keyPressedTime)
                    {
                        pressed = true;
                        keyPressedTime = TimeSpan.FromMilliseconds(75);
                        elaspedKeyPressed = new TimeSpan();
                    }
                    else
                    {
                        elaspedKeyPressed += gameTime.ElapsedGameTime;
                    }
                    if (pressed)
                    {
                        bool isShift = false;
                        if (currentKeys.Contains(Keys.RightShift) || currentKeys.Contains(Keys.LeftShift))
                        {
                            isCaps = !isCaps;
                            isShift = true;
                        }
                        switch (currentKey)
                        {
                            case Keys.A:
                            case Keys.B:
                            case Keys.C:
                            case Keys.D:
                            case Keys.E:
                            case Keys.F:
                            case Keys.G:
                            case Keys.H:
                            case Keys.I:
                            case Keys.J:
                            case Keys.K:
                            case Keys.L:
                            case Keys.M:
                            case Keys.N:
                            case Keys.O:
                            case Keys.P:
                            case Keys.Q:
                            case Keys.R:
                            case Keys.S:
                            case Keys.T:
                            case Keys.U:
                            case Keys.V:
                            case Keys.W:
                            case Keys.X:
                            case Keys.Y:
                            case Keys.Z:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? currentKey.ToString() : ((char)(currentKey.ToString()[0] + 32)).ToString());
                                cursorPosition++;
                                cursorEndPosition++;
                                /*if (isInsert && text.Length > 0 && cursorPosition < text.Length)
                                {
                                    text = text.Remove(cursorPosition, 1);
                                }*///works
                                break;
                            case Keys.D0:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? ")" : "0");
                                cursorPosition++;
                                cursorEndPosition++;
                                /*if (isInsert && text.Length > 0 && cursorPosition < text.Length)
                                {
                                    text = text.Remove(cursorPosition, 1);
                                }*///works
                                break;
                            case Keys.NumPad0:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "0");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.D1:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "!" : "1");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.NumPad1:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "1");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.D2:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "@" : "2");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.NumPad2:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "2");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.D3:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "#" : "3");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.NumPad3:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "3");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.D4:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "$" : "4");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.NumPad4:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "4");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.D5:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "%" : "5");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.NumPad5:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "5");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.D6:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "^" : "6");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.NumPad6:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "6");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.D7:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "&" : "7");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.NumPad7:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "7");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.D8:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "*" : "8");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.NumPad8:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "8");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.Multiply:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "*");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.D9:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "(" : "9");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.NumPad9:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "9");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.Add:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "+");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.OemMinus:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "_" : "-");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.Subtract:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, "-");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.OemPeriod:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? ">" : ".");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.Decimal:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, ".");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.Home:
                                if (isShift)
                                {
                                    if (selectionDistance > 0)
                                    {
                                        cursorEndPosition = cursorPosition;
                                    }
                                    cursorPosition = 0;
                                    selectionDistance = cursorPosition - cursorEndPosition;
                                }
                                else
                                {
                                    cursorPosition = 0;
                                    cursorEndPosition = cursorPosition;
                                }
                                break;
                            case Keys.Insert:
                                //isInsert = true;
                                break;
                            case Keys.OemComma:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "<" : ",");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.OemBackslash:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "|" : @"\");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.OemPipe:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "|" : @"\");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.Divide:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, @"\");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.OemCloseBrackets:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "}" : "]");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.OemOpenBrackets:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "{" : "[");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.OemPlus:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "+" : "=");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.OemQuestion:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "/" : "?");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.OemQuotes:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? ((char)34).ToString() : "'");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.OemSemicolon:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? ":" : ";");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.OemTilde:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, isCaps ? "~" : "`");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.Tab:
                                break;
                            case Keys.CapsLock:
                                //isShift = !isShift;
                                break;
                            case Keys.RightShift:
                                //isShift = !isShift;
                                break;
                            case Keys.LeftShift:
                                //isShift = !isShift;
                                break;
                            case Keys.Space:
                                ClearSelected();
                                Text = Text.Insert(cursorPosition, " ");
                                cursorPosition++;
                                cursorEndPosition++;
                                break;
                            case Keys.Left:
                                if (cursorPosition > 0)
                                {
                                    if (isShift)
                                    {
                                        if (selectionDistance > 0)
                                        {
                                            cursorEndPosition--;
                                        }
                                        else
                                        {
                                            cursorPosition--;
                                        }
                                        selectionDistance--;
                                    }
                                    else
                                    {
                                        cursorPosition--;
                                        cursorEndPosition = cursorPosition;
                                        selectionDistance = 0;
                                    }
                                }
                                break;
                            case Keys.Right:
                                if (cursorPosition < Text.Length)
                                {
                                    if (isShift)
                                    {
                                        if (selectionDistance >= 0)
                                        {
                                            cursorEndPosition++;
                                        }
                                        else
                                        {
                                            cursorPosition++;
                                        }
                                        selectionDistance++;
                                    }
                                    else
                                    {
                                        cursorPosition++;
                                        cursorEndPosition = cursorPosition;
                                        selectionDistance = 0;
                                    }
                                }
                                break;
                            case Keys.Back:
                                if (selectionDistance != 0)
                                {
                                    ClearSelected();
                                }
                                else if (Text.Length > 0 && cursorPosition > 0)
                                {
                                    Text = Text.Remove(cursorPosition - 1, 1);
                                    cursorPosition--;
                                }
                                break;
                            case Keys.Delete:
                                if (selectionDistance != 0)
                                {
                                    ClearSelected();
                                }
                                else if (Text.Length > 0 && cursorPosition < Text.Length)
                                {
                                    Text = Text.Remove(cursorPosition, 1);
                                }
                                break;
                            case Keys.End:
                                if (isShift)
                                {
                                    if (selectionDistance < 0)
                                    {
                                        cursorPosition = cursorEndPosition;
                                    }
                                    cursorEndPosition = Text.Length;
                                    selectionDistance = cursorEndPosition - cursorPosition;
                                }
                                else
                                {
                                    cursorPosition = Text.Length;
                                    cursorEndPosition = cursorPosition;
                                }
                                break;
                            case Keys.Enter:
                                if (isMultiLine)
                                {
                                    Text = Text.Insert(cursorPosition, "\n");
                                    cursorPosition++;
                                    cursorEndPosition++;
                                }
                                break;
                            case Keys.Escape:
                                IsFocused = false;
                                break;
                            case Keys.Apps:
                                break;
                            case Keys.Attn:
                                break;
                            case Keys.BrowserBack:
                                break;
                            case Keys.BrowserFavorites:
                                break;
                            case Keys.BrowserForward:
                                break;
                            case Keys.BrowserHome:
                                break;
                            case Keys.BrowserRefresh:
                                break;
                            case Keys.BrowserSearch:
                                break;
                            case Keys.BrowserStop:
                                break;
                            case Keys.ChatPadGreen:
                                break;
                            case Keys.ChatPadOrange:
                                break;
                            case Keys.Crsel:
                                break;
                            case Keys.Down:
                                break;
                            case Keys.EraseEof:
                                break;
                            case Keys.Execute:
                                break;
                            case Keys.Exsel:
                                break;
                            case Keys.F1:
                                break;
                            case Keys.F10:
                                break;
                            case Keys.F11:
                                break;
                            case Keys.F12:
                                break;
                            case Keys.F13:
                                break;
                            case Keys.F14:
                                break;
                            case Keys.F15:
                                break;
                            case Keys.F16:
                                break;
                            case Keys.F17:
                                break;
                            case Keys.F18:
                                break;
                            case Keys.F19:
                                break;
                            case Keys.F2:
                                break;
                            case Keys.F20:
                                break;
                            case Keys.F21:
                                break;
                            case Keys.F22:
                                break;
                            case Keys.F23:
                                break;
                            case Keys.F24:
                                break;
                            case Keys.F3:
                                break;
                            case Keys.F4:
                                break;
                            case Keys.F5:
                                break;
                            case Keys.F6:
                                break;
                            case Keys.F7:
                                break;
                            case Keys.F8:
                                break;
                            case Keys.F9:
                                break;
                            case Keys.Help:
                                break;
                            case Keys.ImeConvert:
                                break;
                            case Keys.ImeNoConvert:
                                break;
                            case Keys.Kana:
                                break;
                            case Keys.Kanji:
                                break;
                            case Keys.LaunchApplication1:
                                break;
                            case Keys.LaunchApplication2:
                                break;
                            case Keys.LaunchMail:
                                break;
                            case Keys.LeftAlt:
                                break;
                            case Keys.LeftControl:
                                break;
                            case Keys.LeftWindows:
                                break;
                            case Keys.MediaNextTrack:
                                break;
                            case Keys.MediaPlayPause:
                                break;
                            case Keys.MediaPreviousTrack:
                                break;
                            case Keys.MediaStop:
                                break;
                            case Keys.None:
                                break;
                            case Keys.NumLock:
                                break;
                            case Keys.Oem8:
                                break;
                            case Keys.OemAuto:
                                break;
                            case Keys.OemClear:
                                break;
                            case Keys.OemCopy:
                                break;
                            case Keys.OemEnlW:
                                break;
                            case Keys.Pa1:
                                break;
                            case Keys.PageDown:
                                break;
                            case Keys.PageUp:
                                break;
                            case Keys.Pause:
                                break;
                            case Keys.Play:
                                break;
                            case Keys.Print:
                                break;
                            case Keys.PrintScreen:
                                break;
                            case Keys.ProcessKey:
                                break;
                            case Keys.RightAlt:
                                break;
                            case Keys.RightControl:
                                break;
                            case Keys.RightWindows:
                                break;
                            case Keys.Scroll:
                                break;
                            case Keys.Select:
                                break;
                            case Keys.SelectMedia:
                                break;
                            case Keys.Separator:
                                break;
                            case Keys.Sleep:
                                break;
                            case Keys.Up:
                                break;
                            case Keys.VolumeDown:
                                break;
                            case Keys.VolumeMute:
                                break;
                            case Keys.VolumeUp:
                                break;
                            case Keys.Zoom:
                                break;
                            default:
                                break;
                        }
                        if (isShift)
                        {
                            isCaps = !isCaps;
                        }
                    }
                }
                else
                {
                    elaspedKeyPressed = new TimeSpan();
                    currentKey = Keys.None;
                }
                elaspedBlinkTime += gameTime.ElapsedGameTime;
                if (elaspedBlinkTime >= blinkTime)
                {
                    elaspedBlinkTime = new TimeSpan();
                    blinkOn = !blinkOn;
                }
            }
            if (InputManager.IsLeftClicked())
            {
                if (InputManager.IsLeftClicked(bounds))
                {
                    IsFocused = true;
                }
                else
                {
                    IsFocused = false;
                    blinkOn = false;
                }
            }
            lastKeys = currentKeys;
            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            texture.GraphicsDevice.SetRenderTarget(renderTarget);

            texture.GraphicsDevice.Clear(Color.Transparent);
            cursorPosition = MathHelper.Clamp(cursorPosition, 0, Text.Length);
            cursorEndPosition = MathHelper.Clamp(cursorEndPosition, 0, Text.Length);
            Vector2 cursorVect = new Vector2(4 + font.MeasureString(Text.Substring(viewablePosition, cursorPosition == 0 ? 0 : cursorPosition)).X, (bounds.Height - font.MeasureString(Text.Length > 0 ? Text : "0").Y)/2 - 4);
            Vector2 endCursorVect = new Vector2(4 + font.MeasureString(Text.Substring(viewablePosition, cursorEndPosition == 0 ? 0 : cursorEndPosition)).X, bounds.Height - font.MeasureString(Text.Length > 0 ? Text : "0").Y - 4);
            string pass = "";
            for (int i = 0; i < Text.Length; i++)
            {
                pass += "*";
            }
            if (isPassword)
            {
                cursorVect = new Vector2(4 + font.MeasureString(pass.Substring(viewablePosition, cursorPosition == 0 ? 0 : cursorPosition)).X, bounds.Height - font.MeasureString(pass).Y - 4);
            }

            float offset = 0;
            if (cursorVect.X > bounds.Width - 20)
            {
                offset = cursorVect.X - bounds.Width + 20;
            }
            spriteBatch.Begin();
            if (isPassword)
            {
                spriteBatch.DrawString(font, pass, new Vector2(4 - offset, cursorVect.Y), TextColor);
            }
            else
            {
                spriteBatch.DrawString(font, isMultiLine ? Text : Text.Replace("\n", ""), new Vector2(4 - offset, cursorVect.Y), TextColor);
            }
            if (cursorEndPosition != cursorPosition)
            {
                spriteBatch.Draw(pixel, new Rectangle((int)cursorVect.X, 0, (int)(endCursorVect.X - cursorVect.X), (int)cursorVect.Y * 100), SelectionColor);
            }
            else if (blinkOn)
            {
                spriteBatch.DrawString(font, "|", new Vector2(cursorVect.X - offset, cursorVect.Y), TextColor);
            }
            spriteBatch.End();

            texture.GraphicsDevice.SetRenderTarget(null);
            base.Render(spriteBatch);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);

            spriteBatch.Draw(renderTarget, new Rectangle(bounds.X + SideMargin, bounds.Y, bounds.Width - SideMargin * 2, bounds.Height), Color.White);

            base.Draw(spriteBatch);
        }

        private void ClearSelected()
        {
            if (cursorPosition != cursorEndPosition)
            {
                Text = Text.Remove(cursorPosition, cursorEndPosition - cursorPosition);
            }
            cursorEndPosition = cursorPosition;
            selectionDistance = 0;
        }
    }
}
