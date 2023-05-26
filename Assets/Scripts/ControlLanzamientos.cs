using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class ControlLanzamientos : MonoBehaviour
{

    public GameObject bola;
    public Rigidbody2D pivote;
    public float tiempoQuitarSprintJoin;
    public float tiempoFinJuego;




    private Camera camara;
    private Rigidbody2D bolaRigidbody;
    private SpringJoint2D bolaSprintJoint;

    private bool estaArrastrando;


    void Start()
    {
        camara = Camera.main;

        bolaRigidbody = bola.GetComponent<Rigidbody2D>();

        bolaSprintJoint = bola.GetComponent<SpringJoint2D>();

        bolaSprintJoint.connectedBody = pivote;
}





        void Update()
        {

            if (bolaRigidbody == null) { return; }

            if (!Touchscreen.current.primaryTouch.press.isPressed)
            {
                if(estaArrastrando)

                {
                    LanzarBola();
                    
                }

                estaArrastrando = false;

                return;
            }

            estaArrastrando = true;

            bolaRigidbody.isKinematic = true;

            Vector2 posicionTocar = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 posicionMundo = camara.ScreenToWorldPoint(posicionTocar);
            bolaRigidbody.position = posicionMundo;
        Debug.Log(posicionTocar+ " " + posicionMundo);
        }
        
        
    private void LanzarBola()
    {

        bolaRigidbody.isKinematic = false;
        bolaRigidbody = null;

        Invoke(nameof(QuitarSprintJoin), tiempoQuitarSprintJoin);
    }
    private void QuitarSprintJoin()
    {

        bolaSprintJoint.enabled = false;
        bolaSprintJoint = null;

        Invoke(nameof(FinJuego), tiempoFinJuego);
       
    }

    private void FinJuego()
    {

        SceneManager.LoadScene("FinNivel");

        Debug.Log("Fin Juego");
    }

}
