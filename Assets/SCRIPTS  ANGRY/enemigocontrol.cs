using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemigocontrol : MonoBehaviour
{
    public int disparosNecesarios = 1;

    private int disparosRecibidos = 0;



    private void OnTriggerEnter2D(Collider2D other)
{
        Debug.Log("COLISION");
    if (other.gameObject.CompareTag("Pajaro"))
    {
        

        disparosRecibidos++;

        if (disparosRecibidos >= disparosNecesarios)
        {



            Destroy(gameObject);
             
        }
    }

    else if (other.gameObject.CompareTag("Player"))
    {
        other.gameObject.GetComponent<ControlLanzamientos>();
    }

  }
}
