using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class PhysicalButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        //xr = (XRController)FindObjectOfType(typeof(XRController));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger)
        {
            if (other.transform.parent.TryGetComponent(out ActionBasedController xr))
            {
                xr.SendHapticImpulse(0.7f, 0.1f);
                button.Select();
                button.onClick.Invoke();
            }
        }
    }
}
