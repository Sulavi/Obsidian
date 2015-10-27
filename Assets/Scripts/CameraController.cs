using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    Transform player;

	// Use this for initialization
	void Start () {
	    player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(player.position.x, player.position.y, -10);
	}
}
