using UnityEngine;
using System.Collections;
using Kinect = Windows.Kinect;
using System.Collections.Generic;

public class KinectInterface : MonoBehaviour {

    public GameObject bodySourceGameObject;
    private BodySourceManager bodySourceManager;
    private Dictionary<ulong, GameObject> bodyIdToBodyObjectMap = new Dictionary<ulong, GameObject>();

    public GameObject bikeSeat;
    private GameObject closestPlayer;
    private ulong closestPlayerId;
    private float timePlayerStartedOnBike;

    private Vector3 prevFootPosition = new Vector3(0f, 0f, 0f);

	private KeyboardInterface keyboardController;
	private MotionData ankleVelocityMotionData;
    private Kinect.Body[] bodies;

    // The maximum distance the player is allowed to be away from the bike
    private float playerDistanceCutoff = 9.0f;
    private int motionDataSize = 20;

    private int numTrackedPlayers;
    private int numTrackedClosePlayers;

    void Start() {
        ankleVelocityMotionData = new MotionData(motionDataSize);
    }

    void FixedUpdate() {

        bodySourceGameObject = GameObject.Find("BodyManager");

        if (bodySourceGameObject == null) {
            Debug.Log("Couldn't find body manager object");
            return;
        }

        bodySourceManager = bodySourceGameObject.GetComponent<BodySourceManager>();
        if (bodySourceManager == null) {
            Debug.Log("Couldn't find BodySourceManager");
            return;
        }

        bodies = bodySourceManager.GetData();
        
        if (bodies == null) {
            return;
        }

        updateClosestPlayer();
    }

    void resetTime() {
        // Resets the time on bike, so it doesn't grow too large
        timePlayerStartedOnBike = Time.time;
    }

    private Kinect.Body[] getBodies() {
        return bodies;
    }

    public bool isTracking() {
        return (closestPlayer != null && getChildGameObject(closestPlayer, "SpineBase") != null && 
            playerDistanceFromBike(closestPlayer) < playerDistanceCutoff);
    }

    private float playerDistanceFromBike(GameObject bodyObject) {
        GameObject spineBase = getChildGameObject(bodyObject, "SpineBase");

        float distance = Vector3.Distance(spineBase.transform.position,
            bikeSeat.transform.position);

        //Debug.Log(distance);
        return distance;
    }

    private void updateClosestPlayer() {

        float closestDistance = 999f;
        GameObject closestPlayerTemp = null;
        ulong closestPlayerIdTemp = 0;

        int numTrackedPlayersTemp = 0;
        int numTrackedClosePlayersTemp = 0;

        foreach(var body in getBodies()) {
            if (body.IsTracked){

                GameObject bodyObject;

                if (!bodyIdToBodyObjectMap.ContainsKey(body.TrackingId)) {
                    bodyObject = GameObject.Find("Body:" + body.TrackingId);
                    bodyIdToBodyObjectMap.Add(body.TrackingId, bodyObject);
                }

                bodyIdToBodyObjectMap.TryGetValue(body.TrackingId, out bodyObject);

                float distance = playerDistanceFromBike(bodyObject);

                // Checks if this is the closest player, and if the player is close enough
                if (distance < playerDistanceCutoff) {

                    numTrackedClosePlayersTemp++;

                    if(distance < closestDistance) {
                        numTrackedClosePlayersTemp++;
                        closestPlayerTemp = bodyObject;
                        closestPlayerIdTemp = body.TrackingId;
                    }
                }
            }
        }

        if (closestPlayerTemp != closestPlayer) {
            timePlayerStartedOnBike = Time.time;
        }

        closestPlayer = closestPlayerTemp;
        closestPlayerId = closestPlayerIdTemp;

        numTrackedClosePlayers = numTrackedClosePlayersTemp;
        numTrackedPlayers = numTrackedPlayersTemp;
    }

    // When player got on the bike
    public float getTimePlayerStartedOnBike() {
        return timePlayerStartedOnBike;
    }

    public int getNumTrackedPlayers() {
        return numTrackedPlayers;
    }

    public int getNumTrackedClosePlayers() {
        return numTrackedClosePlayers;
    }

    public bool tooManyClosePlayers() {
        return (getNumTrackedClosePlayers() > 2 || getNumTrackedPlayers() > 4);
    }
	
	private Vector3 getAnkleRightPosition() {
        return getChildGameObject(closestPlayer, "AnkleLeft").transform.position;
	}
	
	private Vector3 getSpineTopPosition() {
		return getChildGameObject(closestPlayer, "SpineShoulder").transform.position;
	}

    private Vector3 getSpineBasePosition() {
        return getChildGameObject(closestPlayer, "SpineBase").transform.position;
    }

    private Vector3 getHipLeftPosition() {
        return getChildGameObject(closestPlayer, "HipLeft").transform.position;
    }

    private Vector3 getHipRightPosition() {
        return getChildGameObject(closestPlayer, "HipRight").transform.position;
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

            Vector3 spineVector = getSpineTopPosition() - getSpineBasePosition();
            Vector3 hipVector = getHipRightPosition() - getHipLeftPosition();
            hipVector.y = 0;

            hipVector.Normalize();

            float lean = Vector3.Dot(spineVector, hipVector);

            return lean;
        } else {
            return 0f;
        }
	}

    // Simple horizontal lean
    public float getHorizontalLeanAlt() {
        if (isTracking()) {

            foreach (var body in bodies) {
                if (body.TrackingId == closestPlayerId) {
                    return body.Lean.X * 1.5f;
                }
            }
        }

        return 0f;
    }

    private GameObject getChildGameObject(GameObject fromGameObject, string withName) {

        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        
        foreach (Transform t in ts) {
            if (t.gameObject.name == withName) {
                return t.gameObject;
            }
        }

        return null;
    }
}
