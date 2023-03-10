using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

//This is the master flow control script for the AR placement scene
public class AppManager : MonoBehaviour
{

    //2D UI
    public GameObject scanAreaPopup;
    public PrefabPicker prefabPicker;
    public PrefabPlacer prefabPlacer;
    public ItemCounter itemCounter;
    public MaxItemPopup maxItemPopup;
    public ARPlaneManager planeManager;
    private bool isFirstPlaneAdded = false;
    public ParticleSystem scanAnim;
    public CheckoutSystem checkoutSystem;

    //3D UI
    public MovementGizmo movementGizmo;
    public WorldSpaceInfoPanel infoPanel;

    //UI Buttons
    public Button clearSelectionButton;
    public Button clearAllButton;

    public SelectableObject currentlySelected;

    //The maximum amount of objects before the popup shows up
    public int maximumSelectables = 10;

    //The amount of current objects
    public int CurrentSelectableCount { get { return m_placedSelectables.Count; } }

    //The list of placed objects currently in the scene
    readonly List<SelectableObject> m_placedSelectables = new List<SelectableObject>();

    // Start is called before the first frame update
    void Start()
    {
        //The core coroutine
        StartCoroutine(SceneFlowRoutine());

        //Do all our event subscriptions
        prefabPicker.onPrefabPicked += OnPrefabPicked;
        prefabPicker.onCancel += OnPickingCancel;
        prefabPlacer.onPrefabPlaced += OnPrefabPlaced;
        SelectableObject.OnSelectableTapped += OnSelectableTapped;
        clearSelectionButton.onClick.AddListener(OnClearSelectionButtonClicked);
        clearSelectionButton.gameObject.SetActive(false);
        infoPanel.OnDeletingObject += OnInfoPanelDeletingObject;
        clearAllButton.onClick.AddListener(DeleteAllPlacedObjects);
        clearAllButton.onClick.AddListener(checkoutSystem.RemoveAllItemsFromList);

        UpdateItemCount();

        planeManager.planesChanged += PlaneAdded;
    }

    // Runs when info panel trash icon is clicked.
     void OnInfoPanelDeletingObject(SelectableObject deletingSelectable)
     {
         //Remove the deleted object from the list
         m_placedSelectables.Remove(deletingSelectable);
         //Update our list and counter
         UpdateDeletedObjects();
     }

    // Delete all placed objects.
     public void DeleteAllPlacedObjects()
     {
         //Loop through all the objects
         for (int i = 0; i < m_placedSelectables.Count; i++)
         {
             //Delete each one
             Destroy(m_placedSelectables[i].gameObject);
         }
         //Clear our list
         m_placedSelectables.Clear();
         //Update our counter
         UpdateDeletedObjects();
     }

    void OnClearSelectionButtonClicked()
    {
        ClearSelection();
    }

    void OnSelectableTapped(SelectableObject selectable, int id)
    {
        SelectObject(selectable, id);
    }

    void OnPrefabPicked(GameObject prefab, int id)
    {
        if (CurrentSelectableCount >= maximumSelectables)
        {
            maxItemPopup.gameObject.SetActive(true);
        }
        else
        {
            ClearSelection();
            prefabPlacer.InstantiatePrefabAndStartPlacing(prefab, id);
            CheckoutSystem.currentlySelectedItemId = id;
        }
    }
    
    void OnPickingCancel()
    {
        prefabPicker.ClearHighlight();
        prefabPlacer.StopPlacing();
    }
    
    void OnPrefabPlaced(SelectableObject placedObject, int id)
    {
        prefabPicker.ClearHighlight();
        SelectObject(placedObject, id);
        m_placedSelectables.Add(placedObject);
        UpdateItemCount();
    }

    void UpdateDeletedObjects()
    {
        //Remove any null objects from the list
        m_placedSelectables.RemoveAll(p => p == null);
        UpdateItemCount();
    }

    void UpdateItemCount()
    {
        // Update the item text and enable the clear selection button
        itemCounter.SetItemCount(CurrentSelectableCount, maximumSelectables);
        clearAllButton.gameObject.SetActive(CurrentSelectableCount > 0);
    }


    public void ClearSelection()
    {
        //Clear our variable
        currentlySelected = null;
        //Clear our 3D UI from our selection
        movementGizmo.ClearSelection();
        infoPanel.ClearSelection();
        clearSelectionButton.gameObject.SetActive(false);
    }

    public void SelectObject(SelectableObject placedObject, int id)
    {
        //Save to a variable
        currentlySelected = placedObject;
        //Set our 3D UI to the new selected object
        movementGizmo.SetSelectedObject(placedObject.gameObject);
        infoPanel.SetSelectedObject(placedObject);
        clearSelectionButton.gameObject.SetActive(true);
        CheckoutSystem.currentlySelectedItemId = id;

    }
    public IEnumerator PlayParticlesAndStop()
    {
        scanAnim.Play();
        yield return new WaitForSeconds(1.5f);
        scanAnim.Stop();
    }

    public void ManageParticles()
    {
        StartCoroutine(PlayParticlesAndStop());

    }
    private bool FirstPlaneAdded()
    {
        return isFirstPlaneAdded;
    }
    private void PlaneAdded(ARPlanesChangedEventArgs args)
    {
        if (args.added.Count > 0)
        {
            isFirstPlaneAdded = true;
        }
        else
        {
            isFirstPlaneAdded = false;
        }

       
    }

   
    IEnumerator SceneFlowRoutine()
    {
        //First, disable all the UI except for the Scan Area Popup.
        prefabPicker.gameObject.SetActive(false);

        //Enable the Scan Area Popup 
        scanAreaPopup.gameObject.SetActive(true);
        //Wait until the session state is ready.
         yield return new WaitUntil(IsARSessionTracking);
        scanAnim.Play();

        //Disable the Scan Area Popup
      

       yield return new WaitUntil(FirstPlaneAdded);
       scanAnim.Stop();
        scanAreaPopup.gameObject.SetActive(false);
        //Enable the Prefab Picker
        prefabPicker.gameObject.SetActive(true);


    }

    public bool IsARSessionTracking()
    {
        return ARSession.state == ARSessionState.SessionTracking;
    }

    void Update()
    {
        if (currentlySelected == null)
        {
            ClearSelection();
        }
    
    }

    

}
