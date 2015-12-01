using UnityEngine;
using System.Collections;

public class ResetWatcher : MonoBehaviour {

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.Escape)) {
			Application.LoadLevel (0);
		}
	}
}
