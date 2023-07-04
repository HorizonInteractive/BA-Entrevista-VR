using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeselectionWorkaround : MonoBehaviour
{
    private EventSystem _eventSystem;

    private GameObject _currentlySelected;

    private void Awake()
    {
        _eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (_eventSystem.currentSelectedGameObject == _currentlySelected) return;
        if (_eventSystem.currentSelectedGameObject == null)
        {
            _eventSystem.SetSelectedGameObject(_currentlySelected);
            return;
        }
        _currentlySelected = _eventSystem.currentSelectedGameObject;
    }
}
