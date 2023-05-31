using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scripttirachinas : MonoBehaviour
{

    public LineRenderer[] lineRenderers;
    public Transform[] stripPosition;
    public Transform center;
    public Transform idlePosition;

    bool isMouseDown;


    private void Start()
    {
        lineRenderers[0].positionCount=2;
        lineRenderers[1].positionCount=2;
        lineRenderers[0].SetPosition(0, stripPosition[0].position);
        lineRenderers[0].SetPosition(0, stripPosition[1].position);

    }
    void Update()
    {
        if (isMouseDown)
        {

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z =10;

            SetStrips(mousePosition);
        }
        else
        {
            ResetStrip();

        }
    }
    private void OnMouseDown()
    {
        isMouseDown =true;
    }
    private void OnMouseUp()
    {
        isMouseDown =false;
    }


    void ResetStrip()
    {
        SetStrips(idlePosition.position);
    }

     void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);
    }
    

}
