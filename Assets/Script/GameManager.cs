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

    private bool hasStarted;
    private bool hasEnded;
    private bool hasPaused;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private float endTime;

    void Start() {
        apples = GameObject.FindGameObjectsWithTag("Apple");
        introText.text = Strings.intro;
        GameObject.Find("TimeText").GetComponent<Text>().text = Strings.time;

        startPosition = player.transform.position;
        startRotation = player.transform.rotation;
    }

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.Escape) || Input.GetKey(KeyCode.Mouse2)) {
            resetGame();
		}

        if (timeManager != null && timeManager.hasEnded() && !gameHasEnded()) {
            endGame();
        }

        if (!gameHasStarted()) {
            playerOnBikeStartCountdown();
        }

        if (hasEnded) {
            int timeSinceEnd = (int) (Time.time - endTime);

            if (timeSinceEnd > 5) {
                resetGame();
            }
        }
    }

    void resetGame() {
        player.transform.position = new Vector3(-21.28f, 20.32f, 13.42f);
        player.transform.rotation = Quaternion.identity;
        playerController.momentum = 0;
        collectable.countSum = 0;

        timeManager.disable();

        guiManager.resetGame();

        hasStarted = false;
        hasEnded = false;

        collectable.counterText.text = collectable.countSum.ToString();

        foreach (GameObject apple in apples) {
            if (!apple.activeSelf) {
                apple.SetActive(true);
            }
        }
    }

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

    public void startGame() {
        hasStarted = true;
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
