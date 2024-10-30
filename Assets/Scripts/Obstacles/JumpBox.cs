using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
    public float jumpForce;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(0,jumpForce,0),ForceMode.Impulse);
        }
    }
}
