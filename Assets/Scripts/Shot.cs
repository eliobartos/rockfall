using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves forward at a certain speed, and dies after a certain time
public class Shot : MonoBehaviour
{

    // The speed at which the shot will move forward
    public float speed = 100.0f;

    // Remove this object after this many seconds
    public float life = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Destroy after 'live' seconds
        Destroy(gameObject, life);
    }

    // Update is called once per frame
    void Update()
    {
        // Move forward at a constant speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
