using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonEnabler : MonoBehaviour
{
    [SerializeField] int nivel = 0;

    void Awake()
    {
        int superado = PlayerPrefs.GetInt("NivelSuperado" + (nivel - 1).ToString(), 0);
        Debug.Log(superado + " hemos leido este valor");
        GetComponent<Button>().interactable = superado == 1 ? true : false;
    }
}