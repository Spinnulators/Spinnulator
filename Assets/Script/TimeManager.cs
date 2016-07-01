using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	private float maxPlayTime = 2.0f * 60f; // Minutes x seconds
	private float startTime;
	private bool enabled = false;

	public void start() {
		startTime = Time.time;
		enabled = true;
	}
    public void end() {
        enabled = false;
    }

	public bool isEnabled() {
		return enabled;
	}

	private float getCurrentPlaytime() {

		if (enabled) {
			return Time.timeSinceLevelLoad - startTime;
		}

		return 0f;
	}

	public bool hasEnded() {
		return getCurrentPlaytime() >= maxPlayTime;
	}

	public void updateGUI(float currentPlaytime) {
		currentPlaytime = Mathf.Clamp (currentPlaytime, 0, maxPlayTime);
		
		int minutes = ((int) (maxPlayTime - currentPlaytime)) / 60;
		int seconds = ((int) (maxPlayTime - currentPlaytime)) % 60;
		
		string minStr = minutes.ToString();
		string secStr = seconds.ToString();
		
		if (minStr.Length == 1 ){
			minStr = "0" + minStr;
		}
		
		if (secStr.Length == 1) {
			secStr = "0" + secStr;
		}
		
		GameObject.Find ("TimeText").GetComponent<Text>().text = minStr + " : " + secStr;
	}

	void OnGUI () {

		if (enabled) {
			updateGUI (getCurrentPlaytime());
		} else {
			updateGUI (0f);
		}
	}
}