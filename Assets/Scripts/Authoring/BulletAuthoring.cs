using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class BulletAuthoring : MonoBehaviour
{
    
}

class BulletAuthoringBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
        
        AddComponent(entity, new Bullet());
        AddComponent(entity, new Movement { Velocity = new float3(1, 0, 0)});
    }
}