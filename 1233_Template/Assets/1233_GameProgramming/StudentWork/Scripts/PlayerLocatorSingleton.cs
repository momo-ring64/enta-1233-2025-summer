using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// monobehaviour only exosys while the game is in play mode
/// </summary>

public class PlayerLocatorSingleton : MonoBehaviour
{
    /// <summary>
    /// static field that exists for the entire projects duration
    /// IMPORTANT! can be null if the game is not playing
    /// </summary>
   public static PlayerLocatorSingleton Instance;

    private void Awake()
    {
        //instance will be null only of no playerlocator game object exists
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("there is more than one playerlocatorsingleton");
            Destroy(gameObject);
        }

    }
}
