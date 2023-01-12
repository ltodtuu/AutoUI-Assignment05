using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class characterScript : NetworkBehaviour
{
    public static HashSet<ulong> ActivePlayers = new HashSet<ulong>();

    [SerializeField]
    GameObject headPose;

    [SerializeField]
    Camera cam;

    public float mouseSensitivity = 400f;
    float xRotation = 10f;
    float yRotation = -10f;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if(IsOwner) cam.enabled = true;

        if (IsOwnedByServer)
        {
            transform.position = new Vector3(-3, 5, -3.4f);
        }
        else
        {
            transform.position = new Vector3(-10.5f, 5, -6.4f);
            transform.rotation = new Quaternion(0, 180, 0,0);
        }
        ActivePlayers.Add(this.OwnerClientId);
    }

    protected virtual void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

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
