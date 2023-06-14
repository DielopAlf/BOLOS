using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class controldatos : MonoBehaviour
{
    private int puntuacion;

    public TextMeshProUGUI PuntosTxt;

    public int puntosPorColision = 20; // Valor de puntos a incrementar por cada colisión


    public static controldatos instance;

    public int Puntuacion
    {
        get { return puntuacion; }
        set
        {
            puntuacion = value;
            PUNTOSTxt(puntuacion);
        }
    }

    public void PUNTOSTxt(int puntuacion)
    {
        PuntosTxt.text = "Puntuación: " + puntuacion.ToString();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}