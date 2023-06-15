using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemigocontrol : MonoBehaviour
{
    public int disparosNecesarios = 1;
    private int disparosRecibidos = 0;

    private interfazController interfazController;
    private ControlDatosjuego datosJuego;
    public AudioClip choqueClip;
    private Renderer renderer;

    private void Start()
    {
        interfazController = FindObjectOfType<interfazController>();
        datosJuego = FindObjectOfType<ControlDatosjuego>();
        renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pajaro"))
        {
            disparosRecibidos++;

            if (disparosRecibidos >= disparosNecesarios)
            {
                AudioSource.PlayClipAtPoint(choqueClip, transform.position);

                datosJuego.numeroDeCerdos--;
                datosJuego.Puntuacion += datosJuego.puntosPorColision;
                interfazController.ActualizarPuntuacionActual(datosJuego.Puntuacion);

                if (datosJuego.CheckWin())
                {
                    StartCoroutine(ProcesarVictoria(other.gameObject));
                }
                else
                {
                    StartCoroutine(DestruirObjeto());
                }
            }
        }
    }

    private IEnumerator ProcesarVictoria(GameObject pajaro)
    {
        if (pajaro.GetComponent<pajarolanzamiento>().enabled == false)
        {
            pajaro.GetComponent<pajarolanzamiento>().enabled = true;
            pajaro.GetComponent<pajarolanzamiento>().FinJuego(true);
            pajaro.GetComponent<pajarolanzamiento>().enabled = false;
        }
        else
        {
            pajaro.GetComponent<pajarolanzamiento>().FinJuego(true);
        }

        yield return new WaitForSeconds(0.5f);

        // Mostrar pantalla de victoria
        // datosJuego.Ganado = true;
        // if (interfazController != null)
        // {
        //     interfazController.MostrarPantallaVictoria();
        // }
    }

    private IEnumerator DestruirObjeto()
    {
        // Desactivar el renderizado
        renderer.enabled = false;

        // Esperar un breve retardo
        yield return new WaitForSeconds(0.1f);

        // Destruir completamente el objeto
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (disparosRecibidos < disparosNecesarios)
        {
            // datosJuego.Ganado = false;
            // if (interfazController != null)
            // {
            //     interfazController.MostrarPantallaDerrota();
            // }
        }
    }
}
