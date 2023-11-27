using System.Collections;
using System.Collections.Generic;
using Timespawn.EntityTween.Tweens;
using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
internal class TweensSettingsAuthoring : MonoBehaviour
{
    public bool EnableTween = true;
    void OnEnable() { }

    class Baker : Baker<TweensSettingsAuthoring>
    {
        public override void Bake(TweensSettingsAuthoring authoring)
        {
            if (!authoring.enabled)
                return;

            var entity = GetEntity(TransformUsageFlags.None);

            if(authoring.EnableTween)
                AddComponent(entity, new EnableTweensT { });
        }
    }
}


