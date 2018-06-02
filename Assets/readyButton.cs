using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class readyButton : MonoBehaviour {
	public InputField value;
	public GameObject gui;
	private bool ready = false;
	private Button btn;
	// Use this for initialization
	void Start () {
		btn = this.GetComponent<Button>();
		btn.onClick.AddListener(clickReady);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void clickReady()
	{
		ready = !ready;
		if(ready == true){
			value.readOnly = !value.readOnly;
		//	JoinGame(int.Parse(value.text));
		}
		else
		{
			value.readOnly = !value.readOnly;
		//	JoinGame (-1);
		}
		return ;
	} 
	void gogame()
	{
		gui.SetActive(false);
		return;
	}
}
