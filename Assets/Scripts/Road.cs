using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Tile {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        validInput = new bool[] { true, true, true, true };
        validOutput = new bool[] { false, false, false, false };
        validOutput[Direction.DIRECTION_LEFT] = true;
        maxResourceNumber = 2;
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    public override void CompleteAction()
    {
        base.CompleteAction();
        //if there is enough resources to go around
        if (resourceNumberValue >= GetNumberOfValidOutputs())
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
