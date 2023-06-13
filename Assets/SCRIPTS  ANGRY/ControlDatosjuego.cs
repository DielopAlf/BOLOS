using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ControlDatosjuego : MonoBehaviour
{
    private int maxpuntuacion;
    private int puntuacion;
    private bool ganado;
    private int vidasExtras;

    public int Puntuacion { get => puntuacion; set => puntuacion = value; }
    public bool Ganado { get => ganado; set => ganado = value; }
    public int MaxPuntuacion { get => maxpuntuacion; set => maxpuntuacion = value; }
    public int VidasExtras { get => vidasExtras; set => vidasExtras = value; }

    public static ControlDatosjuego instance;


    private void Awake()
    {
        /*int numInstancias = FindObjectsOfType<ControlDatosjuego>().Length;

        if (numInstancias != 1)
        {
            //Destroy(this.gameObject);
        }
        else
        {
            //DontDestroyOnLoad(this.gameObject);
        }
        */

        if (instance != null && instance!= this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }


        controldatos[] cerdos = FindObjectsOfType<controldatos>();

        if (cerdos.Length > 0)
        {
            foreach (controldatos powerup in cerdos)
            {

            }
        }
    }
}