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

    public struct CurvesXYZ
    {
        public BlobAssetReference<CurveECS> curveX;
        public BlobAssetReference<CurveECS> curveY;
        public BlobAssetReference<CurveECS> curveZ;

        public bool IsValid()
        {
            if (!curveX.IsCreated)
                return false;
            if (!curveY.IsCreated)
                return false;

            if (!curveZ.IsCreated)
                return false;

            return true;
        }

    }

    [System.Serializable]
    public class ECSCurveBakeData
    {
        public AnimationCurve curve;
        public int samples;
    }

    [System.Serializable]
    public class ECSCurveXYZBakeData
    {
        public AnimationCurve curveX;
        public AnimationCurve curveY;
        public AnimationCurve curveZ;

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

        public static void DrawGizmos(ECSCurveXYZBakeData curveData, Vector3 start, Vector3 end)
        {
            List<Vector3> points = new List<Vector3>();

            points.Add(start);

            for (var i = 0; i < curveData.samples; i++)
            {
                var samplePoint = (float)i / (curveData.samples - 1);
                var sampleValueX = curveData.curveX.Evaluate(samplePoint);
                var sampleValueY = curveData.curveY.Evaluate(samplePoint);
                var sampleValueZ = curveData.curveZ.Evaluate(samplePoint);
                var point = new Vector3();

                point.y = math.lerp(start.y, end.y, sampleValueY);
                point.x = math.lerp(start.x, end.x, sampleValueX);
                point.z = math.lerp(start.z, end.z, sampleValueZ);

                points.Add(point);

                Gizmos.DrawSphere(point, 0.1f);
            }

            if (points.Count % 2 != 0)
            {
                points.Add(end);
            }


            Gizmos.DrawLineStrip(points.ToArray(), false);
        }

        public static CurvesXYZ CreateXYZCurves(ECSCurveXYZBakeData bakeData)
        {
            var samples = bakeData.samples;
            var curvesXYZ = new CurvesXYZ();
            curvesXYZ.curveX = CreateCurve(new ECSCurveBakeData
            {
                curve = bakeData.curveX,
                samples = samples
            });

            curvesXYZ.curveY = CreateCurve(new ECSCurveBakeData
            {
                curve = bakeData.curveY,
                samples = samples
            });

            curvesXYZ.curveZ = CreateCurve(new ECSCurveBakeData
            {
                curve = bakeData.curveZ,
                samples = samples
            });

            return curvesXYZ;
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