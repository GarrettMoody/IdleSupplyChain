using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Farm : Tile {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        validInput[(int)Direction.Up] = false;
        validInput[(int)Direction.Right] = false;
        validInput[(int)Direction.Down] = false;
        validInput[(int)Direction.Left] = false;
        validOutput[(int)Direction.Up] = false;
        validOutput[(int)Direction.Right] = false;
        validOutput[(int)Direction.Down] = false;
        validOutput[(int)Direction.Left] = true;
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}
}
