using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
            if (_camera == null) return; // still null, safely exit
        }

        transform.forward = _camera.transform.forward;
    }
}
