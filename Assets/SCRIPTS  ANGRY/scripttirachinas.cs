using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class scripttirachinas : MonoBehaviour
{
    public GameObject pajaro;
    public Rigidbody2D pivote;
    public float tiempoQuitarSprintJoin;
    public float tiempoFinJuego;

    private Vector3 posicionInicial;

    public float tamanoInicial;
    public float tamanoFinal;

    private Camera camara;
    private Rigidbody2D bolaRigidbody;
    private SpringJoint2D bolaSprintJoint;

    private bool estaArrastrando;

    public float velocidadNormal;
    public float velocidadMinimaParaRomper = 1f;





    void Start()
    {
        camara = Camera.main;

        bolaRigidbody = pajaro.GetComponent<Rigidbody2D>();

        bolaSprintJoint = pajaro.GetComponent<SpringJoint2D>();

        bolaSprintJoint.connectedBody = pivote;

        Transform pajaroTransform = pajaro.transform;
        pajaroTransform.localScale = new Vector3(tamanoInicial, tamanoInicial, 1f);
    }

    void Update()
    {
        if (bolaRigidbody == null) { return; }

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (estaArrastrando)
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
        Debug.Log(posicionTocar + " " + posicionMundo);
    }

    private void LanzarBola()
    {
        bolaRigidbody.isKinematic = false;
        bolaRigidbody = null;
        

        Invoke(nameof(QuitarSprintJoin), tiempoQuitarSprintJoin);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("pared"))
        {
            if (velocidadNormal >= velocidadMinimaParaRomper)
            {
                Destroy(gameObject);
            }

        }
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
