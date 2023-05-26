using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlFinNivel : MonoBehaviour
{

    public TextMeshProUGUI mensajeFinalTexto;
    private ControlDatosJuego datosjuegos;



    void Start()
    {
        datosjuegos = GameObject.Find("DatosJuego").GetComponent<ControlDatosJuego>();
        string mensajeFinal = "Numero de bolos:" + datosjuegos.Puntuacion;
        if (datosjuegos.Puntuacion == 6)
            mensajeFinal += "\n\n¡¡¡ ENHORABUENA !!!";

        mensajeFinalTexto.text = mensajeFinal;
    }

    
}
