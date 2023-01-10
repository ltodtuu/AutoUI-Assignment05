using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterScript : MonoBehaviour
{
    [SerializeField]
    GameObject headPose;

    [SerializeField]
    Camera cam;

    public float mouseSensitivity = 400f;
    float xRotation = 10f;
    float yRotation = -10f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        
        cam.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        cam.transform.localRotation *= Quaternion.Euler(xRotation, 0f, 0f);
    }
}
