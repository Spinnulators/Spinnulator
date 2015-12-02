using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	private float maxPlayTime = 3f * 60f; // Minutes x seconds
	private float startTime;
	private bool enabled = false;

	public void enable() {
		startTime = Time.timeSinceLevelLoad;
		enabled = true;
	}

	public bool hasEnded() {
		float playTime = Time.timeSinceLevelLoad - startTime;
		return playTime >= maxPlayTime;
	}

	void OnGUI () {
		if (!enabled) {
			return;
		}

		float playTime = Time.timeSinceLevelLoad - startTime;
		playTime = Mathf.Clamp (playTime, 0, maxPlayTime);
		
		int minutes = ((int) (maxPlayTime - playTime)) / 60;
		int seconds = ((int) (maxPlayTime - playTime)) % 60;
		
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
}