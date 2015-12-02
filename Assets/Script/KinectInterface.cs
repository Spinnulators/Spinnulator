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
	
	public bool isAnkleLeftFound() {
		return GameObject.Find ("AnkleLeft");
	}

	private Vector3 getAnkleLeftPosition() {
		return GameObject.Find ("AnkleLeft").transform.position;
	}

	public bool isShoulderLeftFound() {
		return GameObject.Find ("ShoulderLeft");
	}
	
	private Vector3 getShoulderLeftPosition() {
		return GameObject.Find ("ShoulderLeft").transform.position;
	}

	// Updates the ankle velocity every frame
	private void updateAnkleVelocity() {
		if (isAnkleLeftFound()) {
			Vector3 footPosition = getAnkleLeftPosition ();
			
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

	public void calibrateShoulderPosition() {
		//Skapar en ursprungsposition varifrån alla andra positioner beror.
		shoulderInitPosition = getShoulderLeftPosition();
		centered = true;
	}

	// Computes shoulder movement in x axis
	public float getShoulderMovementHorizontal () {
		if (isShoulderLeftFound()) {

			Vector3 shoulderPosition = getShoulderLeftPosition();

            if (!centered) {
                calibrateShoulderPosition();
            }

			//Tar ut hur mycket du lutat dig i x-led
			float shoulderMovementHorizontal = shoulderPosition.x - shoulderInitPosition.x;

			return shoulderMovementHorizontal;
		}

		return 0f;
	}
}
