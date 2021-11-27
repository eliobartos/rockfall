using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorManager : Singleton<IndicatorManager>
{
    // The object that all indicators will be children of
    public RectTransform labelContainer;

    // The prefab we'll instantiate for each indicator
    public Indicator indicatorPrefab;

    // This method will be called by other objects
    public Indicator AddIndicator(GameObject target, Color color, Sprite sprite = null, float scale = 1.0f) {
        
        // Create the label object
        var newIndicator = Instantiate(indicatorPrefab);

        // Make it track the target
        newIndicator.target = target.transform;

        // Update its color
        newIndicator.color = color;

        // If we received a sprite, set the indicator's sprite to that
        if (sprite != null) {
            Image image = newIndicator.GetComponent<Image>();
            image.sprite = sprite;
            image.transform.localScale = new Vector3(scale, scale, scale);
        }

        // Add it to the container
        newIndicator.transform.SetParent(labelContainer, false);

        return newIndicator;
    }
}
