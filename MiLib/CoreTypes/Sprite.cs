using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiLib.CoreTypes
{
    public class Sprite : ISprite
    {
        protected VertexBuffer vertexBuffer;
        protected VertexPositionColorTexture[] vertexPositions;
        public Color Color
        {
            get
            {
                return new Color(basicEffect.DiffuseColor.X, basicEffect.DiffuseColor.Y, basicEffect.DiffuseColor.Z, basicEffect.Alpha);
            }
            set
            {
                basicEffect.DiffuseColor = value.ToVector3();
                basicEffect.Alpha = value.A/255f;
            }
        }

        protected BasicEffect basicEffect;

        public Matrix World { private set; get; }

        protected Vector3 scaleVec;

        protected Matrix scale;
        public Vector3 Scale
        {
            get
            {
                return scaleVec;
            }
            set
            {
                scaleVec = value;
                scale = Matrix.CreateScale(value.X * (sourceRectangle.Z - sourceRectangle.X), value.Y * (sourceRectangle.W - sourceRectangle.Y), value.Z);
            }
        }

        protected Matrix position;
        public Vector3 Position
        {
            get
            {
                return position.Translation;
            }
            set
            {
                position = Matrix.CreateTranslation(value);
            }
        }

        //Must make Quarterian: protected Matrix rotation;
        public Vector3 Rotation
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get
            {
                return basicEffect.Texture;
            }
            set
            {
                basicEffect.Texture = value;
            }
        }

        protected Matrix origin;
        public Vector3 Origin
        {
            get
            {
                return origin.Translation;
            }
            set
            {
                origin = Matrix.CreateTranslation(value);
            }
        }

        protected Vector4 sourceRectangle;
        public Rectangle SourceRectangle
        {
            get
            {
                return new Rectangle((int)(sourceRectangle.X * Texture.Width), (int)(sourceRectangle.Y * Texture.Height), (int)(sourceRectangle.Z * Texture.Width), (int)(sourceRectangle.W * Texture.Height));
            }
            set
            {
                sourceRectangle = new Vector4(value.X / (float)Texture.Width, value.Y / (float)Texture.Height, value.Width / (float)Texture.Width, value.Height / (float)Texture.Height);
                sourceRectangle.Z += sourceRectangle.X;
                sourceRectangle.W += sourceRectangle.Y;

                vertexPositions[0] = new VertexPositionColorTexture(vertexPositions[0].Position, vertexPositions[0].Color, new Vector2(sourceRectangle.Z, sourceRectangle.W));
                vertexPositions[1] = new VertexPositionColorTexture(vertexPositions[1].Position, vertexPositions[1].Color, new Vector2(sourceRectangle.X, sourceRectangle.W));
                vertexPositions[2] = new VertexPositionColorTexture(vertexPositions[2].Position, vertexPositions[2].Color, new Vector2(sourceRectangle.Z, sourceRectangle.Y));
                vertexPositions[3] = new VertexPositionColorTexture(vertexPositions[3].Position, vertexPositions[3].Color, new Vector2(sourceRectangle.X, sourceRectangle.Y));
                vertexBuffer.SetData<VertexPositionColorTexture>(vertexPositions);

                scale = Matrix.CreateScale(Scale.X * (sourceRectangle.Z - sourceRectangle.X), Scale.Y * (sourceRectangle.W - sourceRectangle.Y), Scale.Z);

            }
        }

        protected String name;
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool IsUpdating
        {
            get;
            set;
        }

        public bool IsVisible
        {
            get;
            set;
        }

        public Sprite()
            : this("Unknown")
        { }

        public Sprite(String name)
        {
            Texture2D texture = new Texture2D(WindowManager.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            Initialize(name, texture);
        }

        public Sprite(String name, Texture2D texture)
        {
            Initialize(name, texture);
        }

        private void Initialize(String name, Texture2D texture)
        {
            this.name = name;
            basicEffect = new BasicEffect(texture.GraphicsDevice);
            basicEffect.TextureEnabled = true;
            basicEffect.VertexColorEnabled = true;
            basicEffect.Name = string.Format("{0} BasicEffect", name);
            basicEffect.Texture = texture;

            sourceRectangle = new Vector4(0, 0, 1f, 1f);
            origin = Matrix.Identity;
            scale = Matrix.Identity;
            Rotation = Vector3.Zero;
            position = Matrix.Identity;

            vertexPositions = new VertexPositionColorTexture[4];
            vertexPositions[0] = new VertexPositionColorTexture(new Vector3(0f, 0f, 0), Color.White, new Vector2(sourceRectangle.Z, sourceRectangle.W));
            vertexPositions[1] = new VertexPositionColorTexture(new Vector3(1f, 0f, 0), Color.White, new Vector2(sourceRectangle.X, sourceRectangle.W));
            vertexPositions[2] = new VertexPositionColorTexture(new Vector3(0f, 1f, 0), Color.White, new Vector2(sourceRectangle.Z, sourceRectangle.Y));
            vertexPositions[3] = new VertexPositionColorTexture(new Vector3(1f, 1f, 0), Color.White, new Vector2(sourceRectangle.X, sourceRectangle.Y));

            vertexBuffer = new VertexBuffer(texture.GraphicsDevice, typeof(VertexPositionColorTexture), vertexPositions.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionColorTexture>(vertexPositions);

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Render()
        {
        }

        public virtual void Draw(ICamera camera)
        {
            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;
            World = origin * scale * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * position;
            basicEffect.World = World;

            vertexBuffer.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                vertexBuffer.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, vertexBuffer.VertexCount - 2);
            }

            vertexBuffer.GraphicsDevice.SetVertexBuffer(null);
        }
    }
}
