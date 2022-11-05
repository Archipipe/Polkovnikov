using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Spin : MonoBehaviour
{
    public float spinSpeed;
    public float moveSpeed;
    public float moveHeight;
    Vector3 startPosition;
    float t = 0;
    

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        t += Time.deltaTime;
        transform.position = startPosition + new Vector3(0,moveHeight * Mathf.Sin(t*moveSpeed),0);

    }


    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
    }
}
