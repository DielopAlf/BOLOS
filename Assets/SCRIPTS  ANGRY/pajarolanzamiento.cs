using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class pajarolanzamiento : MonoBehaviour
{
    public GameObject pajaroPrefab;
    public Rigidbody2D pivote;
    public float tiempoQuitarSprintJoin;
    public float tiempoFinJuego;
    public float tiempoEspera = 2f;
    public float tiempoAparicionPajaro = 2f; // Tiempo de espera para que aparezca un nuevo pájaro después de lanzar uno

    private Vector3 posicionInicial;

    public float tamanoInicial;
    public float tamanoFinal;

    private Camera camara;
    private Rigidbody2D bolaRigidbody;
    private SpringJoint2D bolaSprintJoint;

    private bool estaArrastrando;
    private bool puedeCrearNuevoPajaro = true; // Flag para permitir la creación de un nuevo pájaro
    private bool pajaroLanzado = false; // Flag para controlar si se ha lanzado un pájaro

    public float velocidadNormal;
    public float velocidadMinimaParaRomper = 1f;

    private int vidasExtras = 3; // Número de vidas extras

    void Start()
    {
        camara = Camera.main;

        CrearNuevoPajaro();
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

        // Verificar si el botón de clic está siendo presionado y evitar crear nuevos pájaros
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            puedeCrearNuevoPajaro = false;
        }
    }

    private void CrearNuevoPajaro()
    {
        if (!puedeCrearNuevoPajaro || vidasExtras <= 0)
        {
            return;
        }

        vidasExtras--; // Reduce el número de vidas extras

        GameObject nuevoPajaro = Instantiate(pajaroPrefab, transform.position, Quaternion.identity);
        nuevoPajaro.transform.localScale = new Vector3(tamanoInicial, tamanoInicial, 1f);

        bolaRigidbody = nuevoPajaro.GetComponent<Rigidbody2D>();
        bolaSprintJoint = nuevoPajaro.GetComponent<SpringJoint2D>();
        bolaSprintJoint.connectedBody = pivote;

        posicionInicial = transform.position;

        puedeCrearNuevoPajaro = false; // Evita la creación de nuevos pájaros hasta que se lance el actual
        pajaroLanzado = false;
    }

    private void LanzarBola()
    {
        if (pajaroLanzado) // Verificar si ya se ha lanzado un pájaro
        {
            return;
        }

        bolaRigidbody.isKinematic = false;

        Invoke(nameof(QuitarSprintJoin), tiempoQuitarSprintJoin);
        pajaroLanzado = true; // Marcar que se ha lanzado un pájaro
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

        puedeCrearNuevoPajaro = true; // Reactivar la bandera para permitir la creación de un nuevo pájaro
        pajaroLanzado = false; // Reiniciar el flag de lanzamiento del pájaro

        if (vidasExtras > 0)
        {
            Invoke(nameof(CrearNuevoPajaro), tiempoEspera);
        }
    }

    private void FinJuego()
    {
        SceneManager.LoadScene("FinNivel");
        Debug.Log("Fin Juego");
    }
}
