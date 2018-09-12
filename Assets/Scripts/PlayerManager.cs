using System.Collections;
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
        public bool snappedToFreeTile;
    }
    private ObjectInHand objectInHand;

    void Start()
    {
        money = 0f;
        objectInHand = new ObjectInHand
        {
            gameObject = null,
            snappedToFreeTile = false
        };
    }
    // Update is called once per frame
    void Update () {
        //Called first frame of mouse down
        if(Input.GetMouseButton(0)){
            mouseDownTimeDelta += Time.deltaTime;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        //Called first frame of mouse up
        if (Input.GetMouseButtonUp(0))
        {
            //check if object is in hand
            if (objectInHand.gameObject != null)
            {
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    //Pointing to FreeTile
                    if (hit.transform.gameObject.GetComponent<FreeTile>() != null)
                    {
                        //object in hand is tile
                        if (IsObjectTile(objectInHand.gameObject))
                        {
                            if(mouseDownTimeDelta < .25f) {
                                //tile was clicked
                                objectInHand.gameObject.GetComponent<Tile>().OnClick();
                            }
                            //Replace free tile with object in hand
                            objectInHand.gameObject.transform.position = hit.transform.gameObject.transform.position;
                            ShowObjectInHand();
                            gameManager.ReplaceBoardTile(hit.transform.gameObject.GetComponent<Tile>(), objectInHand.gameObject.GetComponent<Tile>());
                            DropItemInHand();
                        }
                    }
                    else
                    {
                        DestroyObjectInHand();
                    }
                }
            }
        }

        //Called first frame of mouse down
        //Hand should be empty
        if (Input.GetMouseButtonDown(0)) {
            mouseDownTimeDelta = 0;
            //Pointing to object
            if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform.gameObject != null) {
                //Object is Tile and not FreeTile
                if(IsObjectTile(hit.transform.gameObject) && hit.transform.gameObject.GetComponent<FreeTile>() == null){
                    PickUpGridObject(hit.transform.gameObject);
                }
            }
        }

        //Update mouse moving
        if (Physics.Raycast(ray, out hit, 100.0f)) {
            //hit new game object this frame
            //if(objectPointingTo != hit.transform.gameObject) {
            //    if (objectPointingTo != null)
            //    {
            //        //if old object was tile, reset tile
            //        if (IsObjectTile(objectPointingTo)) {
            //            objectPointingTo.GetComponent<Tile>().ResetTile();
            //            //was pointing to tile and isn't anymore
            //            if(!IsObjectTile(hit.transform.gameObject) && objectInHand != null) {
            //                ShowObjectInHand();
            //            }
            //        } 
            //    }
            //}

            objectPointingTo = hit.transform.gameObject;
            //object in hand
            if(objectInHand.gameObject != null) {
                //object in hand is tile
                if (IsObjectTile(objectInHand.gameObject))
                {
                    //Pointing at free tile
                    if (hit.transform.gameObject.GetComponent<FreeTile>() != null)
                    {
                        //HideObjectInHand();
                        objectInHand.snappedToFreeTile = true;
                        objectInHand.gameObject.transform.position = hit.transform.position;
                        //objectInHand.gameObject.GetComponent<Tile>()
                        FreeTile freeTile = hit.transform.gameObject.GetComponent<FreeTile>();
                        freeTile.SetMaterial(objectInHand.gameObject.GetComponent<Renderer>().material);
                        freeTile.SetMaterialValue(.7f);
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

}
