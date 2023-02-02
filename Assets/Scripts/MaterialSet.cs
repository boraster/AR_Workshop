using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Material", fileName = "Material Set", order = 21)]
public class MaterialSet : ScriptableObject
{
   
    [System.Serializable]
    public class MaterialSetSetup
    {
        //What button to set on the 3D Info Panel UI
        public Color buttonColor = Color.white;
        //The actual material asset
        public Material material;
        //The sprite for the material button
        public Sprite buttonIcon;
    }
    
    public MaterialSetSetup m_defaultMaterialSet;
    public MaterialSetSetup[] materialSets;

    
}
