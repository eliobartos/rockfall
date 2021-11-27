using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{

    // Indicator color
    public Color indicatorColor = Color.white;
    
    // Variables for playing alert sound
    public int alertDistance = 50;
    public AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        // Add indicator to yourself
        IndicatorManager.instance.AddIndicator(
            gameObject,
            indicatorColor
        );

        // Cache audio source
        audioSource = GetComponent<AudioSource>();
    }

    // Play a sound if any asteroid is closer than  meters
    // and we have not played an alert for him
    void Update() {
        
        foreach (var asteroid in FindObjectsOfType<Asteroid>()) {
            if( asteroid.playedAlertSound == false && (float)Vector3.Magnitude(asteroid.transform.position - transform.position) <= alertDistance) {
                asteroid.playedAlertSound = true;
                audioSource.Play();
            }
        }
    }
}