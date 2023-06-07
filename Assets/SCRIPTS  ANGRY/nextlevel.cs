using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextlevel : MonoBehaviour
{
   
    void Start()
    {
        Debug.Log("cargarnivel2");
    }

    
    public void LoadA(string nivel)
    {

        SceneManager.LoadScene(nivel);

    }
}
