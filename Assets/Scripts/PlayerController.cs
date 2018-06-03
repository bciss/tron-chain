using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameManager			gm;
	public float				speed = 10;
	public GameObject			square;
	public string				myid;
	
	private LinkedList<Vector3>	posSquare;
	private Vector3				lastPos;
	private float				angle;
	private string				direction;
	private bool				alive;
	// Use this for initialization
	void Start () {
		posSquare = new LinkedList<Vector3>();
		alive = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (alive) {
			if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != "right") {
				angle = Mathf.Atan2(1, 0) * Mathf.Rad2Deg;
				direction = "left";
		gm.sendDir(direction);
				
			} else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != "up") {
				angle = Mathf.Atan2(0, -1) * Mathf.Rad2Deg;
				direction = "down";
		gm.sendDir(direction);
				
			} else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != "left") {
				angle = Mathf.Atan2(-1, 0) * Mathf.Rad2Deg;
				direction = "right";
		gm.sendDir(direction);
				
			} else if (Input.GetKeyDown(KeyCode.UpArrow) && direction != "down") {
				angle = Mathf.Atan2(0 , 1) * Mathf.Rad2Deg;
				direction = "up";
		gm.sendDir(direction);
				
			}
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}

	public void Direction(string dir) {
		if (dir == "left") {
			angle = Mathf.Atan2(1, 0) * Mathf.Rad2Deg;
			direction = "left";
		} else if (dir == "down") {
			angle = Mathf.Atan2(0, -1) * Mathf.Rad2Deg;
			direction = "down";
		} else if (direction == "right") {
			angle = Mathf.Atan2(-1, 0) * Mathf.Rad2Deg;
			direction = "right";
		} else if (direction == "up") {
			angle = Mathf.Atan2(0 , 1) * Mathf.Rad2Deg;
			direction = "up";
		}
	}

	public void UpdatePos(float x, float y) {
		posSquare.AddFirst(transform.position);
		lastPos = transform.position;
		transform.position = new Vector3(x, y, 0);
		if (transform.position != lastPos) {
			Instantiate(square, lastPos, transform.rotation);
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
		gm.playerDied();
		alive = false;
        // Debug.Log(other.name);
        // Debug.Log("Trigger ");
    }
}
