using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Framework {
    public  class FrameworkRun : MonoSingleton<FrameworkRun>
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
        public void OnDestroy()
        {
            print("Destroy!");
        }

    }
}