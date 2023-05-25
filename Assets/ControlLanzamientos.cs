using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlLanzamientos : MonoBehaviour
{

    public GameObject bola;
    public Rigidbody2D pivote;

    private Camera camara;
    private Rigidbody2D bolaRigidbody;
    private SpringJoint2D bolaSprintJoint;
    
    

    void Start()
    {
        camara = Camera.main;

        bolaRigidbody = bola.GetComponent<Rigidbody2D>();

        bolaSprintJoint = bola.GetComponent<SpringJoint2D>();

        bolaSprintJoint.connectedBody = pivote;


    }

    void Update()
    {

        if(bolaRigidbody == null) { return; }

        if(!Touchscreen.current.primaryTouch.press.isPressed)
        {
            return;
        }
        Vector2 posicionTocar = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 posicionMundo = camara.ScreenToViewportPoint(posicionTocar);             
    }
}
