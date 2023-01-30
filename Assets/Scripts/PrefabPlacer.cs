using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PrefabPlacer : MonoBehaviour
{

    public Material materialWhilePlacing;

    public System.Action<SelectableObject> onPrefabPlaced;

    /// <summary>
    /// The object we are currently placing. If null, it means we have nothing to place.
    /// </summary>
    public SelectableObject objectToPlace;

    /// <summary>
    /// The ARFoundation's RaycastManager is used to raycast and detect AR Planes.
    /// </summary>
    public ARRaycastManager raycastManager;

    /// <summary>
    /// The "Tap To Place" button
    /// </summary>
    public GameObject tapToPlaceUI;

    /// <summary>
    /// Are we placing an object currently or not?
    /// </summary>
    public bool IsPlacing { get; private set; }

    /// <summary>
    /// Used by the Raycast function of the RaycastManager
    /// </summary>
    static readonly List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        SetTapToPlaceUI(false);
    }

    //This should be called by the PrefabPicker once a Prefab is picked.
    public void InstantiatePrefabAndStartPlacing(GameObject prefab)
    {
        //Stop placing if we are already placing.
        StopPlacing();

        //Enable this
        this.gameObject.SetActive(true);

        //Enable the Tap to Place UI
        SetTapToPlaceUI(true);

        //Instantiate the prefab and start placing it.
        var spawnedPrefab = Instantiate(prefab);

        //Make sure to add our component!
        this.objectToPlace = spawnedPrefab.AddComponent<SelectableObject>();
        //
        // //Disable the selectable flag so we can't tap on it
        this.objectToPlace.SetSelectable(false);
        //
        // //Set our object to use the "transparent" look
        this.objectToPlace.materialSwapper.SetMaterial(materialWhilePlacing);

        //Set our placing flag to true.
        IsPlacing = true;
    }

    //Place the object down.
    //A button in the UI can call this function.
    public void PlaceObject()
    {

        var placedObject = objectToPlace;
        
        //Remove the object as the ObjectToPlace to put it down
        objectToPlace = null;
        
        //Change the material back to normal if we changed it.
        placedObject.materialSwapper.RevertToOriginalMaterials();
        
        //Set the selectable flag so we can tap on it.
        placedObject.SetSelectable(true);
        
        //Stop placing
        StopPlacing();
        
        //Invoke our event
        onPrefabPlaced?.Invoke(placedObject);

    }

    //Cancel the placing. This should be called when we start placing a new object.
    public void StopPlacing()
    {
        //If ObjectToPlace is already active, destroy it and start anew.
        if (this.objectToPlace != null) Destroy(this.objectToPlace.gameObject);

        //Disable the Tap to Place UI
        SetTapToPlaceUI(false);

        //Set our placing flag to false
        IsPlacing = false;
    }

    //Enable or disable the TapToPlaceUI
    void SetTapToPlaceUI(bool active)
    {
        //The ? is an inline null check
        tapToPlaceUI?.SetActive(active);
    }

    //Runs every frame. This raycasts and calculates where to place the object.
    void Update()
    {
        //If ObjectToPlace is null, we don't have anything to place... so stop placing
        if (objectToPlace == null && IsPlacing) StopPlacing();
        
        //Only run the update loop if we are currently placing an object
        if (IsPlacing)
        {
            //Where to Raycast in screen pixel coordinate space
            //We will cast to the center of the screen by dividing width and height by 2
            var screenPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        
            //Raycast against the detected AR Planes with our ScreenPoint
            if (raycastManager.Raycast(screenPoint, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;
        
                //Move the object to the detected plane position
                objectToPlace.transform.position = hitPose.position;
            }
        
        }


    }
}
