using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class pajarolanzamiento : MonoBehaviour
{
    
    private Rigidbody2D pivote;
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

    private ControlDatosjuego datosJuego;
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


    public GameObject[] pajarosegundo;


    public float maxSpringRange = 1.35f;
    public Vector2 camInitialPos = new Vector2(0, 0);
    public float camInitialSize = 5;
    public float camSiguiendoPajaroZoom = 2.35f;
    private float timerToFinish;
    public float timeToFinish = 5;

    public enum habilidadespecial { ninguno,crecer };

    public habilidadespecial especial;

    private float tiempoUltimoPajaroQuieto; // Variable para el tiempo que lleva el último pájaro quieto

    private void Start()
    {
        if (GameObject.FindGameObjectsWithTag("pivote").Length>0)
        {
            pivote = GameObject.FindGameObjectsWithTag("pivote")[0].GetComponent<Rigidbody2D>();

        }
       

        sehalanzado = false;
        hatocado = false;

        datosJuego = ControlDatosjuego.instance;

        camara = Camera.main;
        bolaRigidbody = GetComponent<Rigidbody2D>();
        bolaSprintJoint = GetComponent<SpringJoint2D>();

        controladorInterfaz = FindObjectOfType<interfazController>();
        //bolaSprintJoint.connectedBody = pivote;

        initialPosition = transform.position;
        escalaInicial = transform.localScale;

        pajaros = new List<GameObject>();
        pajaros.Add(gameObject);

        timerToFinish = timeToFinish;

        cerdosRestantes = GameObject.FindGameObjectsWithTag("cerdo").Length;
    }

    private void Update()
    {
        Debug.Log("hola");
        if (sehalanzado == true)
        {
            CheckFullStop();
            CamaraSiguePajaro();
            if (Touchscreen.current.primaryTouch.press.isPressed && hatocado == false && puedeCrecer)
            {
                hatocado = true;
                ataqueespecial();
                if (!ultimoPajaroLanzado)
                {
                    
                }
            }

        }

        if (bolaRigidbody == null || juegoDetenido)
            return;

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            Debug.Log("prueba");
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

        Vector3 distance = posicionMundo - pivote.transform.position;
        distance.z = 0;



        if (distance.magnitude > maxSpringRange)
        {
            Vector3 dis = new Vector3(distance.x, distance.y, 0f);
            dis = dis.normalized * maxSpringRange;

            distance = distance.normalized * maxSpringRange;

        }
        bolaRigidbody.position = distance + pivote.transform.position;


    }

    private void LanzarBola()
    {
        sehalanzado = true;

        bolaRigidbody.isKinematic = false;
        bolaRigidbody = null;

        bolaSprintJoint.enabled = true;

        Invoke(nameof(QuitarSprintJoin), tiempoQuitarSprintJoin);
    }

    private void CamaraSiguePajaro()
    {
        if (sehalanzado && camara != null)
        {
            camara.orthographicSize = camSiguiendoPajaroZoom;
            camara.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -12.1f);
        }
    }

    private void CamaraToNormal()
    {
        if (camara != null)
        {
            camara.orthographicSize = camInitialSize;
            camara.transform.position = new Vector3(camInitialPos.x, camInitialPos.y, -12.1f);
            camara = null;
        }

    }

    private void CheckFullStop()
    {
        timerToFinish -= Time.deltaTime;
        if (timerToFinish <= 0f)
        {
            CamaraToNormal();
            if (!haGanado)
            {
                if (datosJuego.VidasExtras <= 0)
                {
                    //si quiero que aparezca cuando los mate a todos
                    //FinJuego(false);

                    // si quiero    ue aparezca con matar a uno
                    if (datosJuego.Puntuacion>0)
                    {
                        FinJuego(true);

                    }
                    else
                    {

                        FinJuego(false);

                    }
                }
                else
                {
                    RespawnBola();
                }
            }

            this.enabled = false;

        }
    }

    private void QuitarSprintJoin()
    {
        bolaSprintJoint.enabled = false;
        bolaSprintJoint = null;

        datosJuego.VidasExtras--;

        controladorInterfaz.PerderVida();
    }

    public void ataqueespecial()
    {

        switch(especial)
        {
            case habilidadespecial.ninguno:
                break;
            case habilidadespecial.crecer:
                // Aumentar el tamaño del pájaro si no ha alcanzado el tamaño máximo
                if (transform.localScale.x < escalaMaxima && transform.localScale.y < escalaMaxima)
                {
                    Vector3 nuevaEscala = transform.localScale * factorEscala;
                    transform.localScale = nuevaEscala;
                }
                break;

        }


    }

    private void RespawnBola()
    {
        GameObject bola= null;


        if (datosJuego.VidasExtras==1)
        {

             bola = Instantiate(pajarosegundo[1], initialPosition, Quaternion.identity);

        }
        else
        {

             bola = Instantiate(pajarosegundo[0], initialPosition, Quaternion.identity);
        }

     
        Rigidbody2D nuevaBolaRigidbody = bola.GetComponent<Rigidbody2D>();
        SpringJoint2D nuevaBolaSprintJoint = bola.GetComponent<SpringJoint2D>();
        nuevaBolaRigidbody.isKinematic = true;
        nuevaBolaSprintJoint.connectedBody = pivote;
        bola.transform.localScale = escalaInicial;
        
        pajaros.Add(bola);
        puedeCrecer = false;

        if (datosJuego.VidasExtras <= 0 && datosJuego.numeroDeCerdos > 0)
        {
            ultimoPajaroLanzado = true;
            tiempoUltimoPajaroQuieto = Time.time; // Guardar el tiempo en el que el último pájaro se queda quieto
        }
    }

    public void FinJuego(bool victoria)
    {
        juegoDetenido = true;
        datosJuego.SaveRecord();

        if (victoria)
        {
            haGanado = true;
            PlayerPrefs.SetInt("NivelSuperado" + nivel.ToString(), 1);
            PlayerPrefs.Save();

            MostrarPantallaVictoriaDerrota(true);
        }
        else
        {
            MostrarPantallaVictoriaDerrota(false);
        }

        Time.timeScale = 0;
    }


    private void MostrarPantallaVictoriaDerrota(bool victoria)
    {

        if (victoria)
        {
            if (ultimoPajaroLanzado)
            {
                // Mostrar pantalla correspondiente al último pájaro
            }
            else
            {
                //controladorInterfaz.MostrarPantallaVictoria();
            }
            controladorInterfaz.MostrarPantallaVictoria();
        }
        else
        {
            controladorInterfaz.MostrarPantallaDerrota();
        }
    }


    private void LateUpdate()
    {
        // Comprobar si el último pájaro lanzado está quieto y mostrar la pantalla de derrota si se cumple el tiempo límite
        if (ultimoPajaroLanzado && Time.time - tiempoUltimoPajaroQuieto >= tiempoFinJuego)
        {
            //FinJuegoSinVictoria();
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