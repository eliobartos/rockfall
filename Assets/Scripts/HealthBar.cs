using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHitPoints(int hitPoints) {
        slider.maxValue = hitPoints;
        slider.value = hitPoints;
    }

    public void SetHitPoints(int hitPoints) {
        slider.value = hitPoints;
    }
}
