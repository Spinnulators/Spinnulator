using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiManager : MonoBehaviour {

    public KinectInterface kinectInterface;
    private GameObject sensorView;
	public GameObject biker;
	public GameManager gameManager;

	public GameObject introPanel;
    public Text introText;
	public GameObject helpPanel;
	public Text helpText;
    public GameObject warningPanel;
    public Text warningText;

    void Start() {
        if (GameObject.Find("SensorView")) {
            sensorView = GameObject.Find("SensorView");
        }
    }

    void FixedUpdate() {
        showTooManyPlayersWarning(kinectInterface.tooManyClosePlayers());

		if (gameManager.gameHasStarted() && !gameManager.gameHasEnded ()) {
			showTrackingStatus (kinectInterface.isTracking());
		}
    }

	/**
	 * Shows a warning if there are too many players
	 */
    private void showTooManyPlayersWarning(bool show) {
        if (show) {
            warningText.text = Strings.warning;

            if (!warningPanel.activeSelf) {
                warningPanel.SetActive(true);
            }
        }
        else {
            warningText.text = "";

            if (warningPanel.activeSelf) {
                warningPanel.SetActive(false);
            }
        }
    }
	
    private void showTrackingStatus(bool tracking){
        if (tracking) {
            if (biker.activeSelf) {
                biker.SetActive(false);
            }

            if (helpPanel.activeSelf) {
                helpPanel.SetActive(false);
            }

            helpText.text = "";
        }
        else {
            if (!biker.activeSelf) {
                biker.SetActive(true);
            }

            if (!helpPanel.activeSelf) {
                helpPanel.SetActive(true); 
            }

            helpText.text = Strings.help;
        }
    }
	
    public void startGame() {

        if (introPanel.activeSelf) {
            introPanel.SetActive(false);
        }

        introText.text = "";

        if (sensorView.activeSelf)
        {
            sensorView.SetActive(false);
        }
    }

	/** 
	 * Resets and initializes the GUI
	 */
    public void resetGame() {

        if (!sensorView.activeSelf) {
            sensorView.SetActive(true);
        }

        if (!introPanel.activeSelf) {
            introPanel.SetActive(true); 
        }

        introText.text = Strings.intro;

		helpPanel.SetActive(false);
		helpText.text = "";
		biker.SetActive(false);
    }

    public void endGame() {
        introPanel.SetActive(true);
        introText.text = Strings.end;

		biker.SetActive (false);
		helpPanel.SetActive(false);
		helpText.text = "";
    }

    public void showStartCountdown(int time) {
        introPanel.SetActive(true);
        introText.text = Strings.starting + time;
    }

    public void hideStartCountdown() {
        introPanel.SetActive(false);
        introText.text = "";
    }
}
