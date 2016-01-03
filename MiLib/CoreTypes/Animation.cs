using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;

namespace MiLib.CoreTypes
{
    public enum AnimationType
    {
        Normal,
        PingPong
    }

    public class Animation
    {
        private AnimationType animationType;

        public AnimationType AnimationType { get { return animationType; } set { animationType = value; if (animationType == AnimationType.PingPong) pingpongDirection = IsReverse ? -1 : 1; } }
        public bool IsLooping { get; set; }

        private int pingpongDirection = 1;
        private bool isReverse;
        public bool IsReverse { get { return isReverse; } set { isReverse = value; startindex = value ? frames.Length - 1 : 0; if (animationType == AnimationType.PingPong) pingpongDirection = IsReverse ? -1 : 1; } }
        private TimeSpan elapsedTime;
        public TimeSpan AnimationSpeed { get; set; }
        public Frame Frame
        {
            get
            {
                return frames[CurrentFrame];
            }
        }
        private bool stop;

        public int CurrentFrame { get; protected set; }
        private Frame[] frames;
        private int startindex;

        public Animation(IEnumerable<Frame> frames)
            : this(AnimationType.Normal, false, true, TimeSpan.FromMilliseconds(250), frames)
        { }

        public Animation(AnimationType animationType, bool isReverse, bool isLooping, TimeSpan animationSpeed, IEnumerable<Frame> frames)
        {
            AnimationType = animationType;
            IsLooping = isLooping;
            AnimationSpeed = animationSpeed;
            this.frames = frames.ToArray();
            elapsedTime = new TimeSpan();
            IsReverse = isReverse;
            CurrentFrame = startindex;
            stop = false;
        }

        public void Reset()
        {
            elapsedTime = new TimeSpan();
            CurrentFrame = startindex;
            stop = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!stop)
            {
                elapsedTime += gameTime.ElapsedGameTime;
                if (elapsedTime >= AnimationSpeed)
                {
                    elapsedTime = TimeSpan.Zero;
                    switch (AnimationType)
                    {
                        case AnimationType.Normal:
                            if (IsReverse)
                            {
                                CurrentFrame--;
                                if (CurrentFrame < 0)
                                {
                                    if (IsLooping)
                                    {
                                        CurrentFrame = frames.Length - 1;
                                    }
                                    else
                                    {
                                        CurrentFrame = 0;
                                        stop = true;
                                    }
                                }
                            }
                            else
                            {
                                CurrentFrame++;
                                if (CurrentFrame > frames.Length - 1)
                                {
                                    if(IsLooping)
                                    {
                                        CurrentFrame = 0;
                                    }
                                    else
                                    {
                                        CurrentFrame = frames.Length - 1;
                                        stop = true;
                                    }
                                }
                            }
                            break;
                        case AnimationType.PingPong:
                            CurrentFrame += pingpongDirection;
                            if (CurrentFrame <= 0)
                            {
                                CurrentFrame = 0;
                                pingpongDirection *= -1;
                                if(!IsReverse && !IsLooping)
                                {
                                    stop = true;
                                }
                            }
                            if (CurrentFrame >= frames.Length - 1)
                            {
                                CurrentFrame = frames.Length - 1;
                                pingpongDirection *= -1;
                                if(IsReverse && !IsLooping)
                                {
                                    stop = true;
                                }
                            }

                            break;
                    }
                }
            }
        }
    }
}
