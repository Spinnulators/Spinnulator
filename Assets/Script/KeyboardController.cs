using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour {

	public float getHorizontalRotation() {
		return Input.GetAxis ("Horizontal");
	}

	public float getForwardMovement() {
		float forward = Input.GetAxis ("Vertical");

		return Mathf.Clamp01 (forward);
	}
}
