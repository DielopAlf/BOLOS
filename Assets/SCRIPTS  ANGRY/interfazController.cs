using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using MyNamespace;

public class interfazController : MonoBehaviour
{

   

    public GameObject spritevida;

    List<GameObject> spritevidas = new List<GameObject>();

    [HideInInspector]
    // public bool juegoDetenido = false;
    public TextMeshProUGUI puntuacionfinal;
    public GameObject panelvictoria;
    public TextMeshProUGUI puntuacionrecord;
    public TextMeshProUGUI PuntosfinalesDerrota;

    public Vector2 posprimeravida;

    public TextMeshProUGUI RecordDerrota;
    float metros;
    public TextMeshProUGUI victoriaText;

    private controldatos datosJuego;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setvidas(int vidas)
    {
        if (vidas > 1)
        {
            Vector2 pos = posprimeravida;

            for (int i = 1; i < vidas; i++)
            {

                GameObject sprite = Instantiate(spritevida, pos, Quaternion.identity);

                spritevidas.Add(sprite);
                pos = new Vector2(pos.x + 0.75f, pos.y);
            }

        }

    }
    public void perdervida()
    {

        if (spritevidas.Count > 0)
        {
            spritevidas[spritevidas.Count - 1].SetActive(false);
            spritevidas.RemoveAt(spritevidas.Count - 1);

        }
        else
        {

            panelvictoria.SetActive(true);
            PuntosfinalesDerrota.text = "puntuacion: " + datosJuego.Puntuacion.ToString();
            RecordDerrota.text = "record: " + PlayerPrefs.GetInt("record" + SceneManager.GetActiveScene().name, 0).ToString();

            //  audioSource.Stop();
            //  audioSource.PlayOneShot(gameoverAudioClip);
        }
    }
}
