using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class ShopData
{
    public List<bool> owned = new List<bool>();
    public List<bool> active = new List<bool>();
    public int diamondsAmount;



    public ShopData(List<bool> newOwned, List<bool> newActive)
    {
        this.owned = new List<bool>(newOwned);
        this.active = new List<bool>(newActive);
    }

    public ShopData()
    {
        int numberOfItems = ItemSelector.instance.transform.childCount;
        for (int i = 0; i < numberOfItems; i++)
        {
            owned.Add(false);
            active.Add(false);
        }
    }

}
