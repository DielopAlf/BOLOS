using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class ControladorVictoria : MonoBehaviour
{
    public TextMeshProUGUI mensajeFinalTexto;
    private ControlDatosjuego datosjuego;
    public TextMeshProUGUI mensajeResultado;


    void Start()
    {
        datosjuego = GameObject.Find("DatosJuego").GetComponent<ControlDatosjuego>();
        string mensajeFinal = (datosjuego.Ganado) ? "HA GANADO!!" : "HA PERDIDO";
        if (datosjuego.Ganado) mensajeFinal += "Puntuación:" + datosjuego.Puntuacion;

        mensajeFinalTexto.text = mensajeFinal;


        if (datosjuego.Ganado)
        {
            string mensajeFinal2 = "";
            float porcentajeconseguido = (float)datosjuego.Puntuacion / (float)datosjuego.MaxPuntuacion;
            Debug.Log(datosjuego.Puntuacion);
            Debug.Log(datosjuego.MaxPuntuacion);
            Debug.Log(porcentajeconseguido);
            if (porcentajeconseguido >= 1f)
            {

                mensajeFinal2 = "excelente los conseguiste todos";


            }
            else if (porcentajeconseguido >= 0.66f && porcentajeconseguido < 1f)
            {
                mensajeFinal2 = "casi,estas a nada de lograrlo";


            }
            
            else
            {

                mensajeFinal2 = "que pocos";

            }

            mensajeResultado.text = mensajeFinal2;

        }
        else
        {
            mensajeResultado.gameObject.SetActive(false);

        }
    }


}