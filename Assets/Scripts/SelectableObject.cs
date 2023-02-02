using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class SelectableObject : MonoBehaviour, IPointerClickHandler
{
    //Global event for others to subscribe to when this object gets selected.
    public static System.Action<SelectableObject, int> OnSelectableTapped;

    public PriceTag priceTag;
    //The materialSwapper helps us set materials on all the renderers
    public MaterialSwapper materialSwapper { get; private set; }

    //Whether or not to allow this object to be selected
    public bool DisableSelection { get; private set; }

    //Is this object selectable?
    public bool IsSelectable { get { return !DisableSelection; } }

    //Set whether or not we can select this object
    public void SetSelectable(bool selectable)
    {
        this.DisableSelection = !selectable;
    }

    void Awake()
    {
        //Create the MaterialSwapper component if it doesn't exist.
        materialSwapper = GetComponent<MaterialSwapper>();
        if (materialSwapper == null) materialSwapper = gameObject.AddComponent<MaterialSwapper>();

        priceTag = GetComponent<PriceTag>();
    }

    //When tapped on by the user, run our OnSelectableTapped callback
    public void OnPointerClick(PointerEventData eventData)
    {

        //Don't do anything if we've disabled selection
        if (DisableSelection)
        {
            return;
        }

        //This object gets selected when clicked on
        OnSelectableTapped?.Invoke(this, priceTag.itemId);
    }

}
