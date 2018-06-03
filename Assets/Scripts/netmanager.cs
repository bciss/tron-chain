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
	protected List<string> chatLog = new List<string> (); 


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
		// gm.InitPlayer(100, 100, "");
		// gm.InitOtherPlayer(50, 50, "");
		socket.On("joinGame", (data) => {
			Debug.Log("recive : joinGame");
			posInit p_params = new posInit();
			p_params = JsonUtility.FromJson<posInit>(data.ToString());
			UnityMainThreadDispatcher.Instance().Enqueue(InitTheMainThread(p_params));
		});

		socket.On("startGame", (data) => {
			Debug.Log("recive : startGame");
			mapInfo p_params = new mapInfo();
			p_params = JsonUtility.FromJson<mapInfo>(data.ToString());
			UnityMainThreadDispatcher.Instance().Enqueue(ThisWillBeExecutedOnTheMainThread(p_params));
		});

		socket.On("idPlayer", (data) => {
			Debug.Log("recive : id");
			string p_params;
			p_params = JsonUtility.FromJson<string>(data.ToString());
			gm.setMyId(p_params);
		});

		socket.On("updateGame", (data) => {
			Debug.Log("recive : updateGame");
			mapInfo p_params = new mapInfo();
			p_params = JsonUtility.FromJson<mapInfo>(data.ToString());
			UnityMainThreadDispatcher.Instance().Enqueue(ThisWillBeExecutedOnTheMainThread(p_params));
		});

	}

	public IEnumerator InitTheMainThread(posInit p_params) {
		gm.InitPlayer(100, 100, "");
		gm.InitOtherPlayer(50, 50, "");
		yield return null;
	}
	public IEnumerator ThisWillBeExecutedOnTheMainThread(mapInfo p_params) {
		Debug.Log ("This is executed from the main thread");
		if (idPlayer == "1") {
			gm.updatePlayer(p_params.p1.x, p_params.p1.y);
			gm.updateOtherPlayer(p_params.p2.x, p_params.p2.y, p_params.p2.direction);
		} else if (idPlayer == "2") {
			gm.updatePlayer(p_params.p2.x, p_params.p2.y);
			gm.updateOtherPlayer(p_params.p1.x, p_params.p1.y, p_params.p1.direction);
		}
			gm.CiaoGuigui();
		
     yield return null;
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
		Debug.Log("sendig : joinGame");
	}

	public void deadGame() {
		socket.Emit("dead", "");
		Debug.Log("sendig : deandGame");
	}

	public void dirPlayer(string p_params) {
		socket.Emit("dirPlayer", p_params);
		Debug.Log("sendig : dirPlayer");
	}











	void Update () {

		string stet = "sa marche bro";
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			socket.Emit("", stet);
		}
	}

}