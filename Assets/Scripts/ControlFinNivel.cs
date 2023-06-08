using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlFinNivel : MonoBehaviour
{

    public TextMeshProUGUI mensajeFinalTexto;
    private ControlDatosjuego datosjuegos;



    void Start()
    {
        datosjuegos = GameObject.Find("datosJuego").GetComponent<ControlDatosjuego>();
        string mensajeFinal = "Numero de bolos:" + datosjuegos.Puntuacion;
        if (datosjuegos.Puntuacion == 6)
            mensajeFinal += "\n\n¡¡¡ ENHORABUENA !!!";

        mensajeFinalTexto.text = mensajeFinal;
    }

    
}
