using UnityEngine;
using System.Collections;

public class KeyboardInterface : MonoBehaviour {

	public float getRotationHorizontal() {
		return Input.GetAxis ("Horizontal");
	}

	public float getMovementForward() {
		float forward = Input.GetAxis ("Vertical");

		return Mathf.Clamp01 (forward);
	}
}
