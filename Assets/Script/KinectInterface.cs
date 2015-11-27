using UnityEngine;
using System.Collections;

public class KinectInterface : MonoBehaviour {

    private bool centered = false;
    
	private Vector3 shoulderInitPosition;
    private Vector3 prevFootPosition = new Vector3(0f, 0f, 0f);

	private KeyboardInterface keyboardController;
	private MotionData ankleVelocityMotionData;

	private int motionDataSize = 20;


    // Use this for initialization
    void Start () {
		ankleVelocityMotionData = new MotionData (motionDataSize);
    }

	void FixedUpdate() {
		updateAnkleVelocity ();
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
			
			float distance = Vector3.Distance (footPosition, prevFootPosition);
			ankleVelocityMotionData.add (distance);
			
			prevFootPosition = footPosition;
		}
	}

	// Get interpolated ankle velocity
	public float getAnkleVelocity () {
		return ankleVelocityMotionData.getAverage ();
	}

	private void initShoulderPosition(Vector3 shoulderPosition) {
		//Skapar en ursprungsposition varifrån alla andra positioner beror.
		shoulderInitPosition = shoulderPosition;
		centered = true;
	}

	// Computes shoulder movement in x axis
	public float getShoulderMovementHorizontal () {
		if (isShoulderLeftFound()) {

			Vector3 shoulderPosition = getShoulderLeftPosition();

			if (centered == false || Input.GetKeyDown (KeyCode.C)) {
				initShoulderPosition(shoulderPosition);
			}

			//Tar ut hur mycket du lutat dig i x-led
			float shoulderMovementHorizontal = shoulderPosition.x - shoulderInitPosition.x;

			return shoulderMovementHorizontal;
		}

		return 0f;
	}
}
