using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MaterialSwapper : MonoBehaviour
{

    //A set of materials
    // [System.Serializable]
    // public class MaterialSet
    // {
    //     //What button to set on the 3D Info Panel UI
    //     public Color buttonColor = Color.white;
    //     //The actual material asset
    //     public Material material;
    //     //The sprite for the material button
    //     public Sprite buttonIcon;
    // }

    public MaterialSet materialSet;
    //The default material set
    public  MaterialSet.MaterialSetSetup m_defaultMaterialSet;

    //Material Sets to show in the 3D Panel UI
    public  MaterialSet.MaterialSetSetup[] materialSets;

    //The current material set
    public  MaterialSet.MaterialSetSetup CurrentSet { get; private set; }

    //All renderers in this object
    MeshRenderer[] m_renderers;

    //The saved original materials.
    readonly List<List<Material>> m_originalMaterials = new List<List<Material>>();

    private void FillMaterialReferences()
    {
        materialSets = new MaterialSet.MaterialSetSetup[materialSet.materialSets.Length];
        
        for (int i = 0; i < materialSet.materialSets.Length; i++)
        {
            materialSets[i] = materialSet.materialSets[i];
        }

        //m_defaultMaterialSet = materialSet.m_defaultMaterialSet;
    }
    private void Awake()
    {
       FillMaterialReferences();
        //Get all the renderers on this object
        m_renderers = GetComponentsInChildren<MeshRenderer>();

        //Initial setup
        SaveOriginalMaterials();
        CurrentSet = m_defaultMaterialSet;
    }

    //Save our original materials on the prefab
    void SaveOriginalMaterials()
    {
        //Clear the array
        m_originalMaterials.Clear();
        //Loop through all renderers and materials
        for (int i = 0; i < m_renderers.Length; i++)
        {
            //Get each renderers materials
            var materialList = new List<Material>();
            m_renderers[i].GetMaterials(materialList);

            //Add the materials to a sublist
            m_originalMaterials.Add(materialList);
        }
    }

    //Apply a set of materials
    public void ApplyMaterialSet(MaterialSet.MaterialSetSetup set)
    {
        SetMaterial(set.material);
        this.CurrentSet = set;
    }

    //Revert to the saved originals
    public void RevertToOriginalMaterials()
    {
        for (int i = 0; i < m_renderers.Length; i++)
        {
            m_renderers[i].materials = m_originalMaterials[i].ToArray();
        }

        CurrentSet = m_defaultMaterialSet;
    }

    //Set a material for all the renderers
    public void SetMaterial(Material m)
    {
        //Loop through all the renderers
        for (int i = 0; i < m_renderers.Length; i++)
        {
            //Get the list of materials
            var materials = m_renderers[i].materials;

            //Replace each material with our new material
            for (int j = 0; j < materials.Length; j++)
            {
                materials[j] = m;
            }

            //Apply our changed material list
            m_renderers[i].materials = materials;
        }
    }

}
