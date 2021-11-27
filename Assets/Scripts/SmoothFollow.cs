using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    
    // The target we are following
    public Transform target;


    // How much we slow down changes in rotation and height
    public float positionDamping;
    public float rotationDamping;

    // Update is called once per fframe
    void Update() {

        // Don't do anything if we don't have a target
        if(!target) return;

        // Calculate the current position
        var wantedPosition = target.position;
        var wantedRotation = target.rotation;

        // Note where we're currently positioned and looking
        var currentPosition = transform.position;
        var currentRotation = transform.rotation;

        // move smootly to wanted position
        //transform.position = Vector3.SmoothDamp(currentPosition, wantedPosition, ref velocity, positionDamping * Time.deltaTime);
        transform.position = Vector3.Lerp(currentPosition, wantedPosition, positionDamping * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(currentRotation, wantedRotation, rotationDamping * Time.deltaTime);
        


    }
}
