using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
using Quobject.EngineIoClientDotNet.ComponentEmitter;
using Newtonsoft.Json;
using Quobject.EngineIoClientDotNet.Modules;

public class netmanager : MonoBehaviour {
	private Socket socket = IO.Socket("http://guiedo.com:9875");

	public GameManager	gm;
	private string idPlayer;


[Serializable]
public class posInit
{
    public float x;
    public float y;
	public string direction;
}

[Serializable]
public class mapInfo
{	
	public posInit p1;
	public posInit p2;
}

	// Use this for initialization
	void Awake () {

		socket.On("joinGame", (data) => {
			posInit p_params = new posInit();
			p_params = JsonUtility.FromJson<posInit>(data.ToString());
			gm.InitPlayer(p_params.x, p_params.y, p_params.direction);
		});

		socket.On("idPlayer", (data) => {
			string p_params;
			p_params = JsonUtility.FromJson<string>(data.ToString());
			gm.setMyId(p_params);
		});

		socket.On("startGame", (data) => {
			mapInfo p_params = new mapInfo();
			p_params = JsonUtility.FromJson<mapInfo>(data.ToString());
			if (idPlayer == "1") {
				gm.updatePlayer(p_params.p1.x, p_params.p1.y);
				gm.updateOtherPlayer(p_params.p2.x, p_params.p2.y, p_params.p2.direction);
			} else if (idPlayer == "2") {
				gm.updatePlayer(p_params.p2.x, p_params.p2.y);
				gm.updateOtherPlayer(p_params.p1.x, p_params.p1.y, p_params.p1.direction);
			}
			gm.CiaoGuigui();
		});

		socket.On("updateGame", (data) => {
			mapInfo p_params = new mapInfo();
			p_params = JsonUtility.FromJson<mapInfo>(data.ToString());
			if (idPlayer == "1") {
				gm.updatePlayer(p_params.p1.x, p_params.p1.y);
				gm.updateOtherPlayer(p_params.p2.x, p_params.p2.y, p_params.p2.direction);
			} else if (idPlayer == "2") {
				gm.updatePlayer(p_params.p2.x, p_params.p2.y);
				gm.updateOtherPlayer(p_params.p1.x, p_params.p1.y, p_params.p1.direction);
			}
		});
	}

	public void connect(string e)
	{
		Debug.Log(e);
	}
	// Update is called once per frame
		public void up(string e)
	{
		Debug.Log(e);
	}

	public void joinGame(float p_params) {
		socket.Emit("joinGame", p_params);
	}

	public void deadGame() {
		socket.Emit("deadGame", "");
	}

	public void dirPlayer(string p_params) {
		socket.Emit("dirPlayer", p_params);
	}











	void Update () {

		string stet = "sa marche bro";
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			socket.Emit("up", stet);
		}
	}

}