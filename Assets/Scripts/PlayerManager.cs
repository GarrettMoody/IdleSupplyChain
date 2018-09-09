using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour {

    public float money;
    GameObject objectInHand;
    GameObject objectPointingTo;

    void Start()
    {
        money = 0f;
        objectInHand = null;
    }
    // Update is called once per frame
    void Update () {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (objectInHand != null) {
            if (Physics.Raycast(ray, out hit, 100.0f)) {
                //Pointing to different object than before
                if(objectPointingTo != hit.transform.gameObject) {
                    //If previous objectPointingTo was FreeTile, return to default color
                    if(objectPointingTo.GetComponent<FreeTile>() != null) {
                        FreeTile tile = objectPointingTo.GetComponent<FreeTile>();
                        tile.SetMaterial(tile.defaultMaterial);
                        tile.SetAlpha(1f);
                    }
                    //Set new objectPointingTo
                    objectPointingTo = hit.transform.gameObject;
                }

                //offset object in hand to appear below other objects
                objectInHand.transform.position = new Vector3(hit.point.x, hit.point.y - .01f, hit.point.z);

                //Pointing at FreeTile, change color to color in hand and hide object in hand
                if (hit.transform.gameObject.GetComponent<FreeTile>() != null) {
                    HideObjectInHand();
                    hit.transform.gameObject.GetComponent<FreeTile>().SetMaterial(objectInHand.GetComponent<Renderer>().material);
                    hit.transform.gameObject.GetComponent<FreeTile>().SetMaterialValue(.7f);
                } else {
                    ShowObjectInHand();
                }
            }
        } 
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
           //Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
            objectPointingTo = hit.transform.gameObject;
        }
    }

    public void PickUpObject(GameObject obj) {
        objectInHand = obj;
    }

    public void ClearHand() {
        if (objectInHand != null) {
            Destroy(objectInHand);
        }
    }

    private void HideObjectInHand() {
        objectInHand.SetActive(false);
    }
     
    private void ShowObjectInHand() {
        objectInHand.SetActive(true);
    }

    public void OnTileDrop() {
        Debug.Log("Tile Drop");
    }

}
