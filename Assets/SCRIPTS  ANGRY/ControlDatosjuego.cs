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

    public int puntosPorColision = 20; // Valor de puntos a incrementar por cada colisión

    public int Puntuacion { get => puntuacion; set => puntuacion = value; }
    public bool Ganado { get => ganado; set => ganado = value; }
    public int MaxPuntuacion { get => maxpuntuacion; set => maxpuntuacion = value; }
    public int VidasExtras { get => vidasExtras; set => vidasExtras = value; }
    public int numeroDeCerdos; // Número total de cerdos en el nivel

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


        GameObject[] cerdos = GameObject.FindGameObjectsWithTag("cerdo");

        if (cerdos.Length > 0)
        {
            numeroDeCerdos = cerdos.Length;

        }
        Time.timeScale=1f;
        vidasExtras = 3;
    }

    public void Update()
    {
        Debug.Log("Cerdos: " + numeroDeCerdos);
        Debug.Log("Vidas: " + vidasExtras);
    }

    public bool CheckWin()
    {
        if (numeroDeCerdos <= 0)
        {
            ganado = true;
            return true;
        }
        else
        {
            ganado = false;
            return false;
        }
    }

    public void SaveRecord()
    {
        string lvlName = SceneManager.GetActiveScene().name;
        if (puntuacion >= PlayerPrefs.GetInt("Record"+lvlName, 0))
        {
            PlayerPrefs.SetInt("Record" + lvlName, puntuacion);
        }
    }

}