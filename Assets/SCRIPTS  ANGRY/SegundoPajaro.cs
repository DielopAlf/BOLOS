using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SegundoPajaro : MonoBehaviour
{
    public GameObject bolaPrefab;
    public Rigidbody2D pivote;
    public float tiempoQuitarSprintJoin;
    public float respawnDelay = 2f;

    private Camera camara;
    private Rigidbody2D bolaRigidbody;
    private SpringJoint2D bolaSprintJoint;
    private bool estaArrastrando;
    private bool juegoDetenido = false;

    private Vector2 initialPosition;
    private Vector3 escalaInicial;

    private int vidasExtras = 3;
    private bool sehalanzado = false;
    private bool hatocado = false;
    public float factorEscala = 1.2f;
    public float escalaMaxima = 2.0f;

    private bool puedeCrecer = true;

    private void Start()
    {
        sehalanzado = false;
        hatocado = false;

        camara = Camera.main;
        bolaRigidbody = GetComponent<Rigidbody2D>();
        bolaSprintJoint = GetComponent<SpringJoint2D>();

        initialPosition = transform.position;
        escalaInicial = transform.localScale;
    }

    private void Update()
    {
        if (bolaRigidbody == null || juegoDetenido)
            return;

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
    }

    private void LanzarBola()
    {
        sehalanzado = true;

        bolaRigidbody.isKinematic = false;
        bolaRigidbody = null;

        bolaSprintJoint.enabled = true;

        Invoke(nameof(QuitarSprintJoin), tiempoQuitarSprintJoin);
    }

    private void QuitarSprintJoin()
    {
        bolaSprintJoint.enabled = false;
        bolaSprintJoint = null;

        vidasExtras--;
        if (vidasExtras <= 0)
        {
            Invoke(nameof(RespawnBola), respawnDelay);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void RespawnBola()
    {
        GameObject bola = Instantiate(bolaPrefab, initialPosition, Quaternion.identity);
        Rigidbody2D nuevaBolaRigidbody = bola.GetComponent<Rigidbody2D>();
        SpringJoint2D nuevaBolaSprintJoint = bola.GetComponent<SpringJoint2D>();
        nuevaBolaRigidbody.isKinematic = true;
        nuevaBolaSprintJoint.connectedBody = pivote;
        bola.transform.localScale = escalaInicial;
    }
}
