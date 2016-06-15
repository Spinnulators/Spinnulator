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
		if (kinectInterface.isTracking()) {
			return kinectInterface.getAnkleVelocity();
		} else {
            return keyboardInterface.getMovementForward();
		}
	}

	public float getRotationHorizontal() {
		if (kinectInterface.isTracking()) {
			return kinectInterface.getHorizontalLean();
		} else {
			return keyboardInterface.getRotationHorizontal();
		}
	}

    public bool isCalibrateKeyPressed() {
        return (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.Mouse1));
    }

	public bool isStartKeyPressed() {
		return (keyboardInterface.isSpacebarPressed () || Input.GetKey(KeyCode.Mouse0));
	}

    public bool isReverseRotationKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    public bool isToggleKinectViewKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.K);
    }
}
