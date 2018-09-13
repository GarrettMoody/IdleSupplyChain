using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    //Prefabs
    public GameObject freeTilePrefab;
    public Farm farmPrefab;
    public Store storePrefab;
    public Road roadPrefab;

    //UI
    public Slider timer;
    public Text moneyText;
    public Text researchText;
    public Button farmButton;

    //Gameboard variables
    private Tile [,] gameBoard;
    public float startX;
    public float startZ;
    public float separationX;
    public float separationZ;
    public float height;
    public int columns;
    public int rows;

    //Global variables
    public float timerScale; //The number of seconds it takes to complete a cycle
    public PlayerManager playerManager;
    private bool timerExpired = false; //Used to show if the timer completed a cycle this frame

	// Use this for initialization
	void Start () {
        gameBoard = new Tile[rows, columns];
        CreateTileBoard();
        timer.value = timer.minValue;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateTimer(Time.deltaTime);

        if(timerExpired) {
            Debug.Log("TimerExpired True");
            //Iterate through each game tile and see if there is something to do
            foreach (Tile tile in gameBoard) {
                //Debug.Log(tile.name);
            }
        }

        timerExpired = false;
	}

    void CreateTileBoard() {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject newTile = Instantiate(freeTilePrefab, new Vector3(separationX * i + startX, height, separationZ * j + startZ), Quaternion.identity);
                if(newTile.GetComponent<Tile>() != null) {
                    gameBoard[i,j] = newTile.GetComponent<Tile>();
                }
            }
        }
    }

    public void OnFarmButtonDown() {
        playerManager.DestroyObjectInHand();
        Farm newFarm = Instantiate(farmPrefab);
        playerManager.PickUpObject(newFarm.gameObject);
    }

    public void OnStoreButtonDown() {
        playerManager.DestroyObjectInHand();
        Store newStore = Instantiate(storePrefab);
        playerManager.PickUpObject(newStore.gameObject);
    }
   
    public void OnRoadButtonDown()
    {
        playerManager.DestroyObjectInHand();
        Road newRoad = Instantiate(roadPrefab);
        playerManager.PickUpObject(newRoad.gameObject);
    }

    public void UpdateMoneyText(float money) {
        moneyText.text = "Money: " + money.ToString();
    }

    public void UpdateResearchText(float research) {
        researchText.text = "Research: " + research.ToString();
    }

    public void UpdateTimer(float deltaTime) {
        timer.value += deltaTime / timerScale;
        if(timer.value >= timer.maxValue) {
            timer.value = timer.minValue;
            timerExpired = true;
        }
    }

    public void ReplaceBoardTile(Tile boardTile, Tile replaceTile) {
        for (int rowsIndex = 0; rowsIndex < rows; rowsIndex++) {
            for (int columnsIndex = 0; columnsIndex < columns; columnsIndex++) {
                if(gameBoard[rowsIndex, columnsIndex] == boardTile) {
                    gameBoard[rowsIndex, columnsIndex] = replaceTile;
                }
            }
        }
    }

}
