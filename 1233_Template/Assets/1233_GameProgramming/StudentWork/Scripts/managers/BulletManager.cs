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
        [SerializeField] private Transform ShootingPoint; // The point where the bullet spawns




        [Header("Raycast")]
        [SerializeField] private LayerMask RaycastMask;
        [SerializeField] private ShootType ShootingCalculation;

        [Header("Sound")]
        [SerializeField] private AudioSource ShootingSource;
        [SerializeField] private AudioClip ShootingSound;

        [SerializeField] private ParticleSystem MuzzleFlash;




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
                    ShootingSource.PlayOneShot(ShootingSound);
                    break;
                default:
                    Debug.LogError("Fire!");
                    break;
            }
         
        }

        private void SpawnPhysicsBullet()
        {

            if (MuzzleFlash != null)
            {
                MuzzleFlash.Play();
            }

            // raycast from the center of the screen
            Ray ray = Cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, RaycastMask))
            {
                targetPoint = hit.point;
            }
            else
            {
                // if nothing hit just shoot far forward
                targetPoint = ray.GetPoint(1000f);
            }

            // calculate the direction to shoot
            Vector3 shootDirection = (targetPoint - ShootingPoint.position).normalized;

            // spawn and launch the bullet
            PhysicsBullet spawnedBullet = Instantiate(
                PhysicsBulletPrefab,
                ShootingPoint.position,
                Quaternion.LookRotation(shootDirection)
            );

            spawnedBullet.Initialize(this);
            spawnedBullet.GetComponent<Rigidbody>().velocity = shootDirection * 50f; // set your desired bullet speed here
        }


        private void DoRaycastShot()
        {

            Vector3 start = Cam.transform.position;
            Vector3 direction = Cam.transform.forward;

            if (Physics.Raycast(start, direction, out RaycastHit hit, Mathf.Infinity, RaycastMask))
            {
                Vector3 end = hit.point;

                // spawn particle ray line
                RaycastBullet bulletVisual = Instantiate(BulletParticle, Vector3.zero, Quaternion.identity);
                bulletVisual.Initialize(start, end);

                OnProjectileCollision(hit.point, hit.normal);
            }
            else
            {
                // no hit, still draw the ray forward
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
            if (Inputs != null && Inputs.Aim)
                Gizmos.DrawLine(Cam.transform.position, Cam.transform.position + Cam.transform.forward);
        }

        private void CleanupParticle()
        {
            Gizmos.DrawLine(Cam.transform.position, Cam.transform.position + Cam.transform.forward * 100);
        }
    }
}
