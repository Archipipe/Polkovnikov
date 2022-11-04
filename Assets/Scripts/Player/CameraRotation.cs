using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    void Start()
    {
        // Закрепляем курсор посередине и убираем его
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
    }

    void Update()
    {
        // ПОлучаем поворот мыши
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Двигаем камеру
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        // Поворачиваем тело
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }
}
