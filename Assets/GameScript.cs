using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameScript : MonoBehaviour {
	float[] boxPrizes;
	public float currentOffer;
	List<GameObject> boxList;
	public GameObject boxButton, canvas, chosenBox, Narrator, offerText, lastBox;
	public int currentRound, boxesToChoose;
	public int[] rounds;
	public bool boxSwap;

	// This will set the amount of money the banker is offering the player
	public void SetOffer(float amount){
		currentOffer = amount;
		// This will set the dialogue box to tell the player how much the banker is offering them
		offerText.GetComponent<Text>().text = "Banker offer\n$"+System.Math.Round(amount, 2);
	}

	public void MakeOffer(){
		// This is how much is in all of the boxes
		float totalFromBoxes = 0;
		// This is the amount of boxes to divide the value by
		int totalDivide = 0;
		// We go through every box still in the game
		for(int i = 0;i<boxList.Count;i+=1){
			// This checks to see if the box is red, we only check red values
			if(boxList[i].GetComponent<BoxScript>().boxValue>=1000.0f){
				// We add the value of the box to the total
				totalFromBoxes+=boxList[i].GetComponent<BoxScript>().boxValue;
				totalDivide+=1;
			}
		}
		// This works out the average of all boxes
		float averageOfBoxes = totalFromBoxes / boxList.Count;

		// This algorithm sets the banker's offer, It starts by dividing the average of the boxes by the total boxes
		// It then divides the value by the remaining rounds
		SetOffer((averageOfBoxes/boxList.Count) / Mathf.Clamp(rounds.Length-currentRound,1,rounds.Length));
	}
	
	// This is used to add a box to the screen
	GameObject CreateBox(int boxNumber,float boxValue,Vector2 boxPosition){
		// We create a gameobject and attach it to our screen
		GameObject spawnedBox = Instantiate(boxButton,canvas.transform,false);
		// We move the box to a specified position
		spawnedBox.GetComponent<RectTransform>().anchoredPosition = boxPosition;
		// We set the box text to the box's number
		spawnedBox.transform.GetChild(0).gameObject.GetComponent<Text>().text=""+boxNumber;
		// We tell the box what it's number and value are
		spawnedBox.GetComponent<BoxScript>().boxNumber=boxNumber;
		spawnedBox.GetComponent<BoxScript>().boxValue=boxValue;
		return spawnedBox;
	}

	// This is used to remove a box from the game and open it
	public void RemoveBox(GameObject box){
		// We go through every box to find the one we want to remove
		for(int i = 0;i<boxList.Count;i+=1){
			// If the current box is our box
			if(boxList[i]==box){
				// Then we remove it from the list
				boxList.RemoveAt(i);
				if(currentRound!=-1){
					// We set the box's text to the value of the box
					box.transform.GetChild(0).gameObject.GetComponent<Text>().fontSize=10;
					box.transform.GetChild(0).gameObject.GetComponent<Text>().text="$"+box.GetComponent<BoxScript>().boxValue;
				}
				// If we are on the last box, then we store it
				if(boxList.Count==1){
					lastBox=boxList[0];
				}
				break;
			}
		}
	}

	// This increases the round by 1
	public void AdvanceRound(){
		// We only increase the round if we are not on the final round already
		if(currentRound-1!=rounds.Length){
			// Increase the current round by 1
			currentRound+=1;
			// Tell the game how many boxes the player gets to choose
			boxesToChoose=rounds[currentRound];
			// The narrator tells the player how many boxes he gets to choose
			Narrator.GetComponent<Text>().text="Please choose "+boxesToChoose+" boxes!";
		}
	}

	// Use this for initialization
	void Start () {
		// We set the player's boxes to choose to 0 and the banker's offer is also 0 at the start of the game
		currentOffer=0.0f;
		boxesToChoose=0;
		// We use -1 to show that the game has not yet begun
		currentRound = -1;
		// This is how many boxes the player chooses each round
		rounds = new int[] {
			3,5,4,
			5,4,3
		};
		// We create a list to store all of our boxes
		boxList = new List<GameObject>();
		// We tell the game what prizes the player can win
		boxPrizes = new float[] {
			0.01f,1.0f,5.0f,
			10.0f,25.0f,50.0f,
			75.0f,100.0f,200.0f,
			300.0f,400.0f,500.0f,
			750.0f,1000.0f,5000.0f,
			10000.0f,25000.0f,50000.0f,
			75000.0f,100000.0f,200000.0f,
			300000.0f,400000.0f,500000.0f,
			750000.0f,1000000.0f
		};

		// This randomises the boxes so player's don't know what is in each box
		for(int i = 0;i<boxPrizes.Length;i+=1){
			// We store the value of our current box
			float saveBox = boxPrizes[i];
			// We select another box at random
			int randomSelect = Random.Range(0,boxPrizes.Length-1);
			// We then swap the two values
			boxPrizes[i]=boxPrizes[randomSelect];
			boxPrizes[randomSelect]=saveBox;
		}

		// The default positions for the first box
		float x = -350.0f;
		float y = -400.0f + (Mathf.Ceil(boxPrizes.Length/11.0f)*70.0f);
		// This goes through every prize possible and creates a box for that prize
		for(int i = 0;i<boxPrizes.Length;i+=1){
			// We create a box at a specified position and set its value to the prize value
			boxList.Add(CreateBox(i+1,boxPrizes[i],new Vector2(x,y)));
			// We tell the next box to spawn 80 pixels away from the previous box
			x+=80.0f;
			// If the next box were to spawn off screen then we tell it to start a new line
			if(x >= 420.0f){
				x=-350.0f;
				y -=80.0f;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Restart the game when the "R" key is pressed
		if(Input.GetKey("r")){
			SceneManager.LoadScene(0);
		}
	}
}
