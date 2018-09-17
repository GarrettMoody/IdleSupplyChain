using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : Tile {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        validInput = new bool[] {true, true, true, true}; //can input anywhere
        validOutput = new bool[] { false, false, false, false }; //cannot output
        maxResourceValue = 10;
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    public override void OnClick()
    {
        base.OnClick();
        playerManager.AddMoney(resourceValue);
        SubtractResourceValue(resourceValue);
    }
}
