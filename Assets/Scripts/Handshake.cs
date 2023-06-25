using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Handshake : MonoBehaviour
{
    public GameObject target;

    private void Update()
    {
        if(target != null)
        {
            transform.parent.LookAt(target.transform.position);
            transform.parent.eulerAngles = transform.parent.eulerAngles + Vector3.right * 90;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (other.transform.parent.TryGetComponent(out ActionBasedController xr))
            {
                xr.SendHapticImpulse(0.7f, 0.1f);
                target = other.transform.parent.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            if (other.transform.parent.TryGetComponent(out ActionBasedController xr))
            {
                target = null;
            }
        }
    }
}
