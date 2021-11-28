using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // The prefab to use for the ship, the place it starts from and the current ship object
    public GameObject shipPrefab;
    public Transform shipStartPosition;
    public GameObject currentShip {get; private set;}

    // The prefab to use for the space station, the place it starts from and the current ship object
    public GameObject spaceStationPrefab;
    public Transform spaceStationStartPosition;
    public GameObject currentSpaceStation {get; private set;}

    // The follow script on the main camera
    public SmoothFollow cameraFollow;

    // The game's boundary
    public Boundary boundary;

    // UIs
    public GameObject inGameUI;
    public GameObject pausedUI;
    public GameObject gameOverUI;
    public GameObject mainMenuUI;
    public GameObject creditsUI;

    // The warning UI that appears when we approach the boundary
    public GameObject warningUI; 

    // Is the game currently playing?
    public bool gameIsPlaying {get; private set;}

    // The game's Asteroid Spawner
    public AsteroidSpawner asteroidSpawner;

    // Keeps track of whether the game is paused or not
    public bool paused;

    // Keep high score and current score
    [HideInInspector] public int highScore = 0;
    [HideInInspector] public int currentScore = 0;
    public Text scoreTextInGameUI;
    public Text scoreTextGameOverUI;
    public Text highScoreTextMainMenuUI;
    public Text highScoreTextGameOverUI;

    // Show the main menu when the game starts
    void Start() {
        LoadGameData();
        ShowMainMenu();
    }


    // Shows a UI container, hides all others

    public void ShowUI(GameObject newUI) {
        // create a list of UI containers
        GameObject[] allUI = {inGameUI, pausedUI, gameOverUI, mainMenuUI, creditsUI};

        // Hide the mall
        foreach (GameObject UIToHide in allUI) {
            UIToHide.SetActive(false);
        }

        // And then show the provided UI container
        newUI.SetActive(true);
    }

    public void ShowMainMenu() {
        ShowUI(mainMenuUI);

        // Set high score
        highScoreTextMainMenuUI.text = "High Score: " + highScore; 
        highScoreTextGameOverUI.text = "High Score: " + highScore; 

        // We aren't playing yet when the game starts
        gameIsPlaying = false;

        // Don't spawn asteroids either
        asteroidSpawner.spawnAsteroids = false;
    }

    // Called by the New Game button being tapped
    public void StartGame() {
        
        // Set the current_score to 0
        currentScore = 0;

        // Show the in game UI
        ShowUI(inGameUI);

        // We are now playing
        gameIsPlaying = true;

        // If we have a ship destroy it
        if (currentShip != null) Destroy(currentShip);
        
        // Same for the space station
        if (currentSpaceStation != null) Destroy(currentSpaceStation);

        // Create a new ship and place it at a start position
        currentShip = Instantiate(shipPrefab);
        currentShip.transform.position = shipStartPosition.transform.position;
        currentShip.transform.rotation = shipStartPosition.transform.rotation;

        // Likewise for the space station
        currentSpaceStation = Instantiate(spaceStationPrefab);
        currentSpaceStation.transform.position = spaceStationStartPosition.transform.position;
        currentSpaceStation.transform.rotation = spaceStationStartPosition.transform.rotation;
        currentSpaceStation.GetComponent<DamageTaking>().healthBar = GameObject.Find("Health Bar").GetComponent<HealthBar>();

        // Make the follow script track the new ship
        cameraFollow.target = currentShip.transform.Find("Ship Camera Follow Target").transform;

        // Start spawning asteroids
        asteroidSpawner.spawnAsteroids = true;

        // And aim the spawner at the new space station
        asteroidSpawner.target = currentSpaceStation.transform;

        // Restart asterodi spawner timer
        asteroidSpawner.gameObject.GetComponent<Timer>().BeginTimer();
    }

    // Called by the objects that end the game when they are destroyed
    public void GameOver() {

        // Show the game over UI
        ShowUI(gameOverUI);

        // We're no longer playing
        gameIsPlaying = false;

        // Destroy the ship and station
         if (currentShip != null) Destroy(currentShip);
         if (currentSpaceStation != null) Destroy(currentSpaceStation);

         // Stop spawning asteroids
         asteroidSpawner.spawnAsteroids = false;

         // Stop the timer
         asteroidSpawner.gameObject.GetComponent<Timer>().EndTimer();
         
         // Remove all lingering asteroids
         asteroidSpawner.DestroyAllAsteroids();

         // Stop showing warning UI, if it was visible
         warningUI.SetActive(false);

         // Set current score text
         scoreTextGameOverUI.text = "Score: " + currentScore;

         // Set current score as high score if that is the case
         if(currentScore > highScore) {
             highScore = currentScore;
             highScoreTextMainMenuUI.text = "High Score: " + highScore; 
             highScoreTextGameOverUI.text = "High Score: " + highScore; 
             SaveSystem.SaveGame(highScore);
         }

         // Reset the controls so that when a new game is starting
         // joystick is centered and we are not firing
         InputManager.instance.Reset();
         
    }

    // Called when the pause or resume buttons are tapped
    public void SetPaused(bool paused) {
        // switch between in game and paused UI
        inGameUI.SetActive(!paused);
        pausedUI.SetActive(paused);

        if(paused) {
            // Stop time
            Time.timeScale = 0.0f;
        } else {
            // Resume time
            Time.timeScale = 1.0f;
        }
    }

    public void Update() {
        
        
        // If we don't have a ship, bail out
        if (currentShip == null) 
            return;

        // Set proper score during the game text
        scoreTextInGameUI.text = "Score: " + currentScore;

        // If the ship is outside the Boundary's Destroy radius, game over.
        // If it's within the Destroy radius, but outside of warning radius, show
        // warning UI. If it is within both, don't show anything

        float distance = (currentShip.transform.position - boundary.transform.position).magnitude;

        if(distance > boundary.destroyRadius) {
            // The ship has gone beyond the destroy radius, so it's game over
            GameOver();
        } else if(distance > boundary.warningRadius) {
            // The ship has gone beyond the warning radius, so show the warning UI
            warningUI.SetActive(true);
        } else {
            // It's within the warning threshold, don't show the warning UI
            warningUI.SetActive(false);
        }

        
    }

    public void LoadGameData() {

        // If we don't have a save file, set high score to 0
        // else, load file and get true high score
        SaveGameData data = SaveSystem.LoadGame();
        if(data == null) {
            highScore = 0;
        } else {
            highScore = data.highScore;
        }
        
    }
}
