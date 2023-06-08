using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using MyNamespace;

public class pajarolanzamiento : MonoBehaviour
{
    public GameObject bolaPrefab;
    public Rigidbody2D pivote;
    public float tiempoQuitarSprintJoin;
    public float tiempoFinJuego;

    public int vidasExtras = 3; // Número de vidas extra para el pájaro
    public float respawnDelay = 2f; // Retraso antes de que el pájaro vuelva a aparecer

    private Camera camara;
    private List<GameObject> pajaros; // Lista para almacenar los pájaros creados
    private Rigidbody2D bolaRigidbody;
    private SpringJoint2D bolaSprintJoint;
    private bool estaArrastrando;
    private bool juegoDetenido = false; // Variable para controlar si el juego está detenido

    private Vector2 initialPosition; // Posición inicial del pájaro

    private controldatos datosJuego;
    private interfazController controladorInterfaz;
    private bool haGanado = false; // Variable para verificar si el jugador ha ganado el juego

    private int cerdosRestantes; // Número de cerdos restantes en la escena

    private void Start()
    {
        camara = Camera.main;
        bolaRigidbody = GetComponent<Rigidbody2D>();
        bolaSprintJoint = GetComponent<SpringJoint2D>();
        datosJuego = GameObject.Find("datosJuego").GetComponent<controldatos>();
        controladorInterfaz = GameObject.Find("controladorVictoria").GetComponent<interfazController>();
        bolaSprintJoint.connectedBody = pivote;

        initialPosition = transform.position; // Almacenar la posición inicial del pájaro

        pajaros = new List<GameObject>();
        pajaros.Add(gameObject); // Agregar el pájaro inicial a la lista

        // Obtener el número inicial de cerdos en la escena
        cerdosRestantes = GameObject.FindGameObjectsWithTag("cerdo").Length;
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
        bolaRigidbody.isKinematic = false;
        bolaRigidbody = null;

        bolaSprintJoint.enabled = true; // Reactivar el SpringJoint

        Invoke(nameof(QuitarSprintJoin), tiempoQuitarSprintJoin);
    }

    private void QuitarSprintJoin()
    {
        bolaSprintJoint.enabled = false;
        bolaSprintJoint = null;

        vidasExtras--; // Reducir el número de vidas extra

        if (vidasExtras <= 0 && !haGanado)
        {
            if (datosJuego.Puntuacion >= datosJuego.puntosPorColision * cerdosRestantes)
            {
                haGanado = true;
                FinJuego(true); // Llamar a la función FinJuego con victoria si ha tocado todos los cerdos y no le quedan vidas extras
            }
            else
            {
                FinJuego(false); // Llamar a la función FinJuego con derrota si no ha ganado y no ha tocado todos los cerdos
            }
        }
        else
        {
            Invoke(nameof(RespawnBola), respawnDelay);
        }

        controladorInterfaz.PerderVida(); // Actualizar la interfaz para mostrar la pérdida de una vida
    }

    private void RespawnBola()
    {
        GameObject bola = Instantiate(bolaPrefab, initialPosition, Quaternion.identity);
        Rigidbody2D nuevaBolaRigidbody = bola.GetComponent<Rigidbody2D>();
        SpringJoint2D nuevaBolaSprintJoint = bola.GetComponent<SpringJoint2D>();
        nuevaBolaRigidbody.isKinematic = true;
        nuevaBolaSprintJoint.connectedBody = pivote;

        pajaros.Add(bola); // Agregar el nuevo pájaro a la lista
    }

   private void FinJuego(bool victoria)
{
    juegoDetenido = true; // Detener el juego

    if (victoria)
    {
        controladorInterfaz.MostrarPantallaVictoria(); // Mostrar la interfaz de victoria
        FindObjectOfType<nextlevel>().DesbloquearNivel(); // Desbloquear el siguiente nivel
    }
    else
    {
        controladorInterfaz.MostrarPantallaDerrota(); // Mostrar la interfaz de derrota
    }

    Time.timeScale = 0; // Detener la escala de tiempo del juego
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("cerdo"))
        {
            datosJuego.Puntuacion += datosJuego.puntosPorColision; // Incrementar la puntuación utilizando la propiedad Puntuacion
            cerdosRestantes--; // Reducir el número de cerdos restantes

            if (!haGanado && cerdosRestantes <= 0)
            {
                haGanado = true;
                FinJuego(true); // Llamar a la función FinJuego con victoria si ha tocado todos los cerdos
            }
        }
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Cargar de nuevo la escena actual
        Time.timeScale = 1; // Restablecer la escala de tiempo del juego
        juegoDetenido = false;
        GetComponent<pajarolanzamiento>().enabled = true;
    }

    public void VolverAlMenuPrincipal()
    {
        SceneManager.LoadScene("Titulo"); // Cargar la escena del menú principal
        Time.timeScale = 1; // Restablecer la escala de tiempo del juego
    }
}
