using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSteering : MonoBehaviour
{
    // The rate at which the ship turns
    public float turnRate = 6.0f;

    // The strenght with which the ship levels out
    public float levelDamping = 1.0f;

    void LateUpdate() {

        // Create a new rotation by multyplying the joystick's direction 
        // by turnRate and clamping that to 90% of a half a circle

        // First, get the user's input
        var steeringInput = InputManager.instance.steering.delta;

        // Create a rotation amount a sa vector
        var rotation = new Vector2();

        rotation.y = steeringInput.x;
        rotation.x = -steeringInput.y;

        // Multiply the rotation by turt rate to get the amount we want to steer by
        rotation = rotation * turnRate;

        // Turn this into radians by multiplying by 90% of a half-circle
        //rotation.x = Mathf.Clamp(rotation.x, -Mathf.PI * 0.9f, Mathf.PI * 0.9f);

        // Turn those radians into a rotation quaternion
        var newOrientation = Quaternion.Euler(rotation);

        // Combite this turn with our current rotation (rotate the object for newOrientation)
        transform.rotation *= newOrientation;

        // Next, try to minimize roll!
        // Start by working out what our orientation would be
        // if we weren't rolled around the z axis at all
        var levelAngles = transform.eulerAngles;
        levelAngles.z = 0.0f;
        //levelAngles.x = 0.0f;
        var levelOrientation = Quaternion.Euler(levelAngles);

        // Combine our current orientation with a small amount
        // of this "zero-roll" orientation. When this happens over multiple
        // frames, the object will slowly level out to zero roll
        if(steeringInput.x == 0 && steeringInput.y == 0) {
            transform.rotation = Quaternion.Slerp(
                transform.rotation, levelOrientation, levelDamping * Time.deltaTime);
        }
    }
}
