using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger)
        {
            button.Select();
            button.onClick.Invoke();
        }
    }
}
