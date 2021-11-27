using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapons : MonoBehaviour
{

    // The prefab to use for each shot
    public GameObject shotPrefab;

    // The list of places where a shot can emerge from
    public Transform[] firePoints;

    // The index into fire points that the next shot will fire from
    private int firePointIndex;

    public void Awake() {
        // When this object starts up, tell the input manager to use it as the current weapon object
        InputManager.instance.SetWeapons(this);
    }

    // Called when the object is removed
    public void OnDestroy() {
        // Don't do this if we're not playing
        if(Application.isPlaying == true) {
            InputManager.instance.RemoveWeapons(this);
        }
    }

    // Called by InputManager
    public void Fire() {
        
        // If we have no points to fire from, return
        if (firePoints.Length == 0) return;

        // Work out which point to fire from
        var firePointToUse = firePoints[firePointIndex];

        // Create the new shot, at the fire point's position and with its rotation
        Instantiate(shotPrefab, firePointToUse.position, firePointToUse.rotation);

        // Move to the next fire point
        firePointIndex++;

        // Don't drop out of the array
        if(firePointIndex >= firePoints.Length) {
            firePointIndex = 0;
        }

        // If the fire point has an audio source component, play its sound effect
        var audio = firePointToUse.GetComponent<AudioSource>();
        if (audio) {
            audio.Play();
        }

    }
}
