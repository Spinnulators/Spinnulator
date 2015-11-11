using UnityEngine;
using System.Collections;

public class MainEngine : MonoBehaviour {
    private GameObject foot;
    private GameObject head;
    private Vector3 footPosition;
    private Vector3 oldFootPosition=new Vector3(0f,0f,0f);
    private GameObject distance;
    private Vector3 velocity;
    private Vector3 cameraMovementX = new Vector3(1f, 0f, 0f);
    private Vector3 cameraMovementY = new Vector3(0f, 1f, 0f);
    private Vector3 cameraMovementZ = new Vector3(0f, 0f, 1f);
    private GameObject bikeCamera;
    // Use this for initialization
    void Start () {
        if (GameObject.Find("Main Camera"))
        {
            bikeCamera = GameObject.Find("Main Camera");
            Debug.Log("Camera found");
        }
        //StartCoroutine(CalcVelocity());
        
    }
    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("AnkleLeft"))
        {
            foot = GameObject.Find("AnkleLeft");
            velocity = (foot.transform.position - oldFootPosition) / Time.deltaTime;
            oldFootPosition = foot.transform.position;
            Debug.Log(velocity.magnitude);
        }
    }

    //IEnumerator CalcVelocity()
    //{
    //    while (Application.isPlaying)
    //    { 
    //    if (GameObject.Find("AnkleLeft"))
    //    {
    //        foot = GameObject.Find("AnkleLeft");

    //    }
    //    if (GameObject.Find("Head"))
    //    {
    //        head = GameObject.Find("Head");
    //    }
    //    while (GameObject.Find("AnkleLeft"))
    //    {
    //        // Position at frame start
    //        oldFootPosition = foot.transform.position;
    //        // Wait till it the end of the frame
    //        yield return new WaitForEndOfFrame();
    //        // Calculate velocity: Velocity = DeltaPosition / DeltaTime
    //        velocity = (oldFootPosition - transform.position) / Time.deltaTime;
    //        Debug.Log(velocity);
    //    }
    //    }
    //}
}
