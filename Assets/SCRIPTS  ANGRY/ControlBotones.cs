using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlBotones : MonoBehaviour
{
    
    public void OnBotonJugar()
    {

        SceneManager.LoadScene("Juego");

    }
    public void OnBotonCreditos()
    {

        SceneManager.LoadScene("Creditos");

    }



}
