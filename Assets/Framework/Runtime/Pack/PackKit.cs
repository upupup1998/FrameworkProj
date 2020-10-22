using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Framework { 
    public class PackKit : MonoSingleton<PackKit>
    {
        private static bool init = false;
        /// <summary>
        /// 初始化PackKit
        /// </summary>
        public static void Init() {
            if (init) return;
            init = true;
            Instance.InitPack();
        }
        private void InitPack() {
#if UNITY_EDITOR
            if (PackSettings.SimulateAssetBundle)
            {
                //将ab包内的文件加入到索引中
            }
#endif
            else {

            }
        }
    }
}