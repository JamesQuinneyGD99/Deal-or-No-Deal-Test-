using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxScript : MonoBehaviour {
	// Each box will have a number and prize value
	public int boxNumber;
	public float boxValue;
	// This is whether the player has opened this box
	public bool isPicked;
	// This is for retrieving information from the banker
	GameScript gameScript;

	// Use this for initialization
	void Start () {
		// When the game begins we tell the box that is has not been picked yet
		isPicked=false;
		// We tell the box how to talk to the banker
		gameScript=GameObject.Find("GameScripts").GetComponent<GameScript>();
		// We tell the button what to look for when it is clicked
		GetComponent<Button>().onClick.AddListener(DoClick);
	}

	// This is what happens when you click the button
	void DoClick(){
		// This is the text used to give the player information
		Text narratorText = gameScript.Narrator.GetComponent<Text>();

		// We check to see if the game hasn't begun yet
		if(gameScript.currentRound==-1){
			// We give this box to the player
			gameScript.chosenBox=this.gameObject;
			gameScript.RemoveBox(this.gameObject);
			// We tell the box that it can no longer be opened
			isPicked=true;
			// We move the player's box
			GetComponent<RectTransform>().anchoredPosition = new Vector2(-300.0f,300.0f);
			// We tell the game to move forward one round
			gameScript.AdvanceRound();
		}
		// We check to see if the player has boxes to choose, we then check to see if this box can be chosen
		else if(gameScript.boxesToChoose>0&&!isPicked){
			// We tell the game that the player has chosen a box
			gameScript.boxesToChoose-=1;
			// We remove the box from the game and open it
			gameScript.RemoveBox(this.gameObject);
			// We tell this box that it can no longer be chosen
			isPicked=true;
			// We check to see if the player can not choose any more boxes
			if(gameScript.boxesToChoose==0){
				// We check to see if we are not on the final round
				if(gameScript.currentRound!=gameScript.rounds.Length-1){
					// The banker makes the player and offer
					narratorText.text="The banker has made you an offer!\nDeal or no deal?";
					gameScript.MakeOffer();
				}
				// This happens if we are on the last round
				else{
					// The banker asks the player if he wants to swap his box
					narratorText.text="The banker would like to offer a box swap!\nDeal or no deal?";
					gameScript.boxSwap=true;
				}
			}
			else{
				// We update the narrator's text
				narratorText.text="Please choose "+gameScript.boxesToChoose+" boxes!";
			}
		}
		// We check to see if we are on the last box, we also check to see if this is the last box
		else if(gameScript.lastBox!=null&&gameScript.lastBox==this.gameObject){
			// We check to see if the last box is the player's chosen box
			if(gameScript.lastBox==gameScript.chosenBox){
				// We tell the box that it can no longer be chosen
				gameScript.RemoveBox(this.gameObject);
				// We tell the player the prize they have won
				narratorText.text="Congratulations! You win: $"+ System.Math.Round(boxValue,2);
			}
		}
	}
}
