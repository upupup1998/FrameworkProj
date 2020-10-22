using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Charactor {
    protected Observer observer;
    public abstract  void Update(string str);
}
