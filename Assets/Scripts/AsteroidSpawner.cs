using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // The radius of the spawn area
    public float radius = 250.0f;

    // The asteroids to spawn
    public Rigidbody asteroidPrefab;

    // Wait spawnRate + variance seconds between each asteroid
    public AnimationCurve spawnCurve;
    public float variance = 0.5f;

    // The object to aim the asteroids at
    public Transform target;

    // If false, disable spawning
    public bool spawnAsteroids = false;

    public Timer timer;

    void Start() {
        
        // Get timer component
        timer = GetComponent<Timer>();

        // Start the coroutine that creates asteroids
        StartCoroutine(CreateAsteroids());
    }

    IEnumerator CreateAsteroids() {

        while(true) {
            
            // Work out when the next asteroid should appear
            float nextSpawnTime = spawnCurve.Evaluate(timer.GetElapsedTime()) + Random.Range(-variance, variance);
            //Debug.Log("At: " + timer.GetElapsedTime() + " the value is " + spawnCurve.Evaluate(timer.GetElapsedTime()));
            
            if(nextSpawnTime <= 0) {
                nextSpawnTime = 0;
            }

            // Wait that much time
            yield return new WaitForSeconds(nextSpawnTime);

            // Additionally wait untily physics is about to update
            yield return new WaitForFixedUpdate();

            // Create the asteroid
            CreateNewAsteroid();
        }
    }

    void CreateNewAsteroid() {
    
        Debug.Log("Time passed: " + timer.GetElapsedTime());

        // If we're not currently spawning asteroids, bail out
        if(spawnAsteroids == false) 
            return;

        // Randomly select a point on the surface of the sphere
        var asteroidPosition = Random.onUnitSphere * radius;

        // Scale this by the object's scale
        asteroidPosition.Scale(transform.lossyScale);

        // And offset it by the asteroid spawner location
        asteroidPosition += transform.position;

        // Create the new asteroid
        var newAsteroid = Instantiate(asteroidPrefab);

        // Place it at the spot we just calculated
        newAsteroid.transform.position = asteroidPosition;

        // Aim it at the target
        newAsteroid.transform.LookAt(target);

        // Create logic of new asteroids
        // Asteroids have random size and speed.
        // The bigger the size, the more hitpoints they have

        // Random Size
        float randomSize = Random.Range(5, 15);
        Debug.Log("Creating New Asteroid!! Size: " + randomSize);
        newAsteroid.transform.localScale = new Vector3(randomSize, randomSize, randomSize);

        // Set proper health and damage based on size
        DamageTaking damageTaking = newAsteroid.GetComponent<DamageTaking>();
        DamageOnCollide damageOnCollide = newAsteroid.GetComponent<DamageOnCollide>();
        if(damageTaking != null && damageOnCollide != null) {
            
            // Set Asteroid hit points and damage based on its size
            if(randomSize <= 8.0f) {
                damageTaking.hitPoints = 1;
                damageOnCollide.damage = 1;
            } else if(randomSize < 12.0f) {
                damageTaking.hitPoints = 2;
                damageOnCollide.damage = 2;
            } else {
                damageTaking.hitPoints = 3;
                damageOnCollide.damage = 4;
            }

        } else {
            Debug.LogError("Couldn't find Damage Taking component of a new asterodi!");
        }
        
        // Set random speed 
        float randomSpeed = Random.Range(5, 12);
        newAsteroid.GetComponent<Asteroid>().speed = randomSpeed;

    }

    // Called by the editor while the spawner object is selected
    void OnDrawGizmosSelected() {
        
        // We want to draw yellow stuff
        Gizmos.color = Color.yellow;

        // Tell the Gizmos to use our current position and scale
        Gizmos.matrix = transform.localToWorldMatrix;

        // Draw a sphere representing the spawn area
        Gizmos.DrawWireSphere(Vector3.zero, radius);
    }

    public void DestroyAllAsteroids() {
        // Remove all asteroids from the game
        foreach (var asteroid in FindObjectsOfType<Asteroid>()) {
            Destroy (asteroid.gameObject);
        }
    }
}
