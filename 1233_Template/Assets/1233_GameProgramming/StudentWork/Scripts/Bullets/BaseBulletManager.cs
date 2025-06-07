using Chief;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBulletManager : MonoBehaviour
{


    [Header("Physics Bullets")]
    [SerializeField] private PhysicsBullet PhysicsBulletPrefab;
    [Header("Particle")]
    [SerializeField] private RaycastBullet BulletParticle;


    protected void SpawnPhysicsBullet(Transform shootersTransform)
    {
        PhysicsBullet spawnedBullet = Instantiate(PhysicsBulletPrefab, transform.position, transform.rotation);
        spawnedBullet.Initialize(this);
    }

    public void OnProjectileCollision(Vector3 position, Vector3 rotation)
    {
        SpawnParticle(position, rotation);
    }

    private void SpawnParticle(Vector3 position, Vector3 rotation)
    {
        if (BulletParticle != null)
        {
            Instantiate(BulletParticle, position, Quaternion.Euler(rotation));
        }
        else
        {
            Debug.LogWarning("BulletParticle prefab is not assigned or has been destroyed.");
        }
    }
}