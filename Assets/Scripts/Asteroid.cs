using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // The speed at which the asteroid moves
    public float speed = 10.0f;

    // Indicator color
    public Color indicatorColor = Color.white;

    void Start()
    {
        // Set the velocity of the rigidbody
        GetComponent<Rigidbody>().velocity = transform.forward * speed;

        // Create a red indicator for this asteroid
        var indicator = IndicatorManager.instance.AddIndicator(gameObject, indicatorColor);

        // Track the distance from this object to the current space station
        indicator.showDistanceTo = GameManager.instance.currentSpaceStation.transform;

        // Set up proper trail size
        TrailRenderer trail = gameObject.GetComponentInChildren<TrailRenderer>();
        trail.startWidth = transform.localScale.x;
        
        
    }

}
