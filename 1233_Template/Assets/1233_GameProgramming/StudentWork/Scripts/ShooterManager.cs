using Chief;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace Chief
{
    public class ShooterManager : MonoBehaviour
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
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, Mathf.Infinity, RaycastMask))
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
            Instantiate(BulletParticle, position, Quaternion.Euler(rotation));
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
