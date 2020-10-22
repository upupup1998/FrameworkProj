using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : IProduct
{
    public void Get()
    {
        Debug.Log("you get  a Cup");
    }

    public void Sell()
    {
        Debug.Log("you sell  a Cup");
    }
}
