using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public CharacterController characterController;
	private ControlInterface controlInterface;
	
	// Higher speeds need higher gravity
	private float gravity = 14.81f;
	
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
	
	// How much you are able to rotate in the air
	private float airborneRotationScale = 0.5f;
	
	// Minimum treshold, below this will equal zero
	private float rotationDeadzone = 0.1f;
	
	// Max treshold, values above this will be reduced
	private float rotationMaxTreshold = 2.0f;
	
	// How much momentum is reduced every frame
	private float momentumReductionFactor = 0.15f;
	private float momentum;
	private float slopeAngle;
	
	// Use this for initialization
	void Start () {
		controlInterface = new ControlInterface ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float velocity = controlInterface.getMovementForward();
		velocity = scaleVelocity(velocity);
		move (velocity);
		
		float rotation = controlInterface.getRotationHorizontal ();
		rotation = scaleRotation (rotation);
		rotateHorizontal (rotation);
	}
	
	private float scaleVelocity(float velocity) {
		if (velocity < velocityMinTreshold) {
			return 0.0f;
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
	
	public void move(float velocity) {
		
		float momentumIncreaseFactor = 0.7f;
		
		if (characterController.isGrounded) {
			velocity *= (1f - normalizeSlopeAngle (slopeAngle));
			momentum = reduceMomentum(momentum, slopeAngle);
		} else {
			momentum -= momentumReductionFactor;
		}
		
		if (momentum < velocity) {

			momentum = Mathf.Max (momentum, momentum + velocity * momentumIncreaseFactor * Time.deltaTime);
			momentum = Mathf.Min (momentum, velocity);
		}
		
		print("Momentum: " + momentum + ", Velocity:" + velocity);
		
		Vector3 moveDirection = characterController.transform.forward * momentum * Time.deltaTime;
		moveDirection.y -= gravity * Time.deltaTime;
		
		characterController.Move(moveDirection);
	}
	
	public void rotateHorizontal(float rotationAngle) {
		
		Vector3 rotationVector;
		
		// If we are in the air, our turn rate is reduced
		if (characterController.isGrounded) {
			rotationVector = new Vector3 (0f, rotationAngle, 0f);
			characterController.transform.Rotate (rotationVector);
		} else {
			rotationVector = new Vector3 (0f, rotationAngle * airborneRotationScale, 0f);
			characterController.transform.Rotate (rotationVector);
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
		// We are comparing the angle between terrain normal and forward vector
		// We need to reduce by 90 to compare terrain direction and forward vector
		slopeAngle = Vector3.Angle(characterController.transform.forward, hit.normal) - 90f;
	}
	
	private float reduceMomentum(float momentum) {
		return reduceMomentum (momentum, 0f);
	}
	
	private float reduceMomentum (float momentum, float slopeAngle) {

		float slopeMomentumReductionFactor = 0.5f;

		float newMomentum = momentum - momentumReductionFactor * (1f + normalizeSlopeAngle(slopeAngle) * slopeMomentumReductionFactor);
		
		if (newMomentum < 0f) {
			return 0f;
		}
		
		return newMomentum;
	}
	
	// Clamps an angle to +- treshold, normalizes to [-1, 1]
	private float normalizeSlopeAngle(float angle) {
		
		float angleMaxTreshold = 90f;
		
		angle = Mathf.Clamp (angle, -angleMaxTreshold, angleMaxTreshold);
		
		return (angle / angleMaxTreshold);
	}
}
