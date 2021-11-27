using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaking : MonoBehaviour
{
    // The number of hit points object has
    public int hitPoints = 10;

    // If we're destroyed create one of these at our current position
    public GameObject destructionPrefab;

    // Should we end the game if this object is destroyed
    public bool gameOverOnDestroyed = false;

    // Health bar attached to this object
    public HealthBar healthBar;

    public void Start() {

        // If we have a health bar, set it to max health
        if(healthBar != null) {
            healthBar.SetMaxHitPoints(hitPoints);
        }
    }

    // Called by other objects (like asteroids and shots to take damage)
    public void TakeDamage(int amount) {
        
        // Report that we got hit
        //Debug.Log(gameObject.name + " damaged!");

        
        // Deduct the amount from our hit points
        float hitPoinsBefore = hitPoints;
        hitPoints -= amount;

        // Update the health bar, if there is one
        if(healthBar != null) {
            healthBar.SetHitPoints(hitPoints);
        }

        // Are we dead?
        if(hitPoints <= 0) {

            // Log it
            //Debug.Log(gameObject.name + " destroyed!");

            // Remove ourselves from the game
            Destroy(gameObject);

            // If destroyed object is Asteroid, increase current score;
            // Multiple bullets can call this function at the same time, so score was 
            // counted twice or more. We want just the first that makes hit points go to 0
            if(gameObject.tag == "Asteroid" && hitPoinsBefore > 0) {
                GameManager.instance.currentScore++;
            }

            //Do we have a destruction prefab
            if(destructionPrefab != null) {
                // Create it at our current position and rotation
                GameObject destruction = Instantiate(destructionPrefab, transform.position, transform.rotation);
                Destroy(destruction, 5);

            }

            // If we should end the game now, call the game managers GameOver method
            if (gameOverOnDestroyed == true) {
                GameManager.instance.GameOver();
            }
        }
    }
}
