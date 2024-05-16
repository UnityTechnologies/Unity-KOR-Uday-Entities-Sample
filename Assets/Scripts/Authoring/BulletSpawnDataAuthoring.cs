using Unity.Entities;
using UnityEngine;

class BulletSpawnDataAuthoring : MonoBehaviour
{
    public GameObject BulletPrefab;
    public int BulletsPerSpawn = 100;
    public double TimeBetweenSpawns = 0.2f;
    public float BulletSpeed = 8f;
}

class BulletSpawnDataAuthoringBaker : Baker<BulletSpawnDataAuthoring>
{
    public override void Bake(BulletSpawnDataAuthoring authoring)
    {
        var entity = GetEntity(authoring.gameObject, TransformUsageFlags.None);

        var bulletSpawnData = new BulletSpawnData();
        bulletSpawnData.BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic);
        bulletSpawnData.BulletsPerSpawn = authoring.BulletsPerSpawn;
        bulletSpawnData.TimeBetweenSpawns = authoring.TimeBetweenSpawns;
        bulletSpawnData.BulletSpeed = authoring.BulletSpeed;
        bulletSpawnData.NextSpawnTime = 0;
        
        AddComponent(entity, bulletSpawnData);
    }
}