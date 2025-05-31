using Chief;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Chief
{
    public class ShooterManager : MonoBehaviour
    {
        [SerializeField] private Camera Cam;

        [SerializeField] private GameObject BulletPrefab;

        [SerializeField] private ChiefInputs Inputs;

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
            Vector3 direction = Cam.transform.forward;

            Instantiate(BulletPrefab, Cam.transform.position, Cam.transform.rotation);
        }
    }

}
