using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Audio;


public class PhysicalButton : MonoBehaviour
{
    private Button button;
    private float timeSinceEnabled;
    [SerializeField] AudioSource C_Click;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        //xr = (XRController)FindObjectOfType(typeof(XRController));
    }

    private void Update()
    {
        timeSinceEnabled += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (timeSinceEnabled < 0.3f) return;
        if(!other.isTrigger)
        {
            if (other.transform.parent.TryGetComponent(out ActionBasedController xr))
            {
                xr.SendHapticImpulse(0.7f, 0.1f);
                button.Select();
                button.onClick.Invoke();
                C_Click.Play();
            }
        }
    }
}
