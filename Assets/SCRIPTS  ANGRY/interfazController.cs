using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class interfazController : MonoBehaviour
{
    public GameObject vidaSpritePrefab;
    public Transform vidasContainer;

    private List<GameObject> vidasSprites = new List<GameObject>();

    public TextMeshProUGUI puntuacionfinal;
    public GameObject panelvictoria;
    public GameObject panelderrota;
    public TextMeshProUGUI puntuacionrecord;
    public TextMeshProUGUI PuntosfinalesDerrota;
    public Vector2 posprimeravida;
    public TextMeshProUGUI RecordDerrota;
    public float metros;
    public TextMeshProUGUI victoriaText;

    public ControlDatosjuego datosJuego;
    private bool partidaTerminada = false;

    private void Start()
    {
        datosJuego = ControlDatosjuego.instance;

        if (datosJuego == null)
        {
            Debug.LogError("No se encontró el objeto ControlDatosjuego en la escena.");
        }
        else
        {
            CrearVidasSprites();
        }

    }

    public void setvidas(int vidas)
    {
        if (datosJuego != null)
        {
            datosJuego.VidasExtras = vidas - 1;
            ActualizarVidasSprites();
        }
    }

    public void PerderVida()
    {
        if (datosJuego != null)
        {
            datosJuego.VidasExtras--;

            if (datosJuego.VidasExtras >= 0)
            {
                if (vidasSprites.Count > 0)
                {
                    vidasSprites[vidasSprites.Count - 1].SetActive(false);
                    vidasSprites.RemoveAt(vidasSprites.Count - 1);
                }
                else
                {
                    Debug.LogError("No hay suficientes sprites de vida para eliminar.");
                }
            }
            else
            {
                MostrarPantallaDerrota();
            }
        }
    }

    public void MostrarPantallaVictoria()
    {
        if (!partidaTerminada)
        {
            partidaTerminada = true;
            panelvictoria.SetActive(true);
            puntuacionfinal.text = "Puntuación: " + datosJuego.Puntuacion.ToString();
            puntuacionrecord.text = "Record: " + PlayerPrefs.GetInt("record" + SceneManager.GetActiveScene().name, 0).ToString();
            victoriaText.text = "¡HAS GANADO!";
        }
    }

    public void MostrarPantallaDerrota()
    {
        if (!partidaTerminada)
        {
            partidaTerminada = true;
            panelderrota.SetActive(true);
            PuntosfinalesDerrota.text = "Puntuación: " + datosJuego.Puntuacion.ToString();
            RecordDerrota.text = "Record: " + PlayerPrefs.GetInt("record" + SceneManager.GetActiveScene().name, 0).ToString();
        }
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void ReintentarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void CrearVidasSprites()
    {
        if (vidasContainer != null && vidaSpritePrefab != null)
        {
            for (int i = 0; i < datosJuego.VidasExtras; i++)
            {
                GameObject vidaSprite = Instantiate(vidaSpritePrefab, vidasContainer);
                vidasSprites.Add(vidaSprite);
            }
        }
        else
        {
            Debug.LogError("El contenedor de vidas o el prefab del sprite de vida no están asignados.");
        }
    }

    private void ActualizarVidasSprites()
    {
        foreach (GameObject vidaSprite in vidasSprites)
        {
            Destroy(vidaSprite);
        }

        vidasSprites.Clear();

        CrearVidasSprites();
    }
}