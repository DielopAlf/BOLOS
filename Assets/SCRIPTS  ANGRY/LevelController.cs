using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;

    private bool isMovingWithTouch = false;

    private void Update()
    {
        Vector3 rot = Vector3.zero;

        // Movimiento horizontal al tocar la pantalla de una tablet o dispositivo táctil
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float touchX = touch.deltaPosition.x;
            if (touchX != 0f)
            {
                rot = new Vector3(0f, -touchX / 10f, 0f);
                isMovingWithTouch = true;
            }
        }
        else
        {
            isMovingWithTouch = false;
        }

        transform.Rotate(rot * movementSpeed * Time.deltaTime);
    }
}
