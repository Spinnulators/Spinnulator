using UnityEngine;
using System.Collections;

public class SpeedWatcher : MonoBehaviour {

	public PlayerController playerController;
	public AudioSource audioSource;

	private float momentumMaxTreshold = 40f;
	private float momentumMinTreshold = 20f;

	private float momentumMiniTreshold = 10f;
	private float momentumMiniVolume = 0.1f;

	private float volumeScale = 1.5f;

	void FixedUpdate () {

		float momentum = Mathf.Clamp (playerController.momentum, 0, momentumMaxTreshold);

		if (momentum < momentumMiniTreshold) {
			audioSource.volume = 0.05f;
		}

		// Very silent sound at low speeds
		else if (momentum >= momentumMiniTreshold && momentum < momentumMinTreshold) {
			audioSource.volume = momentumMiniVolume;
		} 

		// Higher speed volume
		else {
			float volume = (momentum - momentumMinTreshold) / momentumMaxTreshold;
			volume = Mathf.Clamp (volume, 0, volume);
			audioSource.volume = volume * volumeScale + momentumMiniVolume;
		}
	}
}
