using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using MyNamespace;
//using static LeanTween;

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


    public nextlevel nivelSiguiente;

    private static interfazController controladorinterfaz;

    private void Start()
    {
        camara = Camera.main;
        bolaRigidbody = GetComponent<Rigidbody2D>();
        bolaSprintJoint = GetComponent<SpringJoint2D>();
        datosJuego = GameObject.Find("datosJuego").GetComponent<controldatos>();
        bolaSprintJoint.connectedBody = pivote;

        controladorinterfaz = GameObject.Find("controladorVictoria").GetComponent<interfazController>();



        initialPosition = transform.position; // Almacenar la posición inicial del pájaro

        pajaros = new List<GameObject>();
        pajaros.Add(gameObject); // Agregar el pájaro inicial a la lista
    }

    private void Update()
    {
        if (bolaRigidbody == null || juegoDetenido) { 
            Debug.Log("null");
            return; }

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

       


        if (vidasExtras <= 0)
        {
            Invoke(nameof(FinJuego), tiempoFinJuego);
        }
        else
        {
            Invoke(nameof(RespawnBola), respawnDelay);
        }
        controladorinterfaz.perdervida();
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
    
    private void FinJuego()
    {
        // cqargar canvas victoreia 
        // esta desactivadoi, hay que actiuvarlo

        Debug.Log("Fin Juego");
    }
    void GuardarDatos(bool hasWon)
{
   


    


    // Desactivar el control del jugador
    //GetComponent<BallBehaviour>().enabled = false;
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("cerdo"))
        {
            Debug.Log("Contar");
            datosJuego.Puntuacion += datosJuego.puntosPorColision; // Incrementa la puntuación utilizando la propiedad Puntuacion
        }


    }

    public void ReiniciarNivel()
{
        // Cargar de nuevo la escena actual
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    Time.timeScale = 1; // Restablecer la escala de tiempo del juego
    juegoDetenido = false;
    GetComponent<pajarolanzamiento>().enabled = true;
}
public void VolverAlMenuPrincipal()
{
    // Cargar la escena del menú principal
    SceneManager.LoadScene("MenuInicial");
    Time.timeScale = 1; // Restablecer la escala de tiempo del juego
}
}


