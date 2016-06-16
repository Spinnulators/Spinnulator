using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiManager : MonoBehaviour {
    public GameObject warningPanel;
    public Text warningText;
    public KinectInterface kinectInterface;
    public GameObject biker;

    void FixedUpdate() {
        if (kinectInterface.tooManyClosePlayers())
        {
            warningText.text = "För många spelare i spelområdet";
            warningPanel.SetActive(true);
        } else {
            warningText.text = "";
            warningPanel.SetActive(false);
        }

        if (kinectInterface.isTracking()) {
            biker.SetActive(false);
        } else {
            biker.SetActive(true);
        }
    }
}
