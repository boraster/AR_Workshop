using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class PriceTag : MonoBehaviour
{
    // public ItemPrice itemPriceTag;
    public float price ;
    public int itemId;
    public int howManyOfThisItem = 0;
    
    public float CalculatePrice()
    {
       return price = Random.Range(20.00f, 200.00f);
    }
}
