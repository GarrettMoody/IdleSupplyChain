using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraFacingBillboard : MonoBehaviour {

    private void Start()
    {

    }
    // Update is called once per frame
    void Update () {
        this.transform.LookAt(this.transform.position + Camera.main.transform.rotation * Vector3.forward, 
                              Camera.main.transform.rotation * Vector3.up);
	}
}
