using UnityEngine;
using System.Collections;

public class MainEngine : MonoBehaviour {
    private GameObject foot;
    private GameObject head;
    private Vector3 footPosition;
    private Vector3 oldFootPosition=new Vector3(0f,0f,0f);
    private GameObject distance;
    private Vector3 velocity;
    private Vector3 velocity2=new Vector3(0f,0f,0f);
    private Vector3 velocity3=new Vector3(0f,0f,0f);
    private Vector3 velocity4 = new Vector3(0f, 0f, 0f);
    private Vector3 velocity5 = new Vector3(0f, 0f, 0f);
    private Vector3 cameraMovementX = new Vector3(1f, 0f, 0f);
    private Vector3 cameraMovementY = new Vector3(0f, 1f, 0f);
    private Vector3 cameraMovementZ = new Vector3(0f, 0f, 1f);
    private GameObject bikeCamera;
    // Use this for initialization
    void Start () {
        if (GameObject.Find("Bike Camera"))
        {
            bikeCamera = GameObject.Find("Bike Camera");
            Debug.Log("Camera found");
        }
        //StartCoroutine(CalcVelocity());
        
    }
    // Update is called once per frame
    void Update()
    {
		mouse();
    }

	void Ankle(){
		if (GameObject.Find("AnkleLeft"))
		{
			foot = GameObject.Find("AnkleLeft");
			
			velocity5 = velocity4;
			velocity4 = velocity3;
			velocity3 = velocity2;
			velocity2 = velocity;
			var tmp = (foot.transform.position - oldFootPosition) / Time.deltaTime;
			oldFootPosition = foot.transform.position;
			velocity = (tmp + velocity2 + velocity3 + velocity4 + velocity5) / 5;
			
			
			Debug.Log(velocity.magnitude);
			if (velocity.magnitude > 1 && velocity.magnitude < 10)
			{
				bikeCamera.transform.position += (cameraMovementZ * velocity.magnitude) / 10;
			}
			if (velocity.magnitude > 10)
			{
				bikeCamera.transform.position += new Vector3(0f,0f,0.4f);
			}
		}
	}

	void mouse(){
		if (Input.GetAxis("Mouse Y")>0 || Input.GetAxis("Mouse Y")<0)
		{
			//foot = GameObject.Find("AnkleLeft");
			
			velocity5 = velocity4;
			velocity4 = velocity3;
			velocity3 = velocity2;
			velocity2 = velocity;
			var tmp = (new Vector3 (0f,Input.GetAxis("Mouse Y"),0f) - oldFootPosition) / Time.deltaTime;
			oldFootPosition = new Vector3(0f,Input.GetAxis("Vertical"),0f);
			velocity = (tmp + velocity2 + velocity3 + velocity4 + velocity5) / 5;
			
			
			//Debug.Log(velocity.magnitude);
			if (velocity.magnitude > 1 && velocity.magnitude < 10)
			{
				bikeCamera.transform.position += (cameraMovementZ * velocity.magnitude) / 10;
			}
			if (velocity.magnitude > 10)
			{
				bikeCamera.transform.position += new Vector3(0f,0f,0.1f);
			}
		}
		Debug.Log(bikeCamera.transform.position);
		if (Input.GetAxis ("Mouse X") > 2 && cameraMovementX [0] < 1.5f) {
			bikeCamera.transform.position += (cameraMovementX + new Vector3 (0.1f, 0f, 0f));
		}
		if (Input.GetAxis ("Mouse X") < -2 && cameraMovementX [0] > -1.5f) {
			bikeCamera.transform.position -= (cameraMovementX - new Vector3 (0.1f, 0f, 0f));
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
