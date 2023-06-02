using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContadorPajaros : MonoBehaviour
{
    public TextMeshProUGUI PuntosTxt;
    private controldatos datosJuego;

    public int puntosPorColision = 20; // Valor de puntos a incrementar por cada colisi�n

    private int puntuacion;

    public int Puntuacion
    {
        get { return puntuacion; }
        set
        {
            puntuacion = value;
            PUNTOSTxt(puntuacion);
        }
    }

    private void Start()
    {
        datosJuego = GameObject.Find("datosJuego").GetComponent<controldatos>();
        Puntuacion = datosJuego.Puntuacion;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("cerdo"))
        {
            Debug.Log("Contar");
            Puntuacion += puntosPorColision; // Incrementa la puntuaci�n utilizando la propiedad Puntuacion
            datosJuego.Puntuacion = Puntuacion; // Actualiza la puntuaci�n en el script 'controldatos'

            

        }
        

    }

    public void PUNTOSTxt(int puntuacion)
    {
        PuntosTxt.text = "Puntuacion: " + puntuacion.ToString();
    }
}
