using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

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

    [System.Serializable]
    public class ECSCurveBakeData
    {
        public AnimationCurve curve;
        public int samples;
    }

    public static class ECSCuverUtils
    {
        public static BlobAssetReference<CurveECS> CreateCurve(ECSCurveBakeData data)
        {
            var numberOfSamples = data.samples;
            var unityCurve = data.curve;

            using var blobBuilder = new BlobBuilder(Allocator.Temp);
            ref var sampledCurve = ref blobBuilder.ConstructRoot<CurveECS>();
            var sampledCurveArray = blobBuilder.Allocate(ref sampledCurve.points, numberOfSamples);
            sampledCurve.numberOfSamples = numberOfSamples;

            for (var i = 0; i < numberOfSamples; i++)
            {
                var samplePoint = (float)i / (numberOfSamples - 1);
                var sampleValue = unityCurve.Evaluate(samplePoint);
                sampledCurveArray[i] = sampleValue;
            }

            var blobAssetReference = blobBuilder.CreateBlobAssetReference<CurveECS>(Allocator.Persistent);

            return blobAssetReference;
        }
    }
}