using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{

    public Material defaultMaterial;
    private PlayerManager playerManager;
    // Use this for initialization
    protected virtual void Start()
    {
        this.GetComponent<Renderer>().material = defaultMaterial;
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
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

    virtual public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("T Drag");
        this.transform.position = Input.mousePosition;
    }

    virtual public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("T End Drag");
    }

    virtual public void OnDrop(PointerEventData eventData)
    {
        playerManager.OnTileDrop();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("T Begin Drag");
    }
}
