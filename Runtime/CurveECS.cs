using Unity.Entities;
using Unity.Mathematics;

namespace Timespawn.EntityTween.Tweens
{
    public struct CurveECS
    {
        public BlobArray<float> points;
        public int numberOfSamples;

        public float GetValueAtTime(float time)
        {
            var approxSampleIndex = (numberOfSamples - 1) * time;
            var sampleIndexBelow = (int)math.floor(approxSampleIndex);
            if (sampleIndexBelow >= numberOfSamples - 1)
                return points[numberOfSamples - 1];

            var indexRemainder = approxSampleIndex - sampleIndexBelow;
            return math.lerp(points[sampleIndexBelow], points[sampleIndexBelow + 1], indexRemainder);
        }
    } 
}

