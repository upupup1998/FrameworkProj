using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Framework {
    public class PackKit : Singleton<PackKit>
    {
       
        private static bool init = false;
        string[] allAssetBundleNames;
        private bool isInitDict = false;
        private Dictionary<string, UnityEngine.Object> prefabDict;
        private MonoBehaviour _mono=null;
        /// <summary>
        /// 初始化PackKit
        /// </summary>
        public void Init(MonoBehaviour mono) {
            if (init) return;
            init = true;
            _mono = mono;
            InitPack();
        }
        private void InitPack(){
#if UNITY_EDITOR
            //勾选了Simulate模式，直接在editor模式下加载资源
            if (PackSettings.SimulateAssetBundle)
            {
             
                allAssetBundleNames = UnityEditor.AssetDatabase.GetAllAssetBundleNames();
                //Debug.Log(allAssetBundleNames.Length);
                prefabDict = new Dictionary<string, UnityEngine.Object>();
                InitPrefabDict();
                Debug.Log("Init PackKit Success");
                
            }
            else
#endif
            {
                ////在ab包中预加载资源
                //_mono.StartCoroutine("Load");
            }
        }

        /// <summary>
        /// 预加载资源Prefabs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void AddPrefabLoader(List<string> list) {
            //#if UNITY_EDITOR
            //            //编辑器环境下加载
            //            string abPath = FindAB(assetName);
            //            if (string.IsNullOrEmpty(abPath)) return null;
            //            return GetPrefabForPath(abPath);
            //#else
            //            //非编辑器环境下加载

            //#endif

            //_mono.StartCoroutine(Load(Application.streamingAssetsPath+ "/cube_prefab", "Cube",()=> {
            //    return prefabCallBack;
            //}));
            if (list.Count == 0) return;
            foreach (string s in list) {
                
                //_mono.StartCoroutine(Load());
            }
           
        }
        private UnityEngine.Object prefabCallBack;
        IEnumerator Load(string abPath,string fileName,Action action) {
            
            var prefab = AssetBundle.LoadFromFile(abPath);
            if (prefab==null) {
                Debug.Log("Load AB Failed.");
                yield break;//直接结束后续的操作
            }
           
            UnityEngine.Object obj = prefab.LoadAsset(fileName);
            prefabCallBack = obj;
            prefab.Unload(false);
            action();
        }
#if UNITY_EDITOR
        /// <summary>
        /// 同步加载资源，图片，音乐
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public T LoadAssetSync<T>(string assetName, Action action = null) where T : UnityEngine.Object
        {
            var obj = FindAsset<T>(assetName, action);
            if (obj == null) return default(T);
            return obj;
        }

        /// <summary>
        ///通过遍历ab包，给定资源名来获取资源路径
        /// </summary>
        /// <param name="assetName"></param>

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
        UnityEngine.Object CreateAB(string assetName,Action action=null)
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
                        Debug.Log(s1[0]);
                        if (action != null)
                        {
                            action();
                        }
                       
                        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(s1[0]);
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
        private void InitPrefabDict() {
           
            if (allAssetBundleNames.Length != 0)
            {
                foreach (string abName in allAssetBundleNames)
                {
                    
                    string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(abName);
                    foreach (string assetPath in assetPaths) {
                        if (assetPath.EndsWith("prefab")) {
                            prefabDict.Add(assetPath,UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath));
                        }
                    }
                 
                }
            }
            isInitDict = true;
        }
        /// <summary>
        /// 遍历prefabsdic，获取对应的Object
        /// </summary>
        private UnityEngine.Object GetPrefabForPath(string prefabPath) {
            if (string.IsNullOrEmpty(prefabPath)||!prefabDict.ContainsKey(prefabPath)) return null;
           
            return prefabDict[prefabPath];
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

直接从ab中加载资源：


Simulate mode 加载策略：
    1.加载Prefabs类型的资源
    游戏开始时预先加载好，存放在字典中，需要使用的时候再从字典中取出使用。Compeleted
    2.加载非Prefabs类型的资源 如：音乐，图片，shader，脚本
    直接使用PackKit.LoadAssetSync方法进行加载。Compeleted                            
     */
