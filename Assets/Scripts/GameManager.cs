using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager : MonoBehaviour {

	public netmanager				netManager;
	public PlayerController			playerPrefab;
	public List<PlayerController>	players;
	public PlayerController			curPlayer;
	

[Serializable]
public class posInit
{
    public float x;
    public float y;
	public string direction;
}

	// Use this for initialization
	void Start () {
		players = new List<PlayerController>();
		// players.Add(Instantiate<PlayerController>(playerPrefab, new Vector3(0, 0, 0), transform.rotation));
		
		InvokeRepeating("UpdatePosition", 0, 0.1f); //calls UpdatePosition() every 2 secs
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void UpdatePosition () {
		foreach (PlayerController player in players)
		{
			// player.moveForward();
		}
	}

	public void InitPlayer(float x, float y, string dir) {
		curPlayer =  Instantiate<PlayerController>(playerPrefab, new Vector3(x, y, 0), transform.rotation);
		curPlayer.Direction(dir);
		players.Add(Instantiate<PlayerController>(playerPrefab, new Vector3(0, 0, 0), transform.rotation));
	}

	public void setMyId(string p_id) {
		curPlayer.myid = p_id;
	}

	public void updatePlayer(float x, float y) {
		curPlayer.UpdatePos(x, y);
	}

	public void updateOtherPlayer(float x, float y, string p_dir) {
		curPlayer.UpdatePos(x, y);
	}

	public void playerDied() {
		netManager.deadGame();
	}
	public void sendDir(string p_params) {
		netManager.dirPlayer(p_params);
	}
}
