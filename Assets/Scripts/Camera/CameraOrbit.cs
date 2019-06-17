using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{



    public float distance = 10f;

    public float xSpeed = 120f;
    public float ySpeed = 120f;


    public float yMin = 15f;
    public float yMax = 80f;


    public float x = 0.0f;
    public float y = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 euler = transform.eulerAngles;
        x = euler.y;
        y = euler.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.visible = false;


            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            x += mouseX * xSpeed * Time.deltaTime;
            y -= mouseY * ySpeed * Time.deltaTime;


            y = Mathf.Clamp(y, yMin, yMax);

        }
        else
        {
            Cursor.visible = true;
        }
        transform.rotation = Quaternion.Euler(y, x, 0);
        transform.position = -transform.forward * distance;
    }
}
