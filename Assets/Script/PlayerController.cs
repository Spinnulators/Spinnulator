using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public CharacterController characterController;
	private ControlInterface controlInterface;

	/* VELOCITY */
	// How much the velocity is scaled
	private float velocityScaleFactor = 20.0f;

	// Minimum treshold, below this will equal zero
	private float velocityMinTreshold = 0.1f;

	// Max treshold, values above this will be reduced
	private float velocityMaxTreshold = 10.0f;

	/* ROTATION */
	// How much the rotation is scaled
	private float rotationScaleFactor = 1.5f;

	// How much the rotation is scaled by the velocity
	private float rotationVelocityScaleFactor = 0.5f;

	// Minimum treshold, below this will equal zero
	private float rotationDeadzone = 0.1f;

	// Max treshold, values above this will be reduced
	private float rotationMaxTreshold = 2.0f;

	// Use this for initialization
	void Start () {
		controlInterface = new ControlInterface ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float velocity = controlInterface.getMovementForward();
		velocity = scaleVelocity(velocity);
		moveForward (velocity);

		float rotation = controlInterface.getRotationHorizontal ();
		rotation = scaleRotation (rotation);
		rotateHorizontal (rotation);
	}

	private float scaleVelocity(float velocity) {
		if (velocity < velocityMinTreshold) {
			return 0f;
		}

		velocity = Mathf.Clamp (velocity, velocityMinTreshold, velocityMaxTreshold);

		return velocity * velocityScaleFactor;
	}

	public float scaleRotation(float rotation) {

		if (Mathf.Abs(rotation) < rotationDeadzone) {
			return 0f;
		}

		// Set max rotation
		if (rotation > rotationMaxTreshold) {
			rotation = rotationMaxTreshold;
		}

		if (rotation < -rotationMaxTreshold) {
			rotation = -rotationMaxTreshold;
		}

		// At 0 rVSF this equals 1, at 1 rVSF this equals the movement forward
		float rotationVelocityFactor = ((1f - rotationVelocityScaleFactor) + rotationVelocityScaleFactor * controlInterface.getMovementForward ());

		return rotation * rotationScaleFactor * rotationVelocityFactor;
	}

	public void moveForward(float velocity) {

		float gravity = 9.81f;

		Vector3 moveDirection = characterController.transform.forward * velocity * Time.deltaTime;
		moveDirection.y -= gravity * Time.deltaTime;

		characterController.Move(moveDirection);
	}

	public void rotateHorizontal(float rotationAngle) {

		Vector3 rotationVector;
		float airborneRotationScale = 0.5f;

		// If we are in the air, our turn rate is reduced
		if (characterController.isGrounded) {
			rotationVector = new Vector3 (0f, rotationAngle, 0f);
			characterController.transform.Rotate (rotationVector);
		} else {
			rotationVector = new Vector3 (0f, rotationAngle * airborneRotationScale, 0f);
			characterController.transform.Rotate (rotationVector);
		}
	}
}
