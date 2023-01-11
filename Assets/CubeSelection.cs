using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSelection : MonoBehaviour
{
    [SerializeField]
    Camera m_Camera;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out var hit, 10f))
            {
              
               if(hit.transform.gameObject.tag == "Cube")
                {
                    hit.transform.GetComponentInChildren<Camera>().enabled = true;
                    hit.transform.GetComponent<NetworkCubeMovement>().enabled = true;
                    var _rigidbody = hit.transform.GetComponent<Rigidbody>();
                    _rigidbody.useGravity = false;
                    _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }
        
    }
}
