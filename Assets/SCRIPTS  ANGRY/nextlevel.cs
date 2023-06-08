using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextlevel : MonoBehaviour
{
    public string nivelSiguiente; // Nombre del siguiente nivel

    public void LoadA(string nivel)
    {
        SceneManager.LoadScene(nivel);
    }

    public void DesbloquearNivel()
    {
        
    }
}
