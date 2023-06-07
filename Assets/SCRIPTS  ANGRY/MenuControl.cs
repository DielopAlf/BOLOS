using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyNamespace
{
    public static class GameManager
    {
        public static bool nivel1Completado = false;
        public static bool nivel2Completado = false;
        public static bool nivel3Completado = false;
        public static bool nivel4Completado = false;
    }

    public class MenuControl : MonoBehaviour
    {
        public GameObject menuInicial;
        public GameObject menuNiveles;

        void Start()
        {
            menuInicial.SetActive(true);
            menuNiveles.SetActive(false);
        }

        public void cerrarjuego()
        {
            Application.Quit();
        }

        public void botonplay()
        {
            menuInicial.SetActive(false);
            menuNiveles.SetActive(true);
            Debug.Log("jugar");
        }

        public void volver()
        {
            menuInicial.SetActive(true);
            menuNiveles.SetActive(false);
        }
        public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        System.IO.Directory.Delete(Application.persistentDataPath, true);
        Debug.Log("Todos los datos del juego han sido eliminados.");
    }


        public void cargarNivel(string nivel)
        {
            switch (nivel)
            {
                case "nivel2":
                    if (!GameManager.nivel1Completado)
                    {
                        Debug.Log("Completa el nivel anterior primero.");
                        return;
                    }
                    break;
                case "nivel3":
                    if (!GameManager.nivel2Completado)
                    {
                        Debug.Log("Completa el nivel anterior primero.");
                        return;
                    }
                    break;
                case "nivel4":
                    if (!GameManager.nivel3Completado)
                    {
                        Debug.Log("Completa el nivel anterior primero.");
                        return;
                    }
                    break;
                // Agrega casos para cada nivel que deba ser completado antes de acceder al siguiente
            }

            SceneManager.LoadScene(nivel);
        }
    }
}