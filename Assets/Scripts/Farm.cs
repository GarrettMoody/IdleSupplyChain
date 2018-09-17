using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Farm : Tile {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        validInput = new bool[] { false, false, false, false }; //cannot input;
        validOutput = new bool[] { false, false, false, false }; 
        validOutput[Direction.DIRECTION_LEFT] = true;

        maxResourceValue = 10;
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    public override void CompleteAction()
    {
        base.CompleteAction();
        AddResourceValue(1);

        //if there is enough resources to go around
        if (resourceValue >= GetNumberOfValidOutputs())
        {
            //for each direction
            for (int index = 0; index < validOutput.Length; index++)
            {
                //if tile can send resources that direction
                if (validOutput[index])
                {
                    //call the GameManager to send resources to adjacent tile
                    playerManager.gameManager.SendResources(this, index, 1);
                }
            }
        }
    }
}
