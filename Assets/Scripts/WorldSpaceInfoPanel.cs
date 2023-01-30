using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceInfoPanel : MonoBehaviour
{

    public Camera raycastCamera;
    public SelectableObject selectedObject;
    public RectTransform panel;
    public Button deleteButton;
    public MaterialButton materialButton;
    public MaterialPanel materialPanel;

    public System.Action<SelectableObject> OnDeletingObject { get; set; }
    public bool IsShowing { get; private set; }


    private void Start()
    {
        //Link our buttons to our callbacks.
        deleteButton.onClick.AddListener(OnDeleteClicked);
        materialButton.button.onClick.AddListener(OnMaterialClicked);
        
        this.materialPanel.Hide();
        this.gameObject.SetActive(IsShowing);

        this.materialPanel.OnMaterialSetSelected += UpdateMaterialButtonColor;
    }

    //When a material is set, update the color of the button
    void UpdateMaterialButtonColor(MaterialSwapper.MaterialSet set)
    {
        materialButton.ApplyMaterialSet(set);
    }

    //Set the selected object.
    public void SetSelectedObject(SelectableObject selectable)
    {
        this.selectedObject = selectable;
        UpdateMaterialButtonColor(selectable.materialSwapper.CurrentSet);
        materialPanel.Hide();
        Show();
    }


    //Remove the selected object and hide the panel.
    public void ClearSelection()
    {
        this.selectedObject = null;
        Hide();
    }

    public void Show()
    {
        //Don't run Show if already showing.
        //This is so you can put sounds/animation here and not have it run more than once.
        if (IsShowing) return;
        IsShowing = true;
        UpdatePosition();
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        //Don't run Hide if already not showing.
        //This is so you can put sounds/animation here and not have it run more than once.
        if (!IsShowing) return;
        IsShowing = false;
        this.gameObject.SetActive(false);
    }

    //This runs when the Delete button is clicked.
    void OnDeleteClicked()
    {
        //Call the deleting event.
        OnDeletingObject?.Invoke(selectedObject);
        //
        // //Destroy the object.
        Destroy(selectedObject.gameObject);
        // //You can change this to call an animation, for example,
        Hide();
    }

    //This runs when the Material button is clicked.
    void OnMaterialClicked()
    {
        if (this.materialPanel.IsShowing)
        {
            this.materialPanel.Hide();
        }
        else
        {
            var materialSwapper = selectedObject.GetComponent<MaterialSwapper>();
            if (materialSwapper)
            {
                this.materialPanel.Show(materialSwapper);
            }
        }
    }

    //Late Update runs after all Updates.
    //We put it in LateUpdate as the camera may have moved in Update.
    private void LateUpdate()
    {
        //Only update our position if we have a selected object.
        if (selectedObject != null)
        {
            //Update our canvas space position based on world space.
            UpdatePosition();
        }
        else
        {
            //Otherwise, if there's no selected object, Hide this panel.
            Hide();
        }
    }

    //Update our Canvas Space position based on World Space.
    void UpdatePosition()
    {
        var selectedObjectScreenPoint = RectTransformUtility.WorldToScreenPoint(raycastCamera, selectedObject.transform.position);
        var worldToCanvasPosition = Vector2.zero;
        var isValid = RectTransformUtility.ScreenPointToLocalPointInRectangle(panel.parent as RectTransform, selectedObjectScreenPoint, null, out worldToCanvasPosition);
        if (isValid)
        {
            panel.anchoredPosition = worldToCanvasPosition;
        }

    }

}
