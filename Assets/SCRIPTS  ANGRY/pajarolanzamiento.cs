using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class pajarolanzamiento : MonoBehaviour
{
    private Rigidbody2D pivote;
    public float tiempoQuitarSprintJoin;
    public float tiempoFinJuego;
    public float tiempoEsperaPantallaVictoriaDerrota;

    public int vidasExtras = 2;
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

    public enum habilidadespecial { ninguno, crecer };

    public habilidadespecial especial;

    private float tiempoUltimoPajaroQuieto; // Variable para el tiempo que lleva el último pájaro quieto

    public AudioClip tirarClip;
    public AudioClip lanzamientoClip;
    public AudioClip colisionClip;
    public AudioClip victoriaClip;
    public AudioClip derrotaClip;
    public AudioSource audioSource;



  //  public AudioClip musicaClip;
    public AudioSource musicsource;


    private void Start()
    {
        if (GameObject.FindGameObjectsWithTag("pivote").Length > 0)
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

        audioSource = GetComponent<AudioSource>();


        musicsource = GameObject.Find("controladorsonido").GetComponent<AudioSource>();


        initialPosition = transform.position;
        escalaInicial = transform.localScale;

        pajaros = new List<GameObject>();
        pajaros.Add(gameObject);

        timerToFinish = timeToFinish;

        cerdosRestantes = GameObject.FindGameObjectsWithTag("cerdo").Length;

    }

    private void Update()
    {
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
                    audioSource.PlayOneShot(tirarClip);
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
                audioSource.PlayOneShot(lanzamientoClip);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pajaro")
        {
           
            audioSource.PlayOneShot(colisionClip);
        }
        else if (collision.gameObject.tag == "Choque")
        {
            
            audioSource.PlayOneShot(colisionClip);
        }
        else if (collision.gameObject.tag == "pared")
        {

            audioSource.PlayOneShot(colisionClip);
        }
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
                    if (datosJuego.Puntuacion > 0)
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

        
    }

    public void ataqueespecial()
    {
        switch (especial)
        {
            case habilidadespecial.ninguno:
                break;
            case habilidadespecial.crecer:
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
        controladorInterfaz.PerderVida();
        GameObject bola = null;

        if (datosJuego.VidasExtras == 1)
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
            tiempoUltimoPajaroQuieto = Time.time;
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

            

            audioSource.PlayOneShot(victoriaClip); // Reproducir sonido de victoria
            musicsource.Stop();
        }
        else
        {
            MostrarPantallaVictoriaDerrota(false);

           

            audioSource.PlayOneShot(derrotaClip); // Reproducir sonido de derrota
            musicsource.Stop();
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
                controladorInterfaz.MostrarPantallaVictoria();
            }
        }
        else
        {
            controladorInterfaz.MostrarPantallaDerrota();
           
        }
    }

    private void LateUpdate()
    {
        if (ultimoPajaroLanzado && Time.time - tiempoUltimoPajaroQuieto >= tiempoFinJuego)
        {
            // FinJuegoSinVictoria();
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
