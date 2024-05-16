using Unity.Entities;
using UnityEngine;

class BulletSpawnDataAuthoring : MonoBehaviour
{
    public GameObject BulletPrefab;
    public int BulletsPerSpawn;
    public double TimeBetweenSpawns;
    public double BulletSpeed;
}

class BulletSpawnDataAuthoringBaker : Baker<BulletSpawnDataAuthoring>
{
    public override void Bake(BulletSpawnDataAuthoring authoring)
    {
        var entity = GetEntity(authoring.gameObject, TransformUsageFlags.None);

        var bulletSpawnData = new BulletSpawnData();
        bulletSpawnData.BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.None);
        bulletSpawnData.BulletsPerSpawn = authoring.BulletsPerSpawn;
        bulletSpawnData.TimeBetweenSpawns = authoring.TimeBetweenSpawns;
        bulletSpawnData.BulletSpeed = authoring.BulletSpeed;
        bulletSpawnData.LastSpawnTime = 0;
        
        AddComponent(entity, bulletSpawnData);
    }
}
