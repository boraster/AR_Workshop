using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGizmo : MonoBehaviour
{
    //The currently selected object to adjust
    public GameObject selectedObject;

    //The view camera
    public Camera raycastCamera;

    //Each of the handle axes
    public WorldGizmoHandle rotationHandle;
    public WorldGizmoHandle moveXHandle;
    public WorldGizmoHandle moveZHandle;
    public WorldGizmoHandle moveXZHandle;

    //Whether or not the gizmo is active or not
    public bool IsShowing { get; private set; }

    Vector3 m_grabbedPositionOffset;
    float m_grabbedAngleOffset;
    Vector3 m_grabbedPosition;
    Quaternion m_grabbedRotation;

    private void Awake()
    {
        //Event subscriptions
        rotationHandle.onDrag += OnRotationHandleDrag;
        moveXHandle.onDrag += OnMoveHandleDragAlignX;
        moveZHandle.onDrag += OnMoveHandleDragAlignZ;
        moveXZHandle.onDrag += OnMoveHandleDrag;

        rotationHandle.onBeginDrag += OnBeginHandleDrag;
        moveXHandle.onBeginDrag += OnBeginHandleDrag;
        moveZHandle.onBeginDrag += OnBeginHandleDrag;
        moveXZHandle.onBeginDrag += OnBeginHandleDrag;

        Hide();
    }

    public void SetSelectedObject(GameObject selectedObject)
    {
        if (selectedObject == null)
        {
            Hide();
        }
        else
        {
            this.selectedObject = selectedObject;
            UpdatePosition();
            Show();
        }
    }

    public void ClearSelection()
    {
        this.selectedObject = null;
        Hide();
    }

    public void Show()
    {
        if (IsShowing) return;
        //You can change this method and add functionality. Add animations, for example.
        UpdatePosition();
        this.gameObject.SetActive(true);
        IsShowing = true;
    }

    public void Hide()
    {
        if (!IsShowing) return;
        //You can change this method and add functionality. Add animations, for example.
        this.gameObject.SetActive(false);
        IsShowing = false;
    }

    void OnBeginHandleDrag(Vector2 touchPosition)
    {
        var worldTouchPosition = selectedObject.transform.position;
        var handleTouched = GetTouchedWorldPosition(touchPosition, ref worldTouchPosition);
        if (handleTouched)
        {
            m_grabbedPosition = selectedObject.transform.position;
            m_grabbedRotation = selectedObject.transform.rotation;
            m_grabbedPositionOffset = selectedObject.transform.position - worldTouchPosition;
            m_grabbedAngleOffset = Vector3.SignedAngle(m_grabbedPositionOffset, Vector3.forward, Vector3.up);
        }
    }


    void OnRotationHandleDrag(Vector2 touchPos)
    {
        var worldTouchPos = selectedObject.transform.position;
        GetTouchedWorldPosition(touchPos, ref worldTouchPos);
        var touchPosOffset = selectedObject.transform.position - worldTouchPos;
        var desiredAngle = Vector3.SignedAngle(touchPosOffset, Vector3.forward, Vector3.up);
        var desiredRotation = Quaternion.Euler(0, m_grabbedAngleOffset - desiredAngle, 0);

        selectedObject.transform.rotation = m_grabbedRotation * desiredRotation;
    }

    //Free movement on X and Z
    void OnMoveHandleDrag(Vector2 touchPos)
    {
        selectedObject.transform.position = GetDraggedPosition(touchPos);
    }

    //Only move along X
    void OnMoveHandleDragAlignX(Vector2 touchPos)
    {
        selectedObject.transform.position = GetAlignedMovement(touchPos, selectedObject.transform.right);
    }

    //Only move along Z
    void OnMoveHandleDragAlignZ(Vector2 touchPos)
    {
        selectedObject.transform.position = GetAlignedMovement(touchPos, selectedObject.transform.forward);
    }


    //This is used to calculate the dragged position of the touchpoint
    Vector3 GetDraggedPosition(Vector2 touchPos)
    {
        //Initialize worldPoint with a default value. The next function will populate it if successful.
        var worldPoint = Vector3.zero;

        //This function does the raycast function to find where to place the new object along its flat horizontal Axis.
        var isValidPosition = GetTouchedWorldPosition(touchPos, ref worldPoint);
        if (isValidPosition)
        {
            //The position is valid. Return the new WorldPoint for this touch position.
            return worldPoint + m_grabbedPositionOffset;
        }
        else
        {
            //The position is not valid. Return the object's position.
            return selectedObject.transform.position;
        }
    }

    //This is used for axis-aligned movement along X and Z axes.
    Vector3 GetAlignedMovement(Vector2 touchPos, Vector3 alignAxis)
    {
        //Use the GetDraggedPosition function to convert touchPos to a position.
        var desiredWorldPosition = GetDraggedPosition(touchPos);

        //Now, we need to axis align this position along the axis we dragged.
        var axisAlignedWorldPosition = AxisAlignPosition(desiredWorldPosition, selectedObject.transform, alignAxis);

        return axisAlignedWorldPosition;
    }


    /// <summary>
    /// Aligns "position" along "axis" of the point "origin" in world space.
    /// </summary>
    /// <param name="position">The position to align.</param>
    /// <param name="origin">The origin to align to. Usually, the position of the object you are aligning.</param>
    /// <param name="worldAxis">The world axis to align to, for example an object's "transform.right".</param>
    /// <returns></returns>
    static Vector3 AxisAlignPosition(Vector3 position, Transform origin, Vector3 worldAxis)
    {
        var movementDelta = position - origin.position;
        var axisAlignedMovement = Vector3.Project(movementDelta, worldAxis);
        return origin.position + axisAlignedMovement;
    }


    bool GetTouchedWorldPosition(Vector2 touchPos, ref Vector3 worldTouchPos)
    {
        var plane = new Plane(Vector3.up, selectedObject.transform.position);
        var ray = raycastCamera.ScreenPointToRay(touchPos);
        var dist = 0f;
        var didHit = plane.Raycast(ray, out dist);
        if (didHit)
        {
            worldTouchPos = ray.GetPoint(dist);
        }
        return didHit;
    }

    void UpdatePosition()
    {
        this.transform.position = selectedObject.transform.position;
        this.transform.rotation = selectedObject.transform.rotation;
    }

    //Late Update runs after all Updates.
    //We put it in LateUpdate as the camera may have moved in Update.
    void LateUpdate()
    {
        if (selectedObject != null)
        {
            UpdatePosition();
        }
        else
        {
            Hide();
        }
    }
}
