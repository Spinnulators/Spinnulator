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

    public bool skeletonIsFound() {
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

	// Updates the ankle velocity every frame
	private void updateAnkleVelocity() {
		if (skeletonIsFound()) {
			Vector3 footPosition = getAnkleRightPosition ();
			
			Vector3 distance = footPosition - prevFootPosition;
            //Debug.Log("actual:" + footPosition + "old:" + prevFootPosition);
            float dist = distance.magnitude*6f;
            //Debug.Log(dist);
            if (dist < 24f)
            {
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
		if (skeletonIsFound()) {

            Vector3 spineBase = getSpineBasePosition();
            Vector3 spineTop = getSpineTopPosition();

            float res = spineTop.x - spineBase.x;
            Debug.Log(res);

            return res;
		}

		return 0f;
	}

    private Vector3 getPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
        Vector3 lineVector = lineEnd - lineStart;
        Vector3 pointDiffVector = point - lineStart;
        float pointInLinePercentage = Vector3.Dot(pointDiffVector, lineVector);

        Vector3 pointOnLine = lineStart + pointInLinePercentage * lineEnd;
        return pointOnLine;
    }
}
