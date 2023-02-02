using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CheckoutSystem : MonoBehaviour
{
    public CheckoutSystem cartPanel;
    public TextMeshProUGUI checkoutText;
    public Button cartButton;
    public Button addButton;
    public Button removeButton;
    public CartItems cartItems;
    public RectTransform checkedoutItemsList;
    public GameObject itemText;
    private Dictionary<int, GameObject> checkedOutItems;
    private Dictionary<int, GameObject> itemsInList;
    private PriceTag priceTag;
    private bool isCartPanelOpen;
    private float checkoutTotal = 0f;
    private int itemsListCount = 0;
    public static int currentlySelectedItemId = 0;

    private void Start()
    {
        cartButton.onClick.AddListener(ToggleCartPanel);
        // addButton.onClick.AddListener(AddAmount);
        // removeButton.onClick.AddListener(RemoveAmount);
        addButton.onClick.AddListener(AddItemsToList);
        removeButton.onClick.AddListener(RemoveItemFromList);
        checkedOutItems = new Dictionary<int, GameObject>(10);
        itemsInList = new Dictionary<int, GameObject>(10);
        itemsListCount = cartItems.itemsList.Count;
        
    }

    public void ToggleCartPanel()
    {
        if (!isCartPanelOpen)
        {
            addButton.gameObject.SetActive(true);
            removeButton.gameObject.SetActive(true);
            checkoutText.gameObject.SetActive(true);
            checkedoutItemsList.gameObject.SetActive(true);
        }
        else
        {
            addButton.gameObject.SetActive(false);
            removeButton.gameObject.SetActive(false);
            checkoutText.gameObject.SetActive(false);
            checkedoutItemsList.gameObject.SetActive(false);
        }

        isCartPanelOpen = !isCartPanelOpen;
    }

    public void AddAmount()
    {
        // if (itemsInList.ContainsKey(currentlySelectedItemId))
        // {
        //     return;
        // }

        var checkoutString = new StringBuilder();
        checkoutTotal += cartItems.priceList[currentlySelectedItemId];
        checkoutText.text = checkoutString.Append("$ ").Append(checkoutTotal.ToString("##.##")).ToString();

        // checkedOutItems.Add(currentlySelectedItemId, cartItems.itemsList[currentlySelectedItemId]);
    }

    public void RemoveAmount()
    {
        
        
        if (checkoutTotal < 0f)
        {
            checkoutTotal = 00.00f;
        }
        else
        {
            checkoutTotal -= cartItems.priceList[currentlySelectedItemId];
        }

        var checkoutString = new StringBuilder();
        
        checkoutText.text = checkoutString.Append("$ ").Append(checkoutTotal.ToString("##.##")).ToString();

        // checkedOutItems.Remove(currentlySelectedItemId);
    }

    public void AddItemsToList()
    {
        // priceTag = itemsInList[currentlySelectedItemId].GetComponent<PriceTag>();
        
        if (itemsInList.ContainsKey(currentlySelectedItemId))
        {
            // priceTag.howManyOfThisItem++;
            // checkedOutItems.Add(currentlySelectedItemId + itemsListCount, cartItems.itemsList[currentlySelectedItemId]);
            //
            // var existingTextObj = itemsInList[currentlySelectedItemId].GetComponent<TextMeshProUGUI>();
            //
            // var checkoutString = new StringBuilder();
            //
            // existingTextObj.text = checkoutString.Append(existingTextObj.text).Append(" X ")
            //     .Append(priceTag.howManyOfThisItem.ToString()).ToString();
            //
            return;

        }
        
        // priceTag.howManyOfThisItem++;
        
        
        var textClone = Instantiate(itemText);
        var textObj = textClone.GetComponent<TextMeshProUGUI>();
        textObj.text = cartItems.itemsList[currentlySelectedItemId].gameObject.name;
        textClone.SetActive(true);
        itemsInList.Add(currentlySelectedItemId, textClone);
        checkedOutItems.Add(currentlySelectedItemId, cartItems.itemsList[currentlySelectedItemId]);
        AddAmount();

        textClone.transform.SetParent(itemText.transform.parent);
        textClone.transform.localRotation = itemText.transform.localRotation;
        textClone.transform.localScale = itemText.transform.localScale;
    }

    public void RemoveItemFromList()
    {
        // priceTag = itemsInList[currentlySelectedItemId].GetComponent<PriceTag>();

        
        
        if (!itemsInList.ContainsKey(currentlySelectedItemId))
        {
            return;
        }
        
        RemoveAmount();
        Destroy(itemsInList[currentlySelectedItemId]);
        itemsInList.Remove(currentlySelectedItemId);
        checkedOutItems.Remove(currentlySelectedItemId);
    }

    public void RemoveAllItemsFromList()
    {
        for (int i = 0; i < itemsInList.Count; i++)
        {
            Destroy(itemsInList[i].gameObject);
            itemsInList.Remove(i);
            checkedOutItems.Remove(i);
        }

        checkoutTotal = 0;
        var checkoutString = new StringBuilder();
        checkoutText.text = checkoutString.Append("$ ").Append(checkoutTotal.ToString("##.##")).ToString();
    }
}