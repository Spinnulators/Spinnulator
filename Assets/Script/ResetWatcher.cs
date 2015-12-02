using UnityEngine;
using System.Collections;

public class ResetWatcher : MonoBehaviour {

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.Escape) || Input.GetKey(KeyCode.Mouse2)) {
			Application.LoadLevel (0);
		}
	}
}
