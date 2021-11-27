using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTarget : MonoBehaviour
{
    // The sprite to use for the target reticle
    public Sprite targetImage;
    public Color indicatorColor = Color.white;
    public float indicatorScale = 1.0f;

    void Start() {
        // Register a new indicator that tracks this object, using a yellow color
        IndicatorManager.instance.AddIndicator(gameObject, indicatorColor, targetImage, indicatorScale);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
