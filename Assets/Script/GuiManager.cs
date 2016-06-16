using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiManager : MonoBehaviour {
    public GameObject warningPanel;
    public Text warningText;
    public KinectInterface kinectInterface;
    public GameObject biker;

    void FixedUpdate() {
        if (kinectInterface.tooManyClosePlayers()) {
            warningText.text = "För många spelare i spelområdet";

            if (!warningPanel.activeSelf) {
                warningPanel.SetActive(true);
            }
        } else {
            warningText.text = "";

            if (warningPanel.activeSelf) {
                warningPanel.SetActive(false);
            }
        }

        if (kinectInterface.isTracking()) {
            if (biker.activeSelf) {
                biker.SetActive(false);
            }
        } else if(!biker.activeSelf) {
            biker.SetActive(true);
        }
    }
}
