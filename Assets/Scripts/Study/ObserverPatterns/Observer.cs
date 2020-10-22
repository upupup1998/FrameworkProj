using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer 
{
    public Observer() {
        charactors = new List<Charactor>();
    }
    private List<Charactor> charactors;

    public void Attach(Charactor charactor) {
        charactors.Add(charactor);
    }
    /// <summary>
    /// 广播
    /// </summary>
    public void NoticeAll() {
        foreach (Charactor c in charactors) {
            c.Update("hello ! welcome to game!");
        }
    }
}
