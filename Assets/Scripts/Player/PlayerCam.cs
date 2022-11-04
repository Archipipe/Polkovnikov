using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensitivity;
    public Transform orientation;

    float xRotation;
    float yRotation;

    private void Start()
    {
        // Закрепляем курсор на середине экрана и убираем его
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //Получаем движение мыши
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;


        //Получаем поворот камеры
        yRotation += mouseX;

        xRotation -= mouseY;
        //Фиксируем поворот по горизонтали между -90 и 90
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Поворачиваем камеру и тело игрока
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0,yRotation, 0);
    }

}
