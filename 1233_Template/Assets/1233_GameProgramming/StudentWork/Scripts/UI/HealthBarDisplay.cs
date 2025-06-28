using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script is used to lower the length of the red hp bar in the ui
public class HealthBarDisplay : MonoBehaviour
{
    [SerializeField] private Image HealthBarFill;

    public void UpdateHp(float hpPercent)
    {
        HealthBarFill.fillAmount = Mathf.Clamp(hpPercent, 0, 1f);
    }
}
