using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MiLib.CoreTypes;
using System;
using System.Collections.Generic;

namespace MiLib.UserInterface
{
    public class TextBox
    {
        List<List<TextCharacter>> paragraph = new List<List<TextCharacter>>();
        private List<TextCharacter> currentLine
        {
            get
            {
                return paragraph[paragraph.Count - 1];
            }
        }
        List<SpriteFont> fonts = new List<SpriteFont>();
        Vector2 position;
        Color textColor;
        Color backColor;

        Texture2D pixel;
        Vector2 size;
        Vector4 padding;
        Random random = new Random();

        float CharacterSpacing = 2;

        RenderTarget2D textRender;


        public float ScrollY = 0;
        public float ScrollX = 0;
        Boolean writeAble;

        public TextBox(GraphicsDevice graphicsDevice, ContentManager content, Vector2 position, Color textColor, Color backColor, Vector2 size, Vector4 padding, Boolean writable)
        {
            fonts.Add(content.Load<SpriteFont>("Arial12"));/*
            fonts.Add(content.Load<SpriteFont>("Arial20"));
            fonts.Add(content.Load<SpriteFont>("Arial26"));
            fonts.Add(content.Load<SpriteFont>("Arial40"));
            fonts.Add(content.Load<SpriteFont>("Arial52"));*/
            this.position = position;
            this.textColor = textColor;
            this.backColor = backColor;

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            this.size = size;
            this.padding = padding;

            WindowManager.Window.TextInput += GameWindow_TextInput;
            paragraph.Add(new List<TextCharacter>());

            this.writeAble = writeAble;

        }

        void GameWindow_TextInput(object sender, TextInputEventArgs e)
        {
            if ((int)e.Character == 8)
            {
                if (currentLine.Count == 0)
                {
                    if (paragraph.Count > 1)
                    {
                        paragraph.RemoveAt(paragraph.Count - 1);
                    }
                }
                else
                {
                    currentLine.RemoveAt(currentLine.Count - 1);
                }
            }
            else if ((int)e.Character == 22)
            {
                /* paste
                string word = Clipboard.GetText();
                for (int i = 0; i < word.Length; i++)
                {
                    parse(word[i]);
                }*/
            }
            else
            {
                parse(e.Character);
            }
        }

        private void parse(char chara)
        {
            if ((int)chara == 13)
            {
                paragraph.Add(new List<TextCharacter>());
            }
            else if ((int)chara == 9)
            {
                currentLine.Add(new TextCharacter(fonts[random.Next(fonts.Count)], "         ", new Color(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256))));
            }
            else
            {
                currentLine.Add(new TextCharacter(fonts[random.Next(fonts.Count)], chara.ToString(), new Color(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256))));
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            textRender = new RenderTarget2D(pixel.GraphicsDevice, (int)(size.X + 1 - (padding.X + padding.Z)), (int)(size.Y + 1 - (padding.Y + padding.W)));

            pixel.GraphicsDevice.SetRenderTarget(textRender);

            pixel.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin();

            float yoffset = 0;
            Vector2 lineMeasurement = new Vector2();
            yoffset += fonts[fonts.Count - 1].LineSpacing;
            foreach (List<TextCharacter> line in paragraph)
            {
                int oldindex = 0;
                while (oldindex < line.Count - 1)
                {
                    lineMeasurement = new Vector2(0, 0);
                    int spacingAmount = 0;
                    string lineText = "";
                    string oldlineText = "";
                    for (; oldindex < line.Count - 1; oldindex++)
                    {
                        lineText += line[oldindex].character;
                        lineMeasurement = new Vector2(line[oldindex].font.MeasureString(line[oldindex].character).X + lineMeasurement.X, Math.Max(lineMeasurement.Y, line[oldindex].font.MeasureString(line[oldindex].character).Y));
                        if (lineMeasurement.X + spacingAmount + line[oldindex].font.LineSpacing > size.X - (padding.X + padding.Z))
                        {
                            lineText = oldlineText;
                            break;
                        }
                        lineMeasurement -= new Vector2(line[oldindex].font.MeasureString(line[oldindex].character).X, 0);
                        spriteBatch.DrawString(line[oldindex].font, line[oldindex].character, new Vector2(ScrollX + lineMeasurement.X + spacingAmount * CharacterSpacing, yoffset + ScrollY), line[oldindex].color, 0, new Vector2(0, line[oldindex].font.MeasureString(lineText).Y), 1, SpriteEffects.None, 0);
                        oldlineText = lineText;
                        lineMeasurement += new Vector2(line[oldindex].font.MeasureString(line[oldindex].character).X, 0);
                        spacingAmount++;
                    }
                    yoffset += lineMeasurement.Y + 5;
                }
                if (line.Count == 0)
                {
                    yoffset += lineMeasurement.Y + 5;
                }
            }

            spriteBatch.End();

            pixel.GraphicsDevice.SetRenderTarget(null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, position, null, backColor, 0, Vector2.Zero, size, SpriteEffects.None, 0);
            spriteBatch.Draw(textRender, position + new Vector2(padding.X, padding.Y), Color.White);
        }
    }
}
