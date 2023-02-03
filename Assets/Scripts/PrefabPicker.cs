using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PrefabPicker : MonoBehaviour
{

    /// <summary>
    /// The list of prefabs to create buttons for
    /// </summary>
    public List<GameObject> prefabList;

    /// <summary>
    /// The button template to use
    /// </summary>
    public PrefabPickerButton buttonTemplate;


    /// <summary>
    /// This is called when a prefab is picked.
    /// </summary>
    public System.Action<GameObject, int> onPrefabPicked;

    public System.Action onCancel;

    /// <summary>
    /// This button will cancel the picking when pressed.
    /// </summary>
    public Button cancelButton;

    // public PriceTag itemTag;
    /// <summary>
    /// Keeping a list of all of our instantiated buttons.
    /// </summary>
    readonly List<PrefabPickerButton> m_buttonList = new List<PrefabPickerButton>();

    // Start is called before the first frame update

    private void Awake()
    {
        CreateButtonsFromPrefabsList();

    }

    void Start()
    {
        //Disable the template
        buttonTemplate.gameObject.SetActive(false);

        //Create all the buttons from the prefabs in the list

        //Cancel picking before we start just to clear all variables
        ClearHighlight();

        cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }


    void OnCancelButtonClicked()
    {
        onCancel?.Invoke();
    }

    public void ClearHighlight()
    {

        UnHighlightAllButtons();

        //Disable our Cancel button
        cancelButton.gameObject.SetActive(false);
    }

    void UnHighlightAllButtons()
    {
        for (int i = 0; i < m_buttonList.Count; i++)
        {
            m_buttonList[i].UnHighlight();
        }
    }

    //Create buttons for each prefab in the list
    void CreateButtonsFromPrefabsList()
    {
        //Loop through all the prefabs
        for (int i = 0; i < prefabList.Count; i++)
        {
            //Get the next prefab
            var prefab = prefabList[i];
            //If the prefab doesn't exist (null field) then skip it
            if (prefab == null) continue;
            //Create the button for this prefab
            var prefabButton = CreateButtonFromPrefab(prefab, i);
            // itemTag.itemId = i;
            //Add our new button to our button list
            m_buttonList.Add(prefabButton);

           var priceTag = prefab.GetComponent<PriceTag>();
            priceTag.CalculatePrice();
            priceTag.itemId = i;
        }
    }


    //This is called once for every prefab in the list
     PrefabPickerButton CreateButtonFromPrefab(GameObject prefab, int id)
     {
        // Clone the ButtonTemplate
         var prefabButton = CloneButtonTemplate();
        
         //Rename the button gameobject for organization purposes
         prefabButton.gameObject.name = prefab.name + " Button";
        
         //Link the prefab to that button
         prefabButton.SetPrefab(prefab);
        
         //Add a callback for when the button gets selected
         prefabButton.onSelect.AddListener(() =>
         {
             //Run this code when the button gets clicked
             OnButtonSelected(prefabButton, id);
         });

        // Enable our new button
         prefabButton.gameObject.SetActive(true);
    
         return prefabButton;
     }

    //This runs when a button is clicked
    void OnButtonSelected(PrefabPickerButton selectedPickerButton, int id)
    {
        //Our prefab has been picked!
        var pickedPrefab = selectedPickerButton.linkedPrefab;
    
        //UnHighlight everything else.
        UnHighlightAllButtons();
    
        //Highlight just this selected button
        selectedPickerButton.Highlight();
    
        //Print a message to the console
        Debug.Log(selectedPickerButton + " was selected");
    
        //Now, let's give the selected prefab to the PrefabPlacer
        //The PrefabPlacer should be subscribed to this event.
        //This will also pass the pickedPrefab as a parameter to the event.
        onPrefabPicked?.Invoke(pickedPrefab, id);
    
        //Enable our Cancel button
        cancelButton.gameObject.SetActive(true);
    }
    //
    //
    // //This clones the ButtonTemplate
    // //Because we may have many prefabs in the list, we need a way to create more Buttons
    PrefabPickerButton CloneButtonTemplate()
    {
        var buttonClone = Instantiate(buttonTemplate);
        buttonClone.transform.SetParent(buttonTemplate.transform.parent);
        buttonClone.transform.localRotation = buttonTemplate.transform.localRotation;
        buttonClone.transform.localScale = buttonTemplate.transform.localScale;
        return buttonClone.GetComponent<PrefabPickerButton>();
    }

}
