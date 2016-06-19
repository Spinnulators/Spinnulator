using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiManager : MonoBehaviour {

    public KinectInterface kinectInterface;
    private TimeManager timeManager;
    private GameObject sensorView;

    public GameObject biker;
    public GameObject introPanel;
    public Text introText;
    public GameObject warningPanel;
    public Text warningText;

    void Start() {
        if (GameObject.Find("SensorView")) {
            sensorView = GameObject.Find("SensorView");
        }
    }

    void FixedUpdate() {
        showWarning(kinectInterface.tooManyClosePlayers());
        showTrackingStatus(kinectInterface.isTracking());
    }

    private void showWarning(bool show) {
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

    // TODO knas med denna funktion, då den är under både start och paus med olika funktion
    private void showTrackingStatus(bool tracking){
        if (tracking) {
            if (biker.activeSelf) {
                biker.SetActive(false);
            }

            if (introPanel.activeSelf) {
                introPanel.SetActive(false);
            }

            //introText.text = "";

        }
        else {
            if (!biker.activeSelf) {
                biker.SetActive(true);
            }

            if (!introPanel.activeSelf) {
                introPanel.SetActive(true); 
            }

            //introText.text = Strings.intro;
        }
    }

    public void startGame() {

        if (introPanel.activeSelf) {
            introPanel.SetActive(false);
        }

        introText.text = "";

        timeManager = GameObject.Find("TimeText").GetComponent<TimeManager>();
        timeManager.enable();

        if (sensorView.activeSelf)
        {
            sensorView.SetActive(false);
        }
    }

    public void resetGame() {
        GameObject.Find("TimeText").GetComponent<Text>().text = Strings.time;

        if (!sensorView.activeSelf) {
            sensorView.SetActive(true);
        }

        if (!introPanel.activeSelf) {
            introPanel.SetActive(true);
            //introText.SetActive(true);    
        }

        introText.text = Strings.intro;
    }

    public void endGame() {
        introPanel.SetActive(true);
        introText.text = Strings.end;
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
