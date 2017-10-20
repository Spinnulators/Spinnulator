using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    GameObject[] apples;
    public GameObject player;
    public PlayerController playerController;
    public Collectable collectable;
    public TimeManager timeManager;
    public GameObject introPanel;
    public Text introText;
    public GameObject sensorView;
    public GuiManager guiManager;
    public KinectInterface kinectInterface;
	public ControlInterface controlInterface;

    private bool hasStarted;
    private bool hasEnded;
    private bool hasPaused;
    private Vector3 startPosition;
    private Quaternion startRotation;

	private float startTime;
	private float resetTime;
    private float endTime;

    void Start() {
        Cursor.visible = false;
        apples = GameObject.FindGameObjectsWithTag("Apple");
        introText.text = Strings.intro;

        startPosition = player.transform.position;
        startRotation = player.transform.rotation;

		guiManager.resetGame();
    }

	// Update is called once per frame
	void FixedUpdate () {

		if (controlInterface.isStartKeyPressed () && !gameHasStarted() && !gameHasEnded()) {
			int timeSinceRestartClick = (int)(Time.time - resetTime);
			int restartCountdownLimit = 1;
			if (timeSinceRestartClick > restartCountdownLimit) {
				startTime = Time.time;
				startGame ();
			}
		}
		else if (controlInterface.isStartKeyPressed () && gameHasStarted () && !gameHasEnded ()) {
			int timeSinceStartClick = (int) (Time.time - startTime);
			int startCountdownLimit = 1;
			if (timeSinceStartClick > startCountdownLimit) {
				resetTime=Time.time;
				resetGame();
			}
		}
		if (controlInterface.isResetKeyPressed()) {
            resetGame();
		}

        if (timeManager.isEnabled() && timeManager.hasEnded() && !gameHasEnded()) {
            endGame();
        }

        if (gameHasEnded()) {
			endGameCountdown();
        }
    }

    void resetGame() {
        player.transform.position = new Vector3(-21.28f, 20.32f, 13.42f);
        player.transform.rotation = Quaternion.identity;
        playerController.momentum = 0;
        collectable.countSum = 0;

        timeManager.end();

        guiManager.resetGame();

        hasStarted = false;
        hasEnded = false;

        collectable.counterText.text = collectable.countSum.ToString();

        foreach (GameObject apple in apples) {
            if (!apple.activeSelf) {
                apple.SetActive(true);
            }
        }

		Debug.Log ("Game reset");
    }

	/**
	 * If the kinect is tracking the player, have a small countdown
	 * and automatically start the game afterwards.
	 * 
	 * NOT USED ATM
	 */
    private void playerOnBikeStartCountdown() {
        int countdownLimit = 5;
        int timeDifference = (int) (Time.time - kinectInterface.getTimePlayerStartedOnBike());

        if (kinectInterface.isTracking()) {
            if (timeDifference < countdownLimit) {
                guiManager.showStartCountdown(countdownLimit - timeDifference);
            }
            else {
                startGame();
                guiManager.hideStartCountdown();
            }
        }
    }

	/**
	 * Reset the game after a countdown 
	 */
	private void endGameCountdown() {
		int timeSinceEnd = (int) (Time.time - endTime);
		int countdownLimit = 5;
		
		if (timeSinceEnd > countdownLimit) {
			resetGame();
		}
	}

    public void startGame() {
        hasStarted = true;
		timeManager.start();
        guiManager.startGame();
    }

    public bool gameHasStarted() {
        return hasStarted;
    }

    public void endGame() {
        guiManager.endGame();
        hasEnded = true;
        endTime = Time.time;
    }

    public bool gameHasEnded() {
        return hasEnded;
    }

    public void unpauseGame(){
        hasPaused = false;
    }

    public bool gameHasPaused() {
        return hasPaused;
    }
}
