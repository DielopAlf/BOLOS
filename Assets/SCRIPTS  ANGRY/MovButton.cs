using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MovButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 defaultScale;
    private bool isPressed;
    private float scaleMultiplier = 1.1f;
    private float scaleDuration = 0.2f;

    void Start()
    {
        defaultScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        LeanTween.scale(gameObject, defaultScale * scaleMultiplier, scaleDuration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        LeanTween.scale(gameObject, defaultScale, scaleDuration);
    }

    // Si deseas agregar una funci�n adicional cuando se mantiene presionado el bot�n, puedes usar el m�todo Update.

    private void Update()
    {
        if (isPressed)
        {
            // Agrega aqu� tu c�digo adicional para ejecutar mientras el bot�n se mantiene presionado.
        }
    }
}