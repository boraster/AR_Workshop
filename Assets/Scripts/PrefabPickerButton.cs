using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class PrefabPickerButton : MonoBehaviour
{

    public TextMeshProUGUI labelText;

    public GameObject linkedPrefab;
    public UnityEvent onSelect;
    public UnityEvent onDeselect;

    public GameObject enableOnSelected;

    public bool IsHighlighted { get; private set; }

    Button m_button;

    void Awake()
    {
        if (labelText == null) labelText = GetComponentInChildren<TextMeshProUGUI>();

        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        //Invoke the OnSelect event
        //The PrefabPicker should have set up this event to select the corresponding prefab when it created this button
        onSelect.Invoke();
    }

    public void SetPrefab(GameObject prefab)
    {
        this.linkedPrefab = prefab;

        if (labelText != null)
        {
            labelText.text = linkedPrefab.name;
        }
    }

    public void Highlight()
    {
        //Don't highlight if already highlighted.
        if (IsHighlighted) return;

        //Set our isHighlighted variable
        IsHighlighted = true;

        //Enable our highlight object
        if (enableOnSelected) enableOnSelected.gameObject.SetActive(true);
    }

    public void UnHighlight()
    {
        //Don't run if already unhighlighted.
        if (!IsHighlighted) return;
        //Set our isHighlighted variable
        IsHighlighted = false;
        //Disable our Highlight object
        if (enableOnSelected) enableOnSelected.gameObject.SetActive(false);
    }


}
