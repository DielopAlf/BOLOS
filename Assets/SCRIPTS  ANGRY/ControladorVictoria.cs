using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ControladorVictoria : MonoBehaviour
{
    public TextMeshProUGUI mensajeFinalTexto;
    private ControlDatosjuego datosjuego;
    public TextMeshProUGUI mensajeResultado;

    private bool ultimoPajaroQuieto = false;

    void Start()
    {
        datosjuego = GameObject.Find("DatosJuego").GetComponent<ControlDatosjuego>();
        if (datosjuego == null)
        {
            Debug.LogError("No se encontró el objeto DatosJuego en la escena.");
        }
        else
        {
            string mensajeFinal = (datosjuego.Ganado) ? "¡HAS GANADO!" : "¡HAS PERDIDO!";
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
                    mensajeFinal2 = "¡Excelente, conseguiste todos los puntos!";
                }
                else if (porcentajeconseguido >= 0.66f && porcentajeconseguido < 1f)
                {
                    mensajeFinal2 = "Casi, estás a nada de lograrlo.";
                }
                else
                {
                    mensajeFinal2 = "Qué pocos puntos has conseguido.";
                }

                mensajeResultado.text = mensajeFinal2;
            }
            else
            {
                mensajeResultado.gameObject.SetActive(false);
            }
        }
    }
    public void DetectarUltimoPajaroQuieto()
    {
        if (!ultimoPajaroQuieto)
        {
            // Aquí puedes agregar la lógica para determinar si el último pájaro está completamente quieto
            // Puedes verificar la velocidad lineal y angular del pájaro y compararla con un umbral muy pequeño
            // Si el pájaro está completamente quieto, establece la variable ultimoPajaroQuieto en true y muestra la pantalla correspondiente
        }
    }
}
