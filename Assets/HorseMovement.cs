using UnityEngine;
using System.Collections;

public class HorseMovement : MonoBehaviour {
    private GameObject horse;
	// Use this for initialization
	void Start () {
        horse = GameObject.Find("Horse");
	}
	
	// Update is called once per frame
	void Update () {
        horse.transform.position += new Vector3 (0f,0f,1f);
	}
}
