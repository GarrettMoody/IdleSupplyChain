using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FreeTile : Tile {

	// Use this for initialization
    protected override void Start () {
        base.Start();
        maxResourceNumber = 0;
	}
	
	// Update is called once per frame
    protected override void Update () {
        base.Update();
	}

}
