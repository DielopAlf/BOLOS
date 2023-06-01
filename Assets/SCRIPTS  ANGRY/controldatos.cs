using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controldatos : MonoBehaviour
{
    private int puntuacion;

    public int Puntuacion
    {
        get => puntuacion; set => puntuacion = value;
    }
    private void Awake()
    {

        int numinstancias = FindObjectsOfType<ControlDatosJuego>().Length;

        if (numinstancias != 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }

    }


}
