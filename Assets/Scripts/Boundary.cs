using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    // Show the warning UI when the player is far from the center
    public float warningRadius = 400.0f;

    // End the game when the player is this far from the center
    public float destroyRadius = 450.0f;

    public void OnDrawGizmosSelected() {
        // Show a yellow sphere with the warning radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, warningRadius);

        // And show a red sphere with the destroy radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destroyRadius);
    }
}
