using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    //Default material variables
    public Material defaultMaterial;
    private float defaultMaterialAlpha;
    private float defaultMaterialValue;

    //UI variables
    public Canvas tileCanvas;
    public GameObject resourcePanel;
    public Text resourceNumber;

    //Global variables
    protected int resourceNumberValue;
    protected int maxResourceNumber;

    protected bool[] validInput = new bool[4];
    protected bool[] validOutput = new bool[4];

    protected PlayerManager playerManager;
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
            resourceNumberValue = 0;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(resourceNumberValue > 0) {
            ShowResourceNumber();
            UpdateResourceNumber();
        } else {
            HideResourceNumber();
        }
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
        ResetTile();
        this.GetComponent<BoxCollider>().enabled = true;
    }

    public void DeactivateTile() {
        this.GetComponent<BoxCollider>().enabled = false;
    }

    virtual public void OnClick() {
        //Rotate tile 90 degrees
        this.transform.Rotate(0f, 90f, 0f);

        //Change valid input and output to reflect 90 degree turn
        bool inputZero = validInput[validInput.Length - 1];
        bool outputZero = validOutput[validOutput.Length - 1]; 
        for (int i = validInput.Length - 1; i > 0; i--) {
            validInput[i] = validInput[i - 1];
            validOutput[i] = validOutput[i - 1];
        }
            //we just overrode valid[0], need to set valid[3] individually
        validInput[0] = inputZero;
        validOutput[0] = outputZero;
    }

    public virtual void CompleteAction(){

    }

    private void ShowResourceNumber() {
        if (tileCanvas != null && resourcePanel != null & resourceNumber != null)
        {
            tileCanvas.gameObject.SetActive(true);
            resourcePanel.gameObject.SetActive(true);
            resourceNumber.gameObject.SetActive(true);
        }
    }

    private void HideResourceNumber() {
        if(resourceNumber != null) {
            resourceNumber.gameObject.SetActive(false);
        }
    }

    private void UpdateResourceNumber() {
        if(resourceNumber != null) {
            resourceNumber.text = resourceNumberValue.ToString();
        } 
    }

    protected int GetNumberOfValidOutputs() {
        int count = 0;
        foreach(bool output in validOutput) { 
            if(output) {
                count++;
            }
        }
        return count;
    }

    public int GetResourceNumberValue() {
        return resourceNumberValue;
    }

    public void AddResourceNumberValue(int amount) {
        resourceNumberValue += amount;
        if(resourceNumberValue > maxResourceNumber) {
            resourceNumberValue = maxResourceNumber;
        }
    }

    public void SubtractResourceNumberValue(int amount)
    {
        resourceNumberValue -= amount;
        if (resourceNumberValue < 0)
        {
            resourceNumberValue = 0;
        }
    }

    public bool CanInputResources(int direction) {
        return resourceNumberValue < maxResourceNumber && validInput[direction] ? true : false;
    }
}
