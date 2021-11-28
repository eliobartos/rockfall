using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    // The joystick used to steer the ship
    public VirtualJoystick steering;

    // The delay between firing shots, in seconds
    public float fireRate = 0.2f;

    // The current ShipWeapons script to fire from
    private ShipWeapons currentWeapons;

    // If true, we are currently firing weapons
    private bool isFiring = false;

    // Used to reset the commands so that when a new game starts
    // joystick is centered and we are not firing
    public void Reset() {
        isFiring = false;
        steering.Reset();
    }

    // Called by ShipWeapons to update the current weapons variable
    public void SetWeapons(ShipWeapons weapons) {
        this.currentWeapons = weapons;
    }

    // Likewise, called to reset the currentWeapons variable
    public void RemoveWeapons(ShipWeapons weapons) {
        
        if (this.currentWeapons == weapons) {
            this.currentWeapons = null;
        }
    }

    public void Start() {
        StartCoroutine(FireWeapons());
        //Debug.Log("Staring Fire Coroutine");
    }

    public void StartFiring() {
        // Kicks of the routine that starts firing shots
        isFiring = true;
    }

    IEnumerator FireWeapons() {
        
        while(true) {

            // Loop for as long as is Firing is true
            while(isFiring) {

                // If we have a weapons script, tell it to fire a shot
                if(this.currentWeapons != null) {
                    currentWeapons.Fire();
                }

                // Wait for fire rate seconds before firing next one
                yield return new WaitForSeconds(fireRate);
            }
            yield return new WaitForEndOfFrame();
        }
        
    }

    // Called when user stops touching the fire button
    public void StopFiring() {
        
        // Setting this to false will end the loop in FireWeapons
        isFiring = false;
    }
}
