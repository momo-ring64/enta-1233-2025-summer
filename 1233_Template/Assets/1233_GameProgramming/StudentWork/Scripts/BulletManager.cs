using Chief;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace Chief
{
    public class BulletManager : BaseBulletManager
    {
        [Header("External Scripts")]
        [SerializeField] private Camera Cam;
        [SerializeField] private ChiefInputs Inputs;

        [Header("Physics")]
        [SerializeField] private PhysicsBullet PhysicsBulletPrefab;

        [Header("Raycast")]
        [SerializeField] private RaycastBullet BulletParticle;
        [SerializeField] private LayerMask RaycastMask;
        [SerializeField] private ShootType ShootingCalculation;



        public enum ShootType
        {
            Raycast = 0,
            Physics = 1,
        }

        private void Update()
        {
            if (Inputs.Aim && Inputs.Fire)
            {

                OnFirePressed();
            }
            Inputs.FireInput(false);
        }

        private void OnFirePressed()
        {
            switch (ShootingCalculation)
            {
                case ShootType.Raycast:
                    DoRaycastShot();
                    break;
                case ShootType.Physics:
                    SpawnPhysicsBullet();
                    break;
                default:
                    Debug.LogError("Fire!");
                    break;
            }
         
        }

        private void SpawnPhysicsBullet()
        {
            // does not call collision until physics system collides


            PhysicsBullet spawnedBullet = Instantiate(PhysicsBulletPrefab, Cam.transform.position, Cam.transform.rotation);
            spawnedBullet.Initialize(this);
        }

        private void DoRaycastShot()
        {

            Vector3 start = Cam.transform.position;
            Vector3 direction = Cam.transform.forward;

            if (Physics.Raycast(start, direction, out RaycastHit hit, Mathf.Infinity, RaycastMask))
            {
                Vector3 end = hit.point;

                // Spawn particle ray line
                RaycastBullet bulletVisual = Instantiate(BulletParticle, Vector3.zero, Quaternion.identity);
                bulletVisual.Initialize(start, end);

                OnProjectileCollision(hit.point, hit.normal);
            }
            else
            {
                // No hit, still draw the ray forward
                Vector3 end = start + direction * 100f;
                RaycastBullet bulletVisual = Instantiate(BulletParticle, Vector3.zero, Quaternion.identity);
                bulletVisual.Initialize(start, end);
            }
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, Mathf.Infinity, RaycastMask))
            {
                OnProjectileCollision(hit.point, hit.normal);
            }
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



        //shows the raycast line in scene
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (Inputs.Aim)
                Gizmos.DrawLine(Cam.transform.position, Cam.transform.position + Cam.transform.forward);
        }

        private void CleanupParticle()
        {
            Gizmos.DrawLine(Cam.transform.position, Cam.transform.position + Cam.transform.forward * 100);
        }
    }
}
