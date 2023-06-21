using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsCenter : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.parent.position, transform.position) > 0.01)
        {
            rb.velocity = (transform.parent.position - transform.position) * 10f;
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
