using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartItems : MonoBehaviour
{
    public PrefabPicker prefabPicker;
    public Dictionary<int, GameObject> itemsList;
    public Dictionary<int, float> priceList;
    private PriceTag priceTag;


    private void Awake()
    {
        FillOutDictionaries();
    }
    
    private void FillOutDictionaries()
    {
    
        
        itemsList = new Dictionary<int, GameObject>(prefabPicker.prefabList.Count);
        priceList = new Dictionary<int, float>(prefabPicker.prefabList.Count);
    
        for (int i = 0; i < prefabPicker.prefabList.Count; i++)
        {
            itemsList.Add(i, prefabPicker.prefabList[i]);
            priceList.Add(i, prefabPicker.prefabList[i].GetComponent<PriceTag>().price);
        }
    }
    // private void Start()
    // {
    //
    //     StartCoroutine(FillOutDictionaries());
    // }

    // private IEnumerator FillOutDictionaries()
    // {
    //
    //     yield return new WaitForSeconds(1.0f);
    //     itemsList = new Dictionary<int, GameObject>(prefabPicker.prefabList.Count);
    //     priceList = new Dictionary<int, float>(prefabPicker.prefabList.Count);
    //
    //     for (int i = 0; i < prefabPicker.prefabList.Count; i++)
    //     {
    //         itemsList.Add(i, prefabPicker.prefabList[i]);
    //         priceList.Add(i, prefabPicker.prefabList[i].GetComponent<PriceTag>().price);
    //     }
    // }
}
