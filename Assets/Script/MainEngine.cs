using UnityEngine;
using System.Collections;

public class MainEngine : MonoBehaviour {
    private GameObject foot;
    private GameObject shoulder;
	private GameObject bikeCamera;

    private bool centered = false;
    
	private Vector3 shoulderInitPosition;
    private float shoulderMovement;
	private float shoulderMovementDeadZone = 0.2f;
    private Vector3 footPosition;
    private Vector3 prevFootPosition = new Vector3(0f, 0f, 0f);

	private KeyboardController keyboardController;
	public CharacterController characterController;
	private MotionData motionData;

	private int motionDataSize = 20;
	private float velocityMinTreshold = 0.1f;
	private float velocityMaxTreshold = 2.0f;

    // Use this for initialization
    void Start () {
        if (GameObject.Find("Bike Camera"))
        {
            bikeCamera = GameObject.Find("Bike Camera");
            Debug.Log("Camera found");
        }

		motionData = new MotionData (motionDataSize);
		keyboardController = new KeyboardController ();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
		computeFootTracking ();
		computeShoulderTracking ();
    }

	public void computeFootTracking () {

		if (GameObject.Find ("AnkleLeft")) {
			foot = GameObject.Find ("AnkleLeft");
			
			Vector3 vectorDistance = foot.transform.position - prevFootPosition;
			motionData.add (vectorDistance.magnitude);
			
			prevFootPosition = foot.transform.position;
			
			float movementScaleFactor = 0.8f;
			float averageVelocity = motionData.getAverage ();
			
			// Clamp velocity to max treshhold
			averageVelocity = Mathf.Clamp(averageVelocity, 0f, velocityMaxTreshold);

			// If the speed is above a treshhold, move the character
			if (averageVelocity > velocityMinTreshold) {
				moveForward (averageVelocity * movementScaleFactor);
			}
		} else {
			// If we don't have Kinect tracking, use keyboard
			moveForward(keyboardController.getForwardMovement() * 20.0f);
		}
	}

	public void computeShoulderTracking () {
		if (GameObject.Find ("ShoulderLeft")) {
			shoulder = GameObject.Find ("ShoulderLeft");
			if (centered == false) {
				//Skapar en ursprungsposition varifrån alla andra positioner beror.
				shoulderInitPosition = shoulder.transform.position;
				centered = true;
			}
			if (Input.GetKeyDown (KeyCode.C)) {
				//Resettar ursprungspositionen om det känns weird
				shoulderInitPosition = shoulder.transform.position;
				centered = true;
			}
			if (centered == true) {
				//Tar ut hur mycket du lutat dig i x-led
				shoulderMovement = shoulder.transform.position.x - shoulderInitPosition.x;

				// om du lutar dig utanför deadzone så roteras kameran
				if (Mathf.Abs (shoulderMovement) > shoulderMovementDeadZone) {
					float rotationScalingFactor = motionData.getAverage () * 1.2f;
					rotateHorizontal (shoulderMovement * rotationScalingFactor);
				}
				
			}
		} else {
			// If we don't have Kinect tracking, use keyboard
			rotateHorizontal(keyboardController.getHorizontalRotation());
		}
	}

	public void moveForward(float speed) {
		//bikeCamera.transform.position += bikeCamera.transform.forward * speed;

		//float slopeSpeedMultiplier = AnimationCurve(Keyframe(-90, 1), Keyframe(0, 1), Keyframe(90, 0));

		characterController.SimpleMove(bikeCamera.transform.forward * speed);
	}

	public void rotateHorizontal(float rotationAngle) {
		Vector3 rotationVector = new Vector3(0f, rotationAngle, 0f);
		bikeCamera.transform.Rotate(rotationVector);
	}
}
