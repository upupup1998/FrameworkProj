﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework {
    interface ISingleton {
        void OnSingletonInit();
    }
    public abstract class Singleton<T> : ISingleton where T :Singleton<T> ,new()
    {
        protected static T _Instance = new T();
        private static readonly object obj = new object();
        public static T Instance
        {
            get
            {
                return _Instance;
            }
        }
        public void OnSingletonInit()
        {
           
        }
        protected virtual void Dispose()
        {
            
        }
    }
    public abstract class MonoSingleton<T> : MonoBehaviour,ISingleton where T : MonoSingleton<T>,new()
    {
        protected static T _Instance = new T();
        private static readonly object obj = new object();
        public static T Instance {
            get {
                return _Instance;
            }
        }
        public void OnSingletonInit()
        {
          
        }
        private void OnDestroy()
        {
            _Instance = null;
        }
    }
}