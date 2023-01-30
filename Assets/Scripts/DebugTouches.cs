using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTouches : MonoBehaviour
{

    public RectTransform debugTouchTemplate;

    RectTransform m_mouseDebugTouch;
    readonly List<RectTransform> m_debugTouchList = new List<RectTransform>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            var newDebugTouch = CloneDebugTouchTemplate();
            m_debugTouchList.Add(newDebugTouch);
        }
        m_mouseDebugTouch = CloneDebugTouchTemplate();

        debugTouchTemplate.gameObject.SetActive(false);

        Input.simulateMouseWithTouches = false;
    }

    RectTransform CloneDebugTouchTemplate()
    {
        var clonedTouch = Instantiate(debugTouchTemplate);
        clonedTouch.transform.SetParent(debugTouchTemplate.transform.parent);
        clonedTouch.transform.localPosition = Vector3.zero;
        clonedTouch.transform.localRotation = Quaternion.identity;
        clonedTouch.transform.localScale = Vector3.one;
        return clonedTouch.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            SetDebugTouch(m_mouseDebugTouch, Input.mousePosition);
        }
        else
        {
            m_mouseDebugTouch.gameObject.SetActive(false);
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            var debugTouch = m_debugTouchList[i];
            if (i < m_debugTouchList.Count)
            {
                SetDebugTouch(debugTouch, touch.position);
            }
        }

        for (int i = Input.touchCount; i < m_debugTouchList.Count; i++)
        {
            m_debugTouchList[i].gameObject.SetActive(false);
        }

    }

    void SetDebugTouch(RectTransform rect, Vector2 position)
    {
        var touchPositionInCanvas = Vector2.zero;
        var inCanvas = RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.parent as RectTransform, position, null, out touchPositionInCanvas);
        if (inCanvas)
        {
            rect.anchoredPosition = touchPositionInCanvas;
            rect.gameObject.SetActive(true);
        }
    }
}
