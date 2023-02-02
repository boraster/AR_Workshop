using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCounter : MonoBehaviour
{

    /// <summary>
    /// The text to set our counter to
    /// </summary>
    public TextMeshProUGUI text;

    public void SetItemCount(int count, int max)
    {
        this.text.text = string.Format("{0} / {1} Items", count, max);
    }

}
