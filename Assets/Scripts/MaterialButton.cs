using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{

    public Button button;
    public Image buttonIcon;
    public Image originalLabel;
    public CanvasGroup highlightGroup;
    public System.Action onButtonClicked;

    private void Awake()
    {
        button.onClick.AddListener(() => { onButtonClicked?.Invoke(); });
    }

    public void ApplyMaterialSet(MaterialSet.MaterialSetSetup set)
    {
        buttonIcon.overrideSprite = set.buttonIcon;
        buttonIcon.color = set.buttonColor;
        
    }

}
