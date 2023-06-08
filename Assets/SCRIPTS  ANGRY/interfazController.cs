using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

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

    private ControlDatosjuego datosJuego;

    void Start()
    {
        datosJuego = FindObjectOfType<ControlDatosjuego>();
        CrearVidasSprites();
    }

    public void setvidas(int vidas)
    {
        datosJuego.VidasExtras = vidas - 1;
        ActualizarVidasSprites();
    }

    public void PerderVida()
    {
        datosJuego.VidasExtras--;

        if (datosJuego.VidasExtras >= 0)
        {
            vidasSprites[vidasSprites.Count - 1].SetActive(false);
            vidasSprites.RemoveAt(vidasSprites.Count - 1);
        }
        else
        {
            MostrarPantallaDerrota();
        }
    }

    public void MostrarPantallaVictoria()
    {
        panelvictoria.SetActive(true);
        puntuacionfinal.text = "Puntuación: " + datosJuego.Puntuacion.ToString();
        puntuacionrecord.text = "Record: " + PlayerPrefs.GetInt("record" + SceneManager.GetActiveScene().name, 0).ToString();
        victoriaText.text = "¡HAS GANADO!";
    }

    public void MostrarPantallaDerrota()
    {
        panelderrota.SetActive(true);
        PuntosfinalesDerrota.text = "Puntuación: " + datosJuego.Puntuacion.ToString();
        RecordDerrota.text = "Record: " + PlayerPrefs.GetInt("record" + SceneManager.GetActiveScene().name, 0).ToString();
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
        for (int i = 0; i < datosJuego.VidasExtras; i++)
        {
            GameObject vidaSprite = Instantiate(vidaSpritePrefab, vidasContainer);
            vidasSprites.Add(vidaSprite);
        }
    }

    public void ActualizarVidasSprites()
    {
        foreach (GameObject vidaSprite in vidasSprites)
        {
            Destroy(vidaSprite);
        }

        vidasSprites.Clear();

        CrearVidasSprites();
    }
}
