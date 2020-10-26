using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtension 
{
    /// <summary>
    /// 字符串打印
    /// </summary>
    /// <param name="str"></param>
    public static void Print(this string str) {
        if (str!=null&&!string.IsNullOrEmpty(str)) {
            Debug.Log(str);
        }
    }
    /// <summary>
    /// 实例化object
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="parent"></param>
    public static void Create(this Object obj,Transform parent=null) {
        Object.Instantiate(obj,parent);
    }
    /// <summary>
    /// 字符串打印
    /// </summary>
    /// <param name="str"></param>
    public static void Log(this string str ) {
        Debug.Log(str);
    }
}
