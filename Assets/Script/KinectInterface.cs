using UnityEngine;
using System.Collections;

public class KinectInterface : MonoBehaviour {

    private bool centered = false;
    
	private Vector3 shoulderInitPosition;
    private Vector3 prevFootPosition = new Vector3(0f, 0f, 0f);

	private KeyboardInterface keyboardController;
	private MotionData ankleVelocityMotionData;

	private int motionDataSize = 20;


    public KinectInterface () {
		ankleVelocityMotionData = new MotionData (motionDataSize);
    }

    public bool isTracking() {
        return GameObject.Find("SpineBase") && GameObject.Find("SpineBase") && GameObject.Find("AnkleRight");
    }
	
	private Vector3 getAnkleRightPosition() {
		return GameObject.Find ("AnkleRight").transform.position;
	}
	
	private Vector3 getSpineTopPosition() {
		return GameObject.Find ("SpineShoulder").transform.position;
	}

    private Vector3 getSpineBasePosition() {
        return GameObject.Find("SpineBase").transform.position;
    }

    private Vector3 getHipLeftPosition() {
        return GameObject.Find("HipLeft").transform.position;
    }

    private Vector3 getHipRightPosition() {
        return GameObject.Find("HipRight").transform.position;
    }

    private Vector3 getHandLeftPosition() {
        return GameObject.Find("HandLeft").transform.position;
    }

    private Vector3 getHandRightPosition() {
        return GameObject.Find("HandRight").transform.position;
    }

    private Vector3 getKneeLeftPosition() {
        return GameObject.Find("KneeLeft").transform.position;
    }

    private Vector3 getKneeRightPosition()
    {
        return GameObject.Find("KneeRight").transform.position;
    }

	// Updates the ankle velocity every frame
	private void updateAnkleVelocity() {
		if (isTracking()) {
			Vector3 footPosition = getAnkleRightPosition ();
			
			Vector3 distance = footPosition - prevFootPosition;

            float dist = distance.magnitude*6f;

            if (dist < 24f) {
                ankleVelocityMotionData.add(dist);
            }
			
			prevFootPosition = footPosition;
		}
	}

	// Get interpolated ankle velocity
	public float getAnkleVelocity () {
        //return distance;
        updateAnkleVelocity();
        return ankleVelocityMotionData.getAverage ();
	}

	// Computes lean in x axis
	public float getHorizontalLean () {
        if (isTracking()) {

            Vector3 spineBase = getSpineBasePosition();
            Vector3 spineTop = getSpineTopPosition();
            Vector3 spineVector = spineTop - spineBase;

            Vector3 hipRight = getHipRightPosition();

            Vector3 hipVector = hipRight - spineBase;
            hipVector.y = 0;

            hipVector.Normalize();

            float lean = Vector3.Dot(spineVector, hipVector);

            
            Debug.Log(lean);

            return lean;
        } else {
            return 0f;
        }
	}

    // Returns whether the player can be assumed to have their hands on the handle
    public bool playerHasHandsOnHandle() {

        // Decides how far ahead the hand should be to the knee to be considered on the handle
        float forwardMagnitudeCutoff = 0.8f;
        
        // The cumulative angle the knees should be at to be considered on the bike
        float cumulativeKneeAngleCutoff = 40;

        Vector3 spineTop = getSpineTopPosition();
        Vector3 spineBase = getSpineBasePosition();

        Vector3 hipLeft = getHipLeftPosition();
        Vector3 kneeLeft = getKneeLeftPosition();
        Vector3 handLeft = getHandLeftPosition();

        Vector3 leftHandHipVector = handLeft - hipLeft;
        Vector3 leftKneeHipVector = kneeLeft - hipLeft;

        float leftHandForwardMagnitude = Vector3.Dot(leftHandHipVector, leftKneeHipVector);

        Vector3 hipRight = getHipRightPosition();
        Vector3 kneeRight = getKneeRightPosition();
        Vector3 handRight = getHandRightPosition();

        Vector3 rightHandHipVector = handRight - hipRight;
        Vector3 rightKneeHipVector = kneeRight - hipRight;

        float rightHandForwardMagnitude = Vector3.Dot(rightHandHipVector, rightKneeHipVector);

        bool handsAreForward = (leftHandForwardMagnitude >= forwardMagnitudeCutoff)
            && (rightHandForwardMagnitude >= forwardMagnitudeCutoff);

        bool kneesAreAngled = Vector3.Angle(spineTop - spineBase, rightKneeHipVector) +
            Vector3.Angle(spineTop - spineBase, rightKneeHipVector) > cumulativeKneeAngleCutoff;

        return handsAreForward && kneesAreAngled;
    }

    private Vector3 getPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
        Vector3 lineVector = lineEnd - lineStart;
        Vector3 pointDiffVector = point - lineStart;
        float pointInLinePercentage = Vector3.Dot(pointDiffVector, lineVector);

        Vector3 pointOnLine = lineStart + pointInLinePercentage * lineEnd;
        return pointOnLine;
    }
}
