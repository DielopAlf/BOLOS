using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemigocontrol : MonoBehaviour
{
    public int disparosNecesarios = 1;

    private int disparosRecibidos = 0;

    private interfazController interfazController; // Referencia al script "interfazController"
    private ControlDatosjuego datosJuego; // Referencia al script "ControlDatosjuego"
    public AudioClip golpeSound;
    private AudioSource audioSource;

    private void Start()
    {
        interfazController = FindObjectOfType<interfazController>(); // Obtener una referencia al script "interfazController"
        datosJuego = FindObjectOfType<ControlDatosjuego>(); // Obtener una referencia al script "ControlDatosjuego"
        audioSource = GetComponent<AudioSource>(); // Obtener el componente AudioSource
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pajaro"))
        {
            disparosRecibidos++;

            if (disparosRecibidos >= disparosNecesarios)
            {
                Destroy(gameObject);

                datosJuego.Ganado = true;

                if (interfazController != null)
                {
                    interfazController.MostrarPantallaVictoria();
                }
            }
            else
            {
                // Reproducir el sonido de golpe
                audioSource.PlayOneShot(golpeSound);
            }
        }
    }

    private void OnDestroy()
    {
        if (disparosRecibidos < disparosNecesarios)
        {
            datosJuego.Ganado = false;

            if (interfazController != null)
            {
                interfazController.MostrarPantallaDerrota();
            }
        }
    }
}
