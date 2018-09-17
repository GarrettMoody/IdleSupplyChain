using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceTransport : MonoBehaviour {

    //UI
    public Canvas transportCanvas;
    public GameObject resourcePanel;
    public Text resourceNumberText;

    //Global Variables
    private int resourceNumber;
    private float startTime;
    private float journeyLength;
    private bool onJourney;

    //GameObject references
    private GameManager gameManager;
    private Tile fromTile;
    private Tile toTile;
    private Vector3 startPosition;
    private Vector3 endPosition;
	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (onJourney) {
            float distanceTraveled = (Time.time - startTime) * (journeyLength / gameManager.timerScale);
            float journeyFraction = distanceTraveled / journeyLength;
            this.transform.position = Vector3.Lerp(startPosition, endPosition, journeyFraction);
            if(this.transform.position == endPosition) {
                onJourney = false;
                JourneyEnded();
            }
        }
	}

    public bool StartJourney(Tile fromTile, Tile toTile, int resourceNumber) {
        //Can take requested amount from tile
        if(fromTile.SubtractResourceValue(resourceNumber)) {
            SetFromTile(fromTile);
            SetToTile(toTile);
            SetResourceNumber(resourceNumber);
            startTime = Time.time;
            journeyLength = Vector3.Distance(startPosition, endPosition);
            onJourney = true;
        }

        return onJourney;
    }

    private void JourneyEnded() {
        toTile.AddResourceValue(resourceNumber);
        Destroy(this.gameObject);
    }

    private void SetResourceNumber(int amount) {
        this.resourceNumber = amount;
        resourceNumberText.text = resourceNumber.ToString();
    }

    private void SetFromTile(Tile tile) {
        fromTile = tile;
        startPosition = fromTile.transform.position;
    }

    private void SetToTile(Tile tile) {
        toTile = tile;
        endPosition = toTile.transform.position;
    }
}
