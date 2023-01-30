using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;


//This script handles the opacity of a single handle of the gizmo
//This will also pass though all of Unity's EventSystem interface calls, to detect drags and selections.
//The MovementGizmo script takes the data and moves the objects
public class WorldGizmoHandle : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDeselectHandler, IInitializePotentialDragHandler, IPointerExitHandler
{
    //Called when the handle is dragged, every frame.
    public System.Action<Vector2> onDrag;
    //Called when the handle starts being dragged
    public System.Action<Vector2> onBeginDrag;

    //The canvasgroup we will set opacity for when the handle is dragged
    public CanvasGroup opacityGroup;
    //What opacity to set when being dragged
    public float highlightOpacity = 1f;
    //What opacity to set when not being dragged
    public float unhighlightOpacity = .5f;
    //Whether or not the gizmo handle is currently highlighted
    public bool IsHighlighted { get; private set; }


    //Set the gizmo handle's highlighted state
    public void SetHighlight(bool highlightActive)
    {
        //Don't do anything if we have already set
        if (IsHighlighted == highlightActive) return;

        //Set our bool
        IsHighlighted = highlightActive;

        //Change the opacity for the highlight group, if it exists
        if (opacityGroup) opacityGroup.alpha = IsHighlighted ? highlightOpacity : unhighlightOpacity;
    }

    //Called by Unity's EventSystem (IBeginDragHandler)
    //This is called when this object is just starting to be dragged
    public void OnBeginDrag(PointerEventData eventData)
    {
        //This is the position of the cursor or touch
        var pointerPosition = eventData.position;

        //Call our own onBeginDrag event, passing the touch position
        //The MovementGizmo script should subscribe to this event and do the actual movement
        onBeginDrag?.Invoke(pointerPosition);

        //Set our handle's opacity
        SetHighlight(true);
    }

    //Called by Unity's EventSystem (IDragHandler)
    //This is called every frame as this object is being dragged
    public void OnDrag(PointerEventData eventData)
    {
        //This is the position of the cursor or touch
        var pointerPosition = eventData.position;

        //Call our own onDrag event, passing the touch position
        //The MovementGizmo script should subscribe to this event and do the actual movement
        onDrag?.Invoke(eventData.position);

        //Set our handle's opacity
        SetHighlight(true);
    }

    //Called by Unity's EventSystem (IOnEndDragHandler)
    public void OnEndDrag(PointerEventData eventData)
    {
        //Clear our highlight
        SetHighlight(false);
    }


    //Called by Unity's EventSystem (IDeselectHandler)
    public void OnDeselect(BaseEventData eventData)
    {
        //Clear our highlight
        SetHighlight(false);
    }

    //Called by Unity's EventSystem (OnInitializePotentialDragHandler)
    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        //Enable our highlight as we might begin dragging
        SetHighlight(true);
    }

    //Called by Unity's EventSystem (IPointerExitHandler)
    public void OnPointerExit(PointerEventData eventData)
    {
        //Disable our highlight if we are not dragging
        if (!eventData.dragging)
        {
            SetHighlight(false);
        }
    }
}
