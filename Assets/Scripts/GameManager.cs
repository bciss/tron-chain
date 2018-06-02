using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public PlayerController				playerPrefab;
	public List<PlayerController>	players;
	// Use this for initialization
	void Start () {
		players = new List<PlayerController>();
		players.Add(Instantiate<PlayerController>(playerPrefab, new Vector3(0, 0, 0), transform.rotation));
		InvokeRepeating("UpdatePosition", 0, 0.1f); //calls UpdatePosition() every 2 secs
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void UpdatePosition () {
		foreach (PlayerController player in players)
		{
			player.moveForward();
		}
	}
}
