using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ControlFInal : MonoBehaviour

{

    public TextMeshProUGUI mensajeFinalTexto;
    private ControlDatosJuego datosjuegos;



    void Start()
    {
        datosjuegos = GameObject.Find("DatosJuego").GetComponent<ControlDatosJuego>();
        string mensajeFinal = "Puntuacion" + datosjuegos.Puntuacion;
        if (datosjuegos.Puntuacion == 6)
            mensajeFinal += "\n\n¡¡¡ ENHORABUENA !!!";

        mensajeFinalTexto.text = mensajeFinal;
    }


}
