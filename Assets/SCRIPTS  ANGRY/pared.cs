using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pared : MonoBehaviour
{
    public int disparosNecesarios = 1;
    private int disparosRecibidos = 0;
    public float velocidadNormal;
    public float velocidadMinimaParaRomper = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("COLISION con " + other.gameObject.tag + " y su velocidad es " + other.attachedRigidbody.velocity);
        // other.attachedRigidbody.velocity representa la velocidad en la que dos cuerpos se han chocado.
        Vector2 velocidadGolpe = other.attachedRigidbody.velocity;
        float velocidadX = velocidadGolpe.x;
        float velocidadY = velocidadGolpe.y;

        if (other.gameObject.CompareTag("Pajaro"))
        {
            disparosRecibidos++;

            if (disparosRecibidos >= disparosNecesarios)
            {
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.CompareTag("cerdo"))
        {
            disparosRecibidos++;

            if (disparosRecibidos >= disparosNecesarios)
            {
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.CompareTag("pared"))
        {
            if (velocidadX >= velocidadMinimaParaRomper && velocidadY >= velocidadMinimaParaRomper)
            {
                Destroy(gameObject);
            }
        }
    }
}