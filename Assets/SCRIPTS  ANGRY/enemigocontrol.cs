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

    private void Start()
    {
        interfazController = FindObjectOfType<interfazController>(); // Obtener una referencia al script "interfazController"
        datosJuego = FindObjectOfType<ControlDatosjuego>(); // Obtener una referencia al script "ControlDatosjuego"
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pajaro"))
        {
            disparosRecibidos++;

            if (disparosRecibidos >= disparosNecesarios)
            {
                Destroy(gameObject);
                datosJuego.numeroDeCerdos--;
                datosJuego.Puntuacion += datosJuego.puntosPorColision;
                interfazController.ActualizarPuntuacionActual(datosJuego.Puntuacion);
                if (datosJuego.CheckWin())
                {
                    if (other.gameObject.GetComponent<pajarolanzamiento>().enabled == false)
                    {
                        other.gameObject.GetComponent<pajarolanzamiento>().enabled = true;
                        other.gameObject.GetComponent<pajarolanzamiento>().FinJuego(true);
                        other.gameObject.GetComponent<pajarolanzamiento>().enabled = false;
                    }
                    else
                    {
                        other.gameObject.GetComponent<pajarolanzamiento>().FinJuego(true);
                    }
                }


                /*datosJuego.Ganado = true;
              

                if (interfazController != null)
                {
                    interfazController.MostrarPantallaVictoria();
                }*/
            }
        }
    }

    private void OnDestroy()
    {
        if (disparosRecibidos < disparosNecesarios)
        {
            /*datosJuego.Ganado = false;

            if (interfazController != null)
            {
                interfazController.MostrarPantallaDerrota();
            }*/
        }
    }
}