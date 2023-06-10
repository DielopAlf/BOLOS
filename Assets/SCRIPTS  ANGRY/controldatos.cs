using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class controldatos : MonoBehaviour
{
    private int puntuacion;

    public TextMeshProUGUI PuntosTxt;

    public int puntosPorColision = 20; // Valor de puntos a incrementar por cada colisi�n
    public int numeroDeCerdos; // N�mero total de cerdos en el nivel

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
        PuntosTxt.text = "Puntuaci�n: " + puntuacion.ToString();
    }

    private void Awake()
    {
        int numInstancias = FindObjectsOfType<controldatos>().Length;

        Debug.Log("DATOS+" + numInstancias);

        if (numInstancias != 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
