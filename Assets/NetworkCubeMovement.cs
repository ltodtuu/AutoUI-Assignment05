using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class NetworkCubeMovement : NetworkBehaviour
{

    private float speed = 2.0f;
    private float rotationSpeed = 40.0f;

    [SerializeField]
    Camera _camera;
    [SerializeField]
    Rigidbody _rigidbody;
    [SerializeField]
    Material _player2Material;

    private Vector3 _rotation;

    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();
        if (characterScript.ActivePlayers.Count > 1 && OwnerClientId == characterScript.ActivePlayers.ToList()[1]) this.GetComponentInChildren<Renderer>().material = _player2Material;
    }
    // Update is called once per frame
    void Update()
    {

        if (!IsOwner) return;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        this.transform.position += horizontal * transform.right * speed * Time.deltaTime;
        this.transform.position += vertical * transform.forward * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q)) this._rotation = Vector3.up;
        else if (Input.GetKey(KeyCode.E)) this._rotation = Vector3.down;
        else this._rotation = Vector3.zero;

        this.transform.Rotate(this._rotation * rotationSpeed * Time.deltaTime);

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


    

