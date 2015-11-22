using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.CoreTypes
{
    public enum AnimationType
    {
        Normal,
        Reverse,
        PingPong
    }

    public class Animation
    {
        public AnimationType AnimationType { get; set; }
        public bool IsLooping { get; set; }
        private bool isReverse;
        private TimeSpan elapsedTime;
        public TimeSpan AnimationSpeed { get; set; }
        public Frame Frame
        {
            get
            {
                return frames[CurrentFrame];
            }
        }

        public Rectangle SourceRectangle
        {
            get
            {
                return frames[CurrentFrame].SourceRectangle;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return frames[CurrentFrame].Origin;
            }
        }

        public int CurrentFrame { get; protected set; }
        private Frame[] frames;

        public Animation(Frame[] frames)
            : this(AnimationType.Normal, true, TimeSpan.FromMilliseconds(250), frames)
        { }

        public Animation(List<Frame> frames)
            : this(AnimationType.Normal, true, TimeSpan.FromMilliseconds(250), frames.ToArray())
        { }

        public Animation(AnimationType animationType, bool isLooping, TimeSpan animationSpeed, List<Frame> frames)
            : this(animationType, isLooping, animationSpeed, frames.ToArray())
        { }
        public Animation(AnimationType animationType, bool isLooping, TimeSpan animationSpeed, Frame[] frames)
        {
            AnimationType = animationType;
            IsLooping = isLooping;
            AnimationSpeed = animationSpeed;
            this.frames = frames;
            CurrentFrame = 0;
            elapsedTime = new TimeSpan();
            switch (AnimationType)
            {
                case AnimationType.Normal:
                case AnimationType.PingPong:
                    isReverse = false;
                    break;
                case AnimationType.Reverse:
                    isReverse = true;
                    break;
            }
        }

        public void Reset()
        {
            elapsedTime = new TimeSpan();
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime >= AnimationSpeed)
            {
                elapsedTime = TimeSpan.FromMilliseconds(0);
                switch (AnimationType)
                {
                    case AnimationType.Normal:
                        CurrentFrame++;
                        if (CurrentFrame >= frames.Length)
                        {
                            CurrentFrame = IsLooping ? 0 : frames.Length - 1;
                        }
                        break;
                    case AnimationType.PingPong:
                        if (isReverse)
                        {
                            CurrentFrame--;
                            if (CurrentFrame <= 0)
                            {
                                CurrentFrame = 0;
                                if (IsLooping)
                                {
                                    isReverse = !isReverse;
                                }
                            }
                        }
                        else
                        {
                            CurrentFrame++;
                            if (CurrentFrame >= frames.Length - 1)
                            {
                                CurrentFrame = frames.Length - 1;
                                isReverse = !isReverse;
                            }
                        }
                        break;
                    case AnimationType.Reverse:
                        CurrentFrame--;
                        if (CurrentFrame <= 0)
                        {
                            CurrentFrame = IsLooping ? frames.Length - 1 : 0;
                        }
                        break;
                }
            }
        }
    }
}
