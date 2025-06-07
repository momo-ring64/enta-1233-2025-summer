using Chief;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chief
{
    public class PhysicsBullet : BaseBulletManager
    {

        [SerializeField] private float ProjectileSpeed;

        [SerializeField] private float ProjectileDamage;

        [SerializeField] private Rigidbody Rb;

        private BaseBulletManager shooterManager;

        public void Initialize(BaseBulletManager manager)
        {
            shooterManager = manager;
        }


    // Start is called before the first frame update
    void Start()
        {
            Rb.AddForce(transform.forward * ProjectileSpeed, ForceMode.Impulse);
        }

        void OnCollisionEnter(Collision collision)
        {
            ContactPoint contact = collision.GetContact(0);
            shooterManager.OnProjectileCollision(contact.point, contact.normal);
            Destroy(gameObject);
        }

    }
}
