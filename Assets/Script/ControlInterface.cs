using UnityEngine;
using System.Collections;

public class ControlInterface : MonoBehaviour {

	private KinectInterface kinectInterface;
	private KeyboardInterface keyboardInterface;

	// Use this for initialization
	public ControlInterface () {
		kinectInterface = new KinectInterface ();
		keyboardInterface = new KeyboardInterface ();
	}

	public float getMovementForward() {
		if (kinectInterface.isAnkleLeftFound ()) {
			return kinectInterface.getAnkleVelocity();
		} else {
            return keyboardInterface.getMovementForward();
		}
	}

	public float getRotationHorizontal() {
		if (kinectInterface.isShoulderLeftFound ()) {
			return kinectInterface.getShoulderMovementHorizontal();
		} else {
			return keyboardInterface.getRotationHorizontal();
		}
	}

	public void calibrate() {
		if (kinectInterface.isShoulderLeftFound ()) {
			kinectInterface.calibrateShoulderPosition ();
		}
	}

	public bool isSpacebarPressed() {
		return keyboardInterface.isSpacebarPressed ();
	}
}
