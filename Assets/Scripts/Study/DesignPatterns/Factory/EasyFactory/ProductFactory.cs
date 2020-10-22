using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductFactory 
{
    public static IProduct Create(string name) {
        switch (name) {
            case "mouse":return new Mouse();
            case "cup":return new Cup();
        }
        return null;
    }
}
