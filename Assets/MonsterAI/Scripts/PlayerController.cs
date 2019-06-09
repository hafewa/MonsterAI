using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private float RunSpeed;
    [SerializeField] private float RotationSpeed;

    private Vector3 moveDirection;

    private void Update () {
        if (Input.GetAxis("Vertical") > 0)
        {
            moveDirection = transform.forward;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            moveDirection = -transform.forward;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        transform.Rotate(0, Input.GetAxis("Horizontal")  * RotationSpeed, 0);
    }

    private void FixedUpdate()
    {
        m_rigidbody.AddForce(moveDirection);
        m_rigidbody.velocity = moveDirection * RunSpeed;
    }
}
