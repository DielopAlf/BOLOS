using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class pajarolanzamiento : MonoBehaviour
{
    public GameObject bolaPrefab;
    public Rigidbody2D pivote;
    public float tiempoQuitarSprintJoin;
    public float tiempoFinJuego;
    public float tiempoEsperaPantallaVictoriaDerrota;

    public int vidasExtras = 3;
    public float respawnDelay = 2f;

    private Camera camara;
    private List<GameObject> pajaros;
    private Rigidbody2D bolaRigidbody;
    private SpringJoint2D bolaSprintJoint;
    private bool estaArrastrando;
    private bool juegoDetenido = false;

    private Vector2 initialPosition;
    private Vector3 escalaInicial;

    private controldatos datosJuego;
    private interfazController controladorInterfaz;
    private bool haGanado = false;

    public int nivel;

    private int cerdosRestantes;

    private bool sehalanzado = false;
    private bool hatocado = false;
    public float factorEscala = 1.2f;
    public float escalaMaxima = 2.0f;

    private bool puedeCrecer = true;
    private bool ultimoPajaroLanzado = false;

    private float tiempoUltimoPajaroQuieto; // Variable para el tiempo que lleva el último pájaro quieto

    private void Start()
    {
        sehalanzado = false;
        hatocado = false;

        camara = Camera.main;
        bolaRigidbody = GetComponent<Rigidbody2D>();
        bolaSprintJoint = GetComponent<SpringJoint2D>();
        datosJuego = FindObjectOfType<controldatos>();
        controladorInterfaz = FindObjectOfType<interfazController>();
        //bolaSprintJoint.connectedBody = pivote;

        initialPosition = transform.position;
        escalaInicial = transform.localScale;

        pajaros = new List<GameObject>();
        pajaros.Add(gameObject);

        cerdosRestantes = GameObject.FindGameObjectsWithTag("cerdo").Length;
    }

    private void Update()
    {
        if (sehalanzado == true)
        {
            if (Touchscreen.current.primaryTouch.press.isPressed && hatocado == false && puedeCrecer)
            {
                hatocado = true;

                if (!ultimoPajaroLanzado)
                {
                    // Aumentar el tamaño del pájaro si no ha alcanzado el tamaño máximo
                    if (transform.localScale.x < escalaMaxima && transform.localScale.y < escalaMaxima)
                    {
                        Vector3 nuevaEscala = transform.localScale * factorEscala;
                        transform.localScale = nuevaEscala;
                    }
                }
            }
        }

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

        if (vidasExtras <= 0 && !haGanado)
        {
            if (datosJuego.Puntuacion >= datosJuego.puntosPorColision * cerdosRestantes)
            {
                haGanado = true;
                FinJuego(true);
            }
            else
            {
                FinJuego(false);
            }
        }
        else
        {
            Invoke(nameof(RespawnBola), respawnDelay);
        }

        controladorInterfaz.PerderVida();
    }

    private void RespawnBola()
    {
        GameObject bola = Instantiate(bolaPrefab, initialPosition, Quaternion.identity);
        Rigidbody2D nuevaBolaRigidbody = bola.GetComponent<Rigidbody2D>();
        SpringJoint2D nuevaBolaSprintJoint = bola.GetComponent<SpringJoint2D>();
        nuevaBolaRigidbody.isKinematic = true;
        nuevaBolaSprintJoint.connectedBody = pivote;
        bola.transform.localScale = escalaInicial;

        pajaros.Add(bola);
        puedeCrecer = false;

        if (vidasExtras <= 0 && cerdosRestantes > 0)
        {
            ultimoPajaroLanzado = true;
            tiempoUltimoPajaroQuieto = Time.time; // Guardar el tiempo en el que el último pájaro se queda quieto
        }
    }

    private void FinJuego(bool victoria)
    {
        juegoDetenido = true;

        if (victoria)
        {
            PlayerPrefs.SetInt("NivelSuperado" + nivel.ToString(), 1);
            PlayerPrefs.Save();

            StartCoroutine(MostrarPantallaVictoriaDerrota(true, tiempoEsperaPantallaVictoriaDerrota));
        }
        else
        {
            StartCoroutine(MostrarPantallaVictoriaDerrota(false, tiempoEsperaPantallaVictoriaDerrota));
        }

        Time.timeScale = 0;
    }

    private void FinJuegoSinVictoria()
    {
        FinJuego(false);
    }

    private IEnumerator MostrarPantallaVictoriaDerrota(bool victoria, float tiempoEspera)
    {
        yield return new WaitForSecondsRealtime(tiempoEspera);

        if (victoria)
        {
            if (ultimoPajaroLanzado)
            {
                // Mostrar pantalla correspondiente al último pájaro
            }
            else
            {
                controladorInterfaz.MostrarPantallaVictoria();
            }
        }
        else
        {
            controladorInterfaz.MostrarPantallaDerrota();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("cerdo"))
        {
            datosJuego.Puntuacion += datosJuego.puntosPorColision;
            cerdosRestantes--;

            if (!haGanado && cerdosRestantes <= 0)
            {
                haGanado = true;
                FinJuego(true);
            }
        }
    }

    private void LateUpdate()
    {
        // Comprobar si el último pájaro lanzado está quieto y mostrar la pantalla de derrota si se cumple el tiempo límite
        if (ultimoPajaroLanzado && Time.time - tiempoUltimoPajaroQuieto >= tiempoFinJuego)
        {
            FinJuegoSinVictoria();
        }
    }
    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        juegoDetenido = false;
        GetComponent<pajarolanzamiento>().enabled = true;
    }

    public void VolverAlMenuPrincipal()
    {
        SceneManager.LoadScene("Titulo");
        Time.timeScale = 1;
    }

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        System.IO.Directory.Delete(Application.persistentDataPath, true);
        Debug.Log("Todos los datos del juego han sido eliminados.");
    }
}
