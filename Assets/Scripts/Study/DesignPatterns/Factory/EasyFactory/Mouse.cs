using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : IProduct
{
    public void Get()
    {
        Debug.Log("you get  a mouse");
    }

    public void Sell()
    {
        Debug.Log("you sell  a mouse");
    }
}
