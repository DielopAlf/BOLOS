using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class contadorPajaros : MonoBehaviour

{
    public TextMeshProUGUI PuntosTxt;
    private int cuantos = 0;
    private controldatos datosJuego;

    void Start()
    {
        datosJuego = GameObject.Find("datosJuego").GetComponent<controldatos>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("cerdo"))
        {
            Debug.Log("contar");
            cuantos++;
            datosJuego.Puntuacion = cuantos;
            //PUNTOSTxt();
        }
    }
    public void PUNTOSTxt(int cuantos)
    {

        PuntosTxt.text = "Puntuacion:" + cuantos.ToString();

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("cerdo"))
        {
            cuantos--;
            datosJuego.Puntuacion = cuantos;

        }
    }



}
