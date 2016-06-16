using UnityEngine;
using System.Collections;

public class ControlInterface : MonoBehaviour {

	public KinectInterface kinectInterface;

	public float getMovementForward() {
		if (kinectInterface.isTracking()) {
			return kinectInterface.getAnkleVelocity();
		} else {
            return getKeyboardMovementForward();
		}
	}

	public float getRotationHorizontal() {
		if (kinectInterface.isTracking()) {
			return kinectInterface.getHorizontalLean();
		} else {
			return getKeyboardRotationHorizontal();
		}
	}

    public bool isCalibrateKeyPressed() {
        return (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.Mouse1));
    }

	public bool isStartKeyPressed() {
		return (isSpacebarPressed ()
            || Input.GetKey(KeyCode.Mouse0));
	}

    public bool isReverseRotationKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    public bool isToggleKinectViewKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.K);
    }

    public float getKeyboardRotationHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }

    public float getKeyboardMovementForward()
    {
        float forward = Input.GetAxis("Vertical");
        return Mathf.Clamp01(forward);
    }

    public bool isSpacebarPressed()
    {
        return Input.GetKey(KeyCode.Space);
    }
}
