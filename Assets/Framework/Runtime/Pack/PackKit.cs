using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework { 
    public class PackKit : MonoSingleton<PackKit>
    {
        
        private static bool init = false;
        string[] allAssetBundleNames;
        private Dictionary<string, UnityEngine.Object> prefabDict;
        /// <summary>
        /// 初始化PackKit
        /// </summary>
        public static void Init() {
            if (init) return;
            init = true;
            Instance.InitPack();
        }
        private void InitPack()
        {
#if UNITY_EDITOR
            //勾选了模拟模式，直接在editor模式下加载资源
            if (PackSettings.SimulateAssetBundle)
            {
                allAssetBundleNames = UnityEditor.AssetDatabase.GetAllAssetBundleNames();
                prefabDict = new Dictionary<string, UnityEngine.Object>();
                Instance.AddPrefabDict();
            }
#endif
            //在ab包中加载资源
            else
            {

            }
        }
        /// <summary>
        /// 同步加载资源，图片，音乐
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T LoadAssetSync<T>(string assetName, Action action = null) where T : UnityEngine.Object
        {
            if (Instance.FindAsset<T>(assetName)!=null) return default(T);
            return Instance.FindAsset<T>(assetName, action);
        }

        /// <summary>
        /// 同步加载资源Prefabs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static System.Object LoadPrefabSync(string assetName,Action action=null) {
            if (string.IsNullOrEmpty(Instance.FindAB(assetName))) return null;
            return  Instance.CreateAB(assetName, action);
        }
        /// <summary>
        ///通过遍历ab包，给定资源名来获取资源路径
        /// </summary>
        /// <param name="assetName"></param>
#if UNITY_EDITOR
        string FindAB(string assetName)
        {
            if (allAssetBundleNames.Length != 0)
            {
                foreach (string s in allAssetBundleNames)
                {
                    string[] s1 = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(s, assetName);
                    if (s1.Length != 0)
                    {
                        return s1[0];
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            else
            {
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过资源名字直接获取该资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        System.Object CreateAB(string assetName,Action action=null)
        {
            if (allAssetBundleNames.Length != 0)
            {
                foreach (string s in allAssetBundleNames)
                {
                    string[] s1 = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(s, assetName);
                    if (s1.Length != 0)
                    {
                        if (action!=null) {
                            action();
                        }
                        return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s1[0]);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// 通过资源名字直接获取资源，图片，音乐
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        T FindAsset<T>(string assetName, Action action = null) where T : UnityEngine.Object
        {
            if (allAssetBundleNames.Length != 0)
            {
                foreach (string s in allAssetBundleNames)
                {
                    string[] s1 = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(s, assetName);
                    if (s1.Length != 0)
                    {
                        if (action != null)
                        {
                            action();
                        }

                        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(s1[0]);
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
            else
            {
                return default(T);
            }
            return default(T);
        }
        /// <summary>
        /// 添加prefab到引用中
        /// </summary>
        private void AddPrefabDict() {
            if (allAssetBundleNames.Length != 0)
            {
                foreach (string s in allAssetBundleNames)
                {
                    //string[] s1 = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(s, assetName);
                    //if (s1.Length != 0)
                    //{
                        

                    
                    //}
                    //else
                    //{
                       
                    //}
                }
            }
        }

#endif


    }
}
/*
 AssetDatabase类：对资源进行读取的类
 不能被实例化，可用来读取asset，只能在editor下使用
     方法：
     GetAllAssetBundleNames() 获取所有的ab包的名字
     GetAssetPathsFromAssetBundle(string abName) 获取ab包中的所有资源路径
     GetAssetPathsFromAssetBundleAndAssetName(string abName,string assetName)给定ab包名，资源名字，获取到资源的路径
     LoadAssetAtPath<T>(string assetPath)指定要加载的资源类型T,给定资源的路径，加载资源
     GetAssetBundleDependencies()获取资源引用（暂无使用）

模拟模式加载策略：
1.加载Prefabs类型的资源
游戏开始时预先加载好，存放在字典中，需要使用的时候再从字典中取出使用。
2.加载非Prefabs类型的资源 如：音乐，图片，shader，脚本
直接使用PackKit.LoadAssetSync方法进行加载
     */
