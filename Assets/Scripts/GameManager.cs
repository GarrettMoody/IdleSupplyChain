using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    public GameObject tile;
    public Farm farmPrefab;
    public float startX;
    public float startZ;
    public float separationX;
    public float separationZ;
    public float height;
    public float columns;
    public float rows;

    public Button farmButton;
    public PlayerManager playerManager;
	// Use this for initialization
	void Start () {
        CreateTileBoard();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateTileBoard() {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Instantiate(tile, new Vector3(separationX * i + startX, height, separationZ * j + startZ), Quaternion.identity);
            }
        }
    }

    public void OnFarmButtonDown() {
        playerManager.ClearHand();
        Farm newFarm = Instantiate(farmPrefab);
        playerManager.PickUpObject(newFarm.gameObject);
    }
}
