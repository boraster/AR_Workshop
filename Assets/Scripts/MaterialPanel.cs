using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPanel : MonoBehaviour
{

    public MaterialButton materialButtonTemplate;

    public bool IsShowing { get; private set; }
    public System.Action<MaterialSwapper.MaterialSet> OnMaterialSetSelected { get; set; }

    readonly List<MaterialButton> m_materialButtons = new List<MaterialButton>();

    private void Start()
    {
        materialButtonTemplate.gameObject.SetActive(false);
    }

    public void Hide()
    {
        IsShowing = false;
        this.gameObject.SetActive(false);
    }

    public void Show(MaterialSwapper materialSwapper)
    {
        ShowMaterialButtons(materialSwapper);
        IsShowing = true;
        this.gameObject.SetActive(true);
    }

    public void ShowMaterialButtons(MaterialSwapper materialSwapper)
    {

        var materialSetCount = materialSwapper.materialSets.Length;

        //Create more buttons if needed.
        //Plus one for the original set button
        var buttonsToCreate = materialSetCount - m_materialButtons.Count + 1;
        for (int i = 0; i < buttonsToCreate; i++)
        {
            m_materialButtons.Add(CreateMaterialButton());
        }

        var defaultMatBtn = m_materialButtons[0];
        defaultMatBtn.ApplyMaterialSet(materialSwapper.m_defaultMaterialSet);
        defaultMatBtn.onButtonClicked = () => { SelectDefaultMaterial(materialSwapper); };
        defaultMatBtn.gameObject.SetActive(true);

        //Enable the buttons and assign the information to them
        for (int i = 0; i < materialSetCount; i++)
        {
            var materialSet = materialSwapper.materialSets[i];
            var matBtn = m_materialButtons[i + 1];
            matBtn.ApplyMaterialSet(materialSet);
            matBtn.onButtonClicked = () => { SelectMaterial(materialSwapper, materialSet); };
            matBtn.gameObject.SetActive(true);
        }


        //Disable any extra buttons
        for (int i = materialSetCount; i < m_materialButtons.Count; i++)
        {
            var matBtn = m_materialButtons[i];
            matBtn.gameObject.SetActive(false);
        }

    }

    void SelectDefaultMaterial(MaterialSwapper materialSwapper)
    {
        materialSwapper.RevertToOriginalMaterials();
    }

    void SelectMaterial(MaterialSwapper materialSwapper, MaterialSwapper.MaterialSet set)
    {
        materialSwapper.ApplyMaterialSet(set);
        OnMaterialSetSelected?.Invoke(set);
    }


    MaterialButton CreateMaterialButton()
    {
        var newMaterialButton = Instantiate(materialButtonTemplate).GetComponent<MaterialButton>();
        newMaterialButton.transform.SetParent(materialButtonTemplate.transform.parent);
        newMaterialButton.transform.localPosition = materialButtonTemplate.transform.localPosition;
        newMaterialButton.transform.localRotation = materialButtonTemplate.transform.localRotation;
        newMaterialButton.transform.localScale = materialButtonTemplate.transform.localScale;
        return newMaterialButton;
    }
}
