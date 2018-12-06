using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dealornodeal : MonoBehaviour {
	public GameScript gameScript;
	public bool isDeal;

	// Use this for initialization
	void Start () {
		GetComponent<Button>().onClick.AddListener(DoClick);
	}
	
	// Update is called once per frame
	void Update () {
		gameScript = GameObject.Find("GameScripts").GetComponent<GameScript>();
	}

	void DoClick(){
		if(gameScript.currentOffer!=0.0f){
			if(isDeal){
				gameScript.RemoveBox(gameScript.lastBox);
				gameScript.Narrator.GetComponent<Text>().text="Congratulations! You win: $"+System.Math.Round(gameScript.currentOffer,2);
				gameScript.SetOffer(0.0f);
			}
			else{
				gameScript.SetOffer(0.0f);
				gameScript.AdvanceRound();
			}
		}
		else if(gameScript.boxSwap){
			if(isDeal){
				GameObject chosenBox = gameScript.chosenBox;
				GameObject lastBox = gameScript.lastBox;
				gameScript.lastBox = chosenBox;
				gameScript.RemoveBox(lastBox);
				chosenBox.GetComponent<RectTransform>().anchoredPosition = lastBox.GetComponent<RectTransform>().anchoredPosition;
				chosenBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300.0f,300.0f);
			}
			else{
				gameScript.RemoveBox(gameScript.lastBox);
				gameScript.lastBox=gameScript.chosenBox;
			}
			gameScript.Narrator.GetComponent<Text>().text="Please open your box!";
		}
	}
}
