using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemigocontrol : MonoBehaviour
{
    public int disparosNecesarios = 1;

    private int disparosRecibidos = 0;

    private nextlevel nivelController; // Referencia al script "nextlevel"

    private void Start()
    {
        nivelController = FindObjectOfType<nextlevel>(); // Obtener una referencia al script "nextlevel"
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("COLISION");
        if (other.gameObject.CompareTag("Pajaro"))
        {
            disparosRecibidos++;

            if (disparosRecibidos >= disparosNecesarios)
            {
                Destroy(gameObject);

                nivelController.LoadA("PantallaVictoria"); // Cargar la escena de victoria utilizando el método LoadA del script "nextlevel"
            }
        }
    }
}
