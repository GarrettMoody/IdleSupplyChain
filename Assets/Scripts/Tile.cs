using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{

    public Material defaultMaterial;
    private float defaultMaterialAlpha;
    private float defaultMaterialValue;

    public Canvas tileCanvas;
    public GameObject stockPanel;
    public Text stockNumber;
    private int stockNumberValue;

    protected enum Direction {Up, Right, Down, Left}; //DO NOT CHANGE. Order matters.
    protected bool[] validInput = new bool[4];
    protected bool[] validOutput = new bool[4];

    private PlayerManager playerManager;
    // Use this for initialization
    protected virtual void Start()
    {
        //Set Object material and default color options
        this.GetComponent<Renderer>().material = defaultMaterial;
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        defaultMaterialAlpha = defaultMaterial.color.a;
        float h, s;
        Color.RGBToHSV(defaultMaterial.color, out h,out s,out defaultMaterialValue);

        //not all tiles will have a canvas
        if(tileCanvas != null) {
            tileCanvas.gameObject.SetActive(false);
            stockNumberValue = 0;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public void SetMaterial(Material newMaterial)
    {
        this.GetComponent<Renderer>().material = newMaterial;
    }

    public void SetAlpha(float alpha)
    {
        Color color = this.GetComponent<Renderer>().material.color;
        color.a = alpha;
        this.GetComponent<Renderer>().material.color = color;
    }

    public void SetMaterialValue(float value)
    {
        Color color = this.GetComponent<Renderer>().material.color;
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        color = Color.HSVToRGB(h, s, value);
        this.GetComponent<Renderer>().material.color = color;
    }

    public void ResetTile()
    {
        SetMaterial(defaultMaterial);
        SetAlpha(defaultMaterialAlpha);
        SetMaterialValue(defaultMaterialValue);
    }

    public void ActivateTile() {
        this.GetComponent<BoxCollider>().enabled = true;
    }

    public void DeactivateTile() {
        this.GetComponent<BoxCollider>().enabled = false;
    }

    virtual public void OnClick() {
        //Rotate tile 90 degrees
        this.transform.Rotate(0f, 90f, 0f);

        //Change valid input and output to reflect 90 degree turn
        bool inputZero = validInput[0];
        bool outputZero = validOutput[0];
        for (int i = 0; i < validInput.Length - 2; i++) {
            validInput[i] = validInput[i + 1];
            validOutput[i] = validOutput[i + 1];
        }
            //we just overrode valid[0], need to set valid[3] individually
        validInput[validInput.Length - 1] = inputZero;
        validOutput[validOutput.Length - 1] = outputZero;
    }
}
