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
}
