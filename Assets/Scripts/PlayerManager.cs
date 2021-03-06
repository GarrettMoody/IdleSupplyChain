﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour {

    public float money;
    GameObject objectPointingTo;

    public FreeTile freeTilePrefab;
    public GameManager gameManager;

    private float mouseDownTimeDelta;

    private struct ObjectInHand {
        public GameObject gameObject;
    }
    private ObjectInHand objectInHand;

    void Start()
    {
        money = 0f;
        objectInHand = new ObjectInHand
        {
            gameObject = null
        };
    }
    // Update is called once per frame
    void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Called first frame of mouse down
        //Hand should be empty
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownTimeDelta = 0;
        }

        //If mouse button down
        if (Input.GetMouseButton(0))
        {
            mouseDownTimeDelta += Time.deltaTime;
            //if hand is empty and mouse has been down over object for more than .15 seconds
            if (mouseDownTimeDelta > .15f && objectInHand.gameObject == null)
            {
                //Pointing to object
                if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform.gameObject != null)
                {
                    //object pointing at is Tile and not FreeTile
                    if (IsObjectTile(hit.transform.gameObject) && hit.transform.gameObject.GetComponent<FreeTile>() == null)
                    {
                        //pick up object pointing at
                        PickUpGridObject(hit.transform.gameObject);
                    }
                }
            }
        }
        
        //Called first frame of mouse up
        if (Input.GetMouseButtonUp(0))
        {
            //check if object is in hand
            if (objectInHand.gameObject != null)
            {
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    //Pointing at FreeTile
                    if (hit.transform.gameObject.GetComponent<FreeTile>() != null)
                    {
                        //object in hand is tile
                        if (IsObjectTile(objectInHand.gameObject))
                        {
                            if (mouseDownTimeDelta < .15f)
                            {
                                //tile was clicked
                                objectInHand.gameObject.GetComponent<Tile>().OnClick();
                            }
                            //Replace free tile with object in hand
                            objectInHand.gameObject.transform.position = hit.transform.gameObject.transform.position;
                            ShowObjectInHand();
                            gameManager.ReplaceBoardTile(hit.transform.gameObject.GetComponent<Tile>(), objectInHand.gameObject.GetComponent<Tile>());
                            Destroy(hit.transform.gameObject);
                            DropItemInHand();
                        }
                    }
                    else
                    {
                        DestroyObjectInHand();
                    }
                }
            }
            else
            {//hand is empty 
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    //mouse was clicked
                    if (mouseDownTimeDelta < .15f)
                    {
                        //if pointing at a tile thats not freetile
                        if (hit.transform.gameObject.GetComponent<Tile>() != null && hit.transform.gameObject.GetComponent<FreeTile>() == null)
                        {
                            hit.transform.gameObject.GetComponent<Tile>().OnClick();
                        }
                    }
                }
            }
        }

        //Update mouse moving
        if (Physics.Raycast(ray, out hit, 100.0f)) {

            objectPointingTo = hit.transform.gameObject;
            //object in hand
            if(objectInHand.gameObject != null) {
                //object in hand is tile
                if (IsObjectTile(objectInHand.gameObject))
                {
                    //Pointing at free tile
                    if (hit.transform.gameObject.GetComponent<FreeTile>() != null)
                    {
                        objectInHand.gameObject.transform.position = hit.transform.position;
                        objectInHand.gameObject.GetComponent<Tile>().SetMaterialValue(.7f);
                    } else {
                        objectInHand.gameObject.GetComponent<Tile>().ResetTile();
                    }
                    objectInHand.gameObject.transform.position = new Vector3(hit.point.x, hit.point.y - 0.01f, hit.point.z);
                }
            }
        }
      
    }

    public void PickUpObject(GameObject obj) {
        if(IsObjectTile(obj)) {
            obj.GetComponent<Tile>().DeactivateTile();
        }
        objectInHand.gameObject = obj;
    }

    public void PickUpGridObject(GameObject obj) {
        FreeTile newTile = (FreeTile)Instantiate(freeTilePrefab, obj.transform.position, obj.transform.rotation);
        newTile.SetMaterial(obj.GetComponent<Renderer>().material);
        newTile.SetMaterialValue(.7f);
        gameManager.ReplaceBoardTile(obj.GetComponent<Tile>(), newTile);
        PickUpObject(obj);
    }

    public void DropItemInHand() {
        if (IsObjectTile(objectInHand.gameObject)) {
            objectInHand.gameObject.GetComponent<Tile>().ActivateTile();
        }
        objectInHand.gameObject = null;
    }

    public void DestroyObjectInHand() {
        Destroy(objectInHand.gameObject);
    }

    private void HideObjectInHand() {
        objectInHand.gameObject.SetActive(false);
    }
     
    private void ShowObjectInHand() {
        objectInHand.gameObject.SetActive(true);
    }

    private bool IsObjectTile(GameObject gameObject) {
        return gameObject.GetComponent<Tile>() != null ? true : false;
    }

    public void AddMoney(float moneyToAdd) {
        money += moneyToAdd;
        gameManager.UpdateMoneyText(money);
    }

}
