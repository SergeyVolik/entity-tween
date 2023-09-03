using System.Collections.Generic;
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

    public static class ECSCurveUtils
    {

        public static void DrawGizmos(ECSCurveBakeData curveData, Vector3 start, Vector3 end)
        {
            List<Vector3> points = new List<Vector3>();

            points.Add(start);

            for (var i = 0; i < curveData.samples; i++)
            {
                var samplePoint = (float)i / (curveData.samples - 1);
                var sampleValue = curveData.curve.Evaluate(samplePoint);
                var point = Vector3.Lerp(start, end, samplePoint);

                point.y = math.lerp(start.y, end.y, sampleValue);
                points.Add(point);

                Gizmos.DrawSphere(point, 0.1f);
            }

            if (points.Count % 2 != 0)
            {
                points.Add(end);
            }

            
            Gizmos.DrawLineStrip(points.ToArray(), false);
        }

        public static BlobAssetReference<CurveECS> Bake(this AnimationCurve curve, int samples)
        {
            return CreateCurve(new ECSCurveBakeData
            {
                curve = curve,
                samples = samples
            });
        }

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