using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxItemPopup : MonoBehaviour
{

    public Button okButton;

    private void Awake()
    {
        okButton.onClick.AddListener(OKButtonPressed);
    }

    void OKButtonPressed()
    {
        Hide();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }


}
