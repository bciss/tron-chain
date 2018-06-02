using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class readyButton : MonoBehaviour {
	public InputField value;
	public GameObject gui;
	public GameManager gm;
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
		}
		else
		{
			value.readOnly = !value.readOnly;
			gm.sendReady(float.Parse(value.text));
		}
		return ;
	} 
	public void gogame()
	{
		gui.SetActive(false);
		return;
	}
}
