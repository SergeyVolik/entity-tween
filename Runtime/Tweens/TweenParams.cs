using Timespawn.EntityTween.Math;
using Timespawn.EntityTween.Tweens;
using Unity.Entities;

namespace Timespawn.EntityTween
{
    public struct TweenParams
    {
        public float Duration;
        public EaseType EaseType;
        public bool IsPingPong;
        public byte LoopCount;
        public float StartDelay;
        public bool FromEntityPos;
        public TweenParams(
            in float duration, 
            in EaseType easeType = EaseType.Linear,
            in bool isPingPong = false, 
            in int loopCount = 1, 
            in float startDelay = 0.0f,
            in bool fromEntityPos = false)
        {
            Duration = duration;
            EaseType = easeType;
            IsPingPong = isPingPong;
            LoopCount = (byte) loopCount;
            StartDelay = startDelay;
            FromEntityPos = fromEntityPos;
        }

        public override string ToString()
        {
            string msg = $"{Duration} secs";

           
            msg += $", {EaseType}";
            

            if (IsPingPong)
            {
                msg += ", pingpong";
            }

            if (LoopCount != 1)
            {
                msg += LoopCount == 0 ? ", infinite" : $", {LoopCount} times";
            }

            if (StartDelay > 0.0f)
            {
                msg += $", delayed {StartDelay} secs";
            }

            return msg;
        }
    }
}