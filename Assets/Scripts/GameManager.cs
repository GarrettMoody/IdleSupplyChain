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
    private bool timerExpired; //Used to show if the timer completed a cycle this frame

	// Use this for initialization
	void Start () {
        gameBoard = new Tile[columns, rows];
        CreateTileBoard();
        timer.value = timer.minValue;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateTimer(Time.deltaTime);

        if(timerExpired) {
            //Iterate through each game tile and see if there is something to do
            foreach (Tile tile in gameBoard) {
                tile.CompleteAction();
            }
        }
        timerExpired = false;
	}

    void CreateTileBoard() {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
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
        int rowIndex, columnIndex;
        GetTileIndexes(boardTile,out rowIndex,out columnIndex);
        gameBoard[columnIndex, rowIndex] = replaceTile;
    }

    public Tile GetTileUp(Tile referenceTile) {
        int rowIndex, columnIndex;
        GetTileIndexes(referenceTile, out rowIndex, out columnIndex);
        //If referenceTile is not on the top row
        return rowIndex < rows - 1 ? gameBoard[columnIndex, rowIndex + 1] : null;
    }

    public Tile GetTileDown(Tile referenceTile)
    {
        int rowIndex, columnIndex;
        GetTileIndexes(referenceTile, out rowIndex, out columnIndex);
        //If referenceTile is not on the bottom row
        return rowIndex > 0 ? gameBoard[columnIndex, rowIndex - 1] : null;
    }

    public Tile GetTileRight(Tile referenceTile)
    {
        int rowIndex, columnIndex;
        GetTileIndexes(referenceTile, out rowIndex, out columnIndex);
        //If referenceTile is not on the far right column
        return columnIndex < columns - 1 ? gameBoard[columnIndex + 1, rowIndex] : null;
    }

    public Tile GetTileLeft(Tile referenceTile)
    {
        int rowIndex, columnIndex;
        GetTileIndexes(referenceTile, out rowIndex, out columnIndex);
        //If referenceTile is not far left column
        return columnIndex > 0 ? gameBoard[columnIndex - 1, rowIndex] : null;
    }

    private void GetTileIndexes(Tile referenceTile, out int rowIndex, out int columnIndex) {
        //retrieves the row and column index of the reference tile in the gameboard. If there is no match in the gameboard
        //the function returns -1 for the indexes. 
        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < rows; j++) {
                if(gameBoard[i, j] == referenceTile) {
                    columnIndex = i;
                    rowIndex = j;
                    return;
                }
            }
        }
        rowIndex = -1;
        columnIndex = -1;
        return;
    }

    public void SendResources(Tile fromTile, int direction, int amount) {
        Tile toTile;
        switch (direction)
        {
            case Direction.DIRECTION_UP:
                toTile = GetTileUp(fromTile);
                //can tile recieve input
                if (toTile.CanInputResources(Direction.DIRECTION_DOWN))
                {
                    toTile.AddResourceNumberValue(amount);
                    fromTile.SubtractResourceNumberValue(amount);
                }

                break;
            case Direction.DIRECTION_RIGHT:
                toTile = GetTileRight(fromTile);
                //can tile recieve input
                if (toTile.CanInputResources(Direction.DIRECTION_LEFT))
                {
                    toTile.AddResourceNumberValue(amount);
                    fromTile.SubtractResourceNumberValue(amount);
                }

                break;
            case Direction.DIRECTION_DOWN:
                toTile = GetTileDown(fromTile);
                //can tile recieve input
                if (toTile.CanInputResources(Direction.DIRECTION_UP))
                {
                    toTile.AddResourceNumberValue(amount);
                    fromTile.SubtractResourceNumberValue(amount);
                }

                break;
            case Direction.DIRECTION_LEFT:
                toTile = GetTileLeft(fromTile);
                //can tile recieve input
                if (toTile.CanInputResources(Direction.DIRECTION_RIGHT))
                {
                    toTile.AddResourceNumberValue(amount);
                    fromTile.SubtractResourceNumberValue(amount);
                }

                break;
        }
    }
}
