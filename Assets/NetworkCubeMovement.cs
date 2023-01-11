using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCubeMovement : NetworkBehaviour
{

    private float speed = 2.0f;

    [SerializeField]
    Camera _camera;
    [SerializeField]
    Rigidbody _rigidbody;

    // Update is called once per frame
    void Update()
    {

        if (!IsOwner) return;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        this.transform.position += horizontal * transform.right * speed * Time.deltaTime;
        this.transform.position += vertical * transform.forward * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space)) this.transform.position += transform.up * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Escape))
        {
            _camera.enabled = false;
            this.enabled = false;
            _rigidbody.useGravity = true;
            _rigidbody.constraints = RigidbodyConstraints.None;
        }
    }
}


    

