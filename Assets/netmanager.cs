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


[Serializable]
public class Data
{
    public float x;
    public float y;
}

	// Use this for initialization
	void Awake () {

		socket.On("connect", new Action(() => { 
			Debug.Log("entre");
		}));
		
		socket.On("up", (data) => {
			Debug.Log("lol");
			Data tamere = new Data();
			tamere = JsonUtility.FromJson<Data>(data.ToString());
			Debug.Log(tamere.x);
			Debug.Log(tamere.y);
			//up(data);
		});
		//socket.On("connect",Socket.EVENT_CONNECT);
	
		Debug.Log("sortie");

		socket.Emit("up", "ldspogfk");
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
	void Update () {

		string stet = "sa marche bro";
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			socket.Emit("up", stet);
		}
	}

}