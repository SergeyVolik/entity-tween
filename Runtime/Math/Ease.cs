using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEditor;

namespace Timespawn.EntityTween.Math
{

    public enum EaseType : byte
    {
        Linear,
        QuadraticEaseIn,
        QuadraticEaseOut,
        QuadraticEaseInOut,
        CubicEaseIn,
        CubicEaseOut,
        CubicEaseInOut,
        QuarticEaseIn,
        QuarticEaseOut,
        QuarticEaseInOut,
        QuinticEaseIn,
        QuinticEaseOut,
        QuinticEaseInOut,
        SineEaseIn,
        SineEaseOut,
        SineEaseInOut,
        CircularEaseIn,
        CircularEaseOut,
        CircularEaseInOut,
        ExponentialEaseIn,
        ExponentialEaseOut,
        ExponentialEaseInOut,
        ElasticEaseIn,
        ElasticEaseOut,
        ElasticEaseInOut,
        BackEaseIn,
        BackEaseOut,
        BackEaseInOut,
        BounceEaseIn,
        BounceEaseInOut,

    }

    public static class Ease
    {
        public delegate float EaseFunction(in float t);

        private const float M_PI_2 = math.PI / 2f;

        // Modeled after the line y = x
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float LinearInterpolation(float p)
        {
            return p;
        }

        // Modeled after the parabola y = x^2
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float QuadraticEaseIn(float p)
        {
            return p * p;
        }

        // Modeled after the parabola y = -x^2 + 2x
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float QuadraticEaseOut(float p)
        {
            return -(p * (p - 2));
        }

        // Modeled after the piecewise quadratic
        // y = (1/2)((2x)^2)             ; [0, 0.5)
        // y = -(1/2)((2x-1)*(2x-3) - 1) ; [0.5, 1]
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float QuadraticEaseInOut(float p)
        {
            if (p < 0.5)
            {
                return 2 * p * p;
            }
            else
            {
                return (-2 * p * p) + (4 * p) - 1;
            }
        }

        // Modeled after the cubic y = x^3
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float CubicEaseIn(float p)
        {
            return p * p * p;
        }

        // Modeled after the cubic y = (x - 1)^3 + 1
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float CubicEaseOut(float p)
        {
            float f = (p - 1);
            return f * f * f + 1;
        }

        // Modeled after the piecewise cubic
        // y = (1/2)((2x)^3)       ; [0, 0.5)
        // y = (1/2)((2x-2)^3 + 2) ; [0.5, 1]
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float CubicEaseInOut(float p)
        {
            if (p < 0.5f)
            {
                return 4 * p * p * p;
            }
            else
            {
                float f = ((2 * p) - 2);
                return 0.5f * f * f * f + 1;
            }
        }

        // Modeled after the quartic x^4
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float QuarticEaseIn(float p)
        {
            return p * p * p * p;
        }

        // Modeled after the quartic y = 1 - (x - 1)^4
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float QuarticEaseOut(float p)
        {
            float f = (p - 1);
            return f * f * f * (1 - p) + 1;
        }

        // Modeled after the piecewise quartic
        // y = (1/2)((2x)^4)        ; [0, 0.5)
        // y = -(1/2)((2x-2)^4 - 2) ; [0.5, 1]
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float QuarticEaseInOut(float p)
        {
            if (p < 0.5)
            {
                return 8 * p * p * p * p;
            }
            else
            {
                float f = (p - 1);
                return -8 * f * f * f * f + 1;
            }
        }

        // Modeled after the quintic y = x^5
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float QuinticEaseIn(float p)
        {
            return p * p * p * p * p;
        }

        // Modeled after the quintic y = (x - 1)^5 + 1
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float QuinticEaseOut(float p)
        {
            float f = (p - 1);
            return f * f * f * f * f + 1;
        }

        // Modeled after the piecewise quintic
        // y = (1/2)((2x)^5)       ; [0, 0.5)
        // y = (1/2)((2x-2)^5 + 2) ; [0.5, 1]
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float QuinticEaseInOut(float p)
        {
            if (p < 0.5)
            {
                return 16 * p * p * p * p * p;
            }
            else
            {
                float f = ((2 * p) - 2);
                return 0.5f * f * f * f * f * f + 1;
            }
        }

        // Modeled after quarter-cycle of sine wave
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float SineEaseIn(float p)
        {
            return math.sin((p - 1) * M_PI_2) + 1;
        }

        // Modeled after quarter-cycle of sine wave (different phase)
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float SineEaseOut(float p)
        {
            return math.sin(p * M_PI_2);
        }

        // Modeled after half sine wave
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float SineEaseInOut(float p)
        {
            return 0.5f * (1 - math.cos(p * math.PI));
        }

        // Modeled after shifted quadrant IV of unit circle
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float CircularEaseIn(float p)
        {
            return 1 - math.sqrt(1 - (p * p));
        }

        // Modeled after shifted quadrant II of unit circle
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float CircularEaseOut(float p)
        {
            return math.sqrt((2 - p) * p);
        }

        // Modeled after the piecewise circular function
        // y = (1/2)(1 - sqrt(1 - 4x^2))           ; [0, 0.5)
        // y = (1/2)(sqrt(-(2x - 3)*(2x - 1)) + 1) ; [0.5, 1]
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float CircularEaseInOut(float p)
        {
            if (p < 0.5f)
            {
                return 0.5f * (1 - math.sqrt(1 - 4 * (p * p)));
            }
            else
            {
                return 0.5f * (math.sqrt(-((2 * p) - 3) * ((2 * p) - 1)) + 1);
            }
        }

        // Modeled after the exponential function y = 2^(10(x - 1))
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float ExponentialEaseIn(float p)
        {
            return (p == 0.0) ? p : math.pow(2, 10 * (p - 1));
        }

        // Modeled after the exponential function y = -2^(-10x) + 1
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float ExponentialEaseOut(float p)
        {
            return (p == 1.0) ? p : 1 - math.pow(2, -10 * p);
        }

        // Modeled after the piecewise exponential
        // y = (1/2)2^(10(2x - 1))         ; [0,0.5)
        // y = -(1/2)*2^(-10(2x - 1))) + 1 ; [0.5,1]
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float ExponentialEaseInOut(float p)
        {
            if (p == 0.0f || p == 1.0f) return p;

            if (p < 0.5f)
            {
                return 0.5f * math.pow(2, (20 * p) - 10);
            }
            else
            {
                return -0.5f * math.pow(2, (-20 * p) + 10) + 1;
            }
        }

        // Modeled after the damped sine wave y = sin(13pi/2*x)*pow(2, 10 * (x - 1))
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float ElasticEaseIn(float p)
        {
            return math.sin(13 * M_PI_2 * p) * math.pow(2, 10 * (p - 1));
        }

        // Modeled after the damped sine wave y = sin(-13pi/2*(x + 1))*pow(2, -10x) + 1
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float ElasticEaseOut(float p)
        {
            return math.sin(-13 * M_PI_2 * (p + 1)) * math.pow(2, -10 * p) + 1;
        }

        // Modeled after the piecewise exponentially-damped sine wave:
        // y = (1/2)*sin(13pi/2*(2*x))*pow(2, 10 * ((2*x) - 1))      ; [0,0.5)
        // y = (1/2)*(sin(-13pi/2*((2x-1)+1))*pow(2,-10(2*x-1)) + 2) ; [0.5, 1]
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float ElasticEaseInOut(float p)
        {
            if (p < 0.5f)
            {
                return 0.5f * math.sin(13 * M_PI_2 * (2 * p)) * math.pow(2, 10 * ((2 * p) - 1));
            }
            else
            {
                return 0.5f * (math.sin(-13 * M_PI_2 * ((2 * p - 1) + 1)) * math.pow(2, -10 * (2 * p - 1)) + 2);
            }
        }

        // Modeled after the overshooting cubic y = x^3-x*sin(x*pi)
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float BackEaseIn(float p)
        {

            return p * p * p - p * math.sin(p * math.PI);
        }

        // Modeled after overshooting cubic y = 1-((1-x)^3-(1-x)*sin((1-x)*pi))
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float BackEaseOut(float p)
        {
            float f = (1 - p);
            return 1 - (f * f * f - f * math.sin(f * math.PI));
        }

        // Modeled after the piecewise overshooting cubic function:
        // y = (1/2)*((2x)^3-(2x)*sin(2*x*pi))           ; [0, 0.5)
        // y = (1/2)*(1-((1-x)^3-(1-x)*sin((1-x)*pi))+1) ; [0.5, 1]
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float BackEaseInOut(float p)
        {
            if (p < 0.5f)
            {
                float f = 2 * p;
                return 0.5f * (f * f * f - f * math.sin(f * math.PI));
            }
            else
            {
                float f = (1 - (2 * p - 1));
                return 0.5f * (1 - (f * f * f - f * math.sin(f * math.PI))) + 0.5f;
            }
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float BounceEaseIn(float p)
        {
            return 1 - BounceEaseOut(1 - p);
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float BounceEaseOut(float p)
        {
            if (p < 4 / 11.0)
            {
                return (121 * p * p) / 16.0f;
            }
            else if (p < 8 / 11.0)
            {
                return (363 / 40.0f * p * p) - (99 / 10.0f * p) + 17 / 5.0f;
            }
            else if (p < 9 / 10.0f)
            {
                return (4356 / 361.0f * p * p) - (35442 / 1805.0f * p) + 16061 / 1805.0f;
            }
            else
            {
                return (54 / 5.0f * p * p) - (513 / 25.0f * p) + 268 / 25.0f;
            }
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float BounceEaseInOut(float p)
        {
            if (p < 0.5)
            {
                return 0.5f * BounceEaseIn(p * 2);
            }
            else
            {
                return 0.5f * BounceEaseOut(p * 2 - 1) + 0.5f;
            }
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float SmoothStart(in float t, in int exponent)
        {
            float product = 1;
            for (int n = 0; n < exponent; n++)
            {
                product *= t;
            }

            return product;
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float SmoothStop(in float t, in int exponent)
        {
            float product = 1;
            for (int n = 0; n < exponent; n++)
            {
                product *= (1 - t);
            }

            return 1 - product;
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float SmoothStep(in float t, in int exponent)
        {
            return math.lerp(SmoothStart(t, exponent), SmoothStop(t, exponent), t);
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public static float Crossfade(in EaseFunction easeA, in EaseFunction easeB, in float t)
        {
            return (easeA(t) * (1 - t)) + (easeB(t) * t);
        }


        public static float CalculatePercentage(in float t, in EaseType type)
        {
            switch (type)
            {
                case EaseType.Linear:
                    return LinearInterpolation(t);
                   
                case EaseType.QuadraticEaseIn:
                    return QuadraticEaseIn(t);
                   
                case EaseType.QuadraticEaseOut:
                    return QuadraticEaseOut(t);
                   
                case EaseType.QuadraticEaseInOut:
                    return QuadraticEaseInOut(t);
                   
                case EaseType.CubicEaseIn:
                    return CubicEaseIn(t);
                   
                case EaseType.CubicEaseOut:
                    return CubicEaseOut(t);
                    
                case EaseType.CubicEaseInOut:
                    return CubicEaseInOut(t);

                case EaseType.QuarticEaseIn:
                    return QuarticEaseIn(t);

                case EaseType.QuarticEaseOut:
                    return QuarticEaseOut(t);
                  
                case EaseType.QuarticEaseInOut:
                    return QuarticEaseInOut(t);
                 
                case EaseType.QuinticEaseIn:
                    return QuinticEaseIn(t);
                 
                case EaseType.QuinticEaseOut:
                    return QuinticEaseOut(t);
                   
                case EaseType.QuinticEaseInOut:
                    return QuinticEaseInOut(t);
                    
                case EaseType.SineEaseIn:
                    return SineEaseIn(t);
                   
                case EaseType.SineEaseOut:
                    return SineEaseOut(t);
                    
                case EaseType.SineEaseInOut:
                    return SineEaseInOut(t);
                   
                case EaseType.CircularEaseIn:
                    return CircularEaseIn(t);
                    
                case EaseType.CircularEaseOut:
                    return CircularEaseOut(t);
                    
                case EaseType.CircularEaseInOut:
                    return CircularEaseInOut(t);
                    
                case EaseType.ExponentialEaseIn:
                    return ExponentialEaseIn(t);
                    
                case EaseType.ExponentialEaseOut:
                    return ExponentialEaseOut(t);
                    
                case EaseType.ExponentialEaseInOut:
                    return ExponentialEaseInOut(t);
                    
                case EaseType.ElasticEaseIn:
                    return ElasticEaseIn(t);
                    
                case EaseType.ElasticEaseOut:
                    return ElasticEaseOut(t);
                    
                case EaseType.ElasticEaseInOut:
                    return ElasticEaseInOut(t);
                    
                case EaseType.BackEaseIn:
                    return BackEaseIn(t);
                    
                case EaseType.BackEaseOut:
                    return BackEaseOut(t);
                    
                case EaseType.BackEaseInOut:
                    return BackEaseInOut(t);
                    
                case EaseType.BounceEaseIn:
                    return BounceEaseIn(t);
                    
                case EaseType.BounceEaseInOut:
                    return BounceEaseInOut(t);
                    
                default:
                    break;
            }

            return t;
        }
    }
}