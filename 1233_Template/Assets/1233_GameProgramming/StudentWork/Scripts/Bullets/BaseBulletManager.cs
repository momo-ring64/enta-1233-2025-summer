using Chief;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBulletManager : MonoBehaviour
{


    [Header("Physics Bullets")]
    [SerializeField] protected PhysicsBullet PhysicsBulletPrefab;
    [Header("Particle")]
    [SerializeField] protected RaycastBullet BulletParticle;


    protected void SpawnPhysicsBullet(Transform shootersTransform)
    {
        PhysicsBullet spawnedBullet = Instantiate(PhysicsBulletPrefab, transform.position, transform.rotation);
        spawnedBullet.Initialize(this);
    }

    //signals when player shoots at collidable object
    public void OnProjectileCollision(Vector3 position, Vector3 rotation)
    {
        SpawnParticle(position, rotation);
    }

    //spawns particle that lands wherever youre shooting at
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