using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlDatosjuego : MonoBehaviour
{
    private static ControlDatosjuego instance;

    public static ControlDatosjuego Instance
    {
        get { return instance; }
    }

    private int maxPuntuacion;
    private int puntuacion;
    private bool ganado;
    private int vidasExtras;

    public int Puntuacion
    {
        get => puntuacion;
        set => puntuacion = value;
    }

    public bool Ganado
    {
        get => ganado;
        set => ganado = value;
    }

    public int MaxPuntuacion
    {
        get => maxPuntuacion;
        set => maxPuntuacion = value;
    }

    public int VidasExtras
    {
        get => vidasExtras;
        set => vidasExtras = value;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        controldatos[] cerdos = FindObjectsOfType<controldatos>();

        if (cerdos.Length > 0)
        {
            foreach (controldatos powerup in cerdos)
            {
                
            }
        }
    }
}
