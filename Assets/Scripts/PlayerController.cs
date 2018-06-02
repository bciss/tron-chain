using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameManager gm;
	public float speed = 10;
	public GameObject square;
	
	private LinkedList<Vector3>	posSquare;
	private Vector3				lastPos;
	private float				angle;
	private string				direction;
	// Use this for initialization
	void Start () {
		posSquare = new LinkedList<Vector3>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != "right") {
			angle = Mathf.Atan2(1, 0) * Mathf.Rad2Deg;
			direction = "left";
		} else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != "up") {
			angle = Mathf.Atan2(0, -1) * Mathf.Rad2Deg;
			direction = "down";
		} else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != "left") {
			angle = Mathf.Atan2(-1, 0) * Mathf.Rad2Deg;
			direction = "right";
		} else if (Input.GetKeyDown(KeyCode.UpArrow) && direction != "down") {
			angle = Mathf.Atan2(0 , 1) * Mathf.Rad2Deg;
			direction = "up";
		}

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public void moveForward() {
		posSquare.AddFirst(transform.position);
		lastPos = transform.position;
		transform.position += transform.up;
		Instantiate(square, lastPos, transform.rotation);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.name);
        // Debug.Log("Trigger ");
    }
}
