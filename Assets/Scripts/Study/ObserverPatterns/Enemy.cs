using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Charactor
{
    public Enemy(Observer observer)
    {
        this.observer = observer;
        this.observer.Attach(this);
    }
    public override void Update(string str)
    {
        Debug.Log("observer say:" + str);
    }
}
