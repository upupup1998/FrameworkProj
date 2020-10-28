using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;
using System.Text;
namespace Framework {
    public struct Asset
    {
        public Dictionary<string, string> dict;
    }
    public class PackKit : Singleton<PackKit>
    {

        private readonly string abPath = Application.streamingAssetsPath;//AB放的路径
        private static bool init = false;
        string[] allAssetBundleNames;
        private bool isInitDict = false;
        private Dictionary<string, UnityEngine.Object> prefabDict;
        private MonoBehaviour _mono=null;
        private Asset asset;
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
                prefabDict = new Dictionary<string, UnityEngine.Object>();

            }
            else
#endif
            {
                //初始化
                prefabDict = new Dictionary<string, UnityEngine.Object>();
                byte[] buffer = File.ReadAllBytes(abPath + "/Config");
                string str = Encoding.UTF8.GetString(buffer);
                asset = JsonConvert.DeserializeObject<Asset>(str);
            }
        }

        /// <summary>
        /// 根据路径，文件名字加载Asset
        /// </summary>
        /// <param name="文件路径"></param>
        /// <param name="文件名"></param>
        /// <returns></returns>
        private T GetAsset<T>(string packName,string fileName) where T:UnityEngine.Object {
            
            AssetBundle ab = AssetBundle.LoadFromFile(packName);
            UnityEngine.Object obj = ab.LoadAsset<T>(fileName);
            ab.Unload(false);
            ab = null;
            return (T)obj;
        }



        /// <summary>
        /// 预加载资源Prefabs,list=null时就默认添加所有的prefab，否则添加指定的prefab
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void AddPrefabLoader(List<string> list=null) {

#if UNITY_EDITOR
            //编辑器环境下预加载
            if (PackSettings.SimulateAssetBundle)
            {
                if (list == null)
                {
                    InitPrefabDict();
                }
                else
                {
                    InitPrefabDict(list);
                }
            }
            else
#endif
            { 
                //非编辑器环境下预加载
                if (list != null)
                {
                    string listPrefabs = "";
                    foreach (string s in list)
                    {
                        if (asset.dict.ContainsKey(s))
                        {
                            // Debug.Log("key:" + s + ",value:" + asset.dict[s]);
                            // key = "Cube" , "value = "Assets/StreamingAssets/prefabs （Dict存放格式）
                            //"fire":"F:/Ackerman/FrameworkProj/Assets/StreamingAssets/art" （Dict存放格式 : 文件名 + 文件路径）
                            prefabDict.Add(s, GetAsset<UnityEngine.Object>(asset.dict[s], s));
                            listPrefabs += s+",";
                        }
                    }
                    Debug.Log("预加载Prefabs"+listPrefabs+"成功");
                }
                else
                {
                    //暂无
                }
            }

        }

        private string GetABPathInGame(string str) {
            if (asset.dict.ContainsKey(str)) {
                return asset.dict[str];
            }
            return string.Empty;
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

        /// <summary>
        /// 同步加载资源，图片，音乐,预制体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public T LoadAssetSync<T>(string assetName, Action action = null) where T : UnityEngine.Object
        {
            
#if UNITY_EDITOR
            if (PackSettings.SimulateAssetBundle)
            {
                var obj = FindAsset<T>(assetName, action);
                if (obj == null) return default(T);
                return obj;
            }
            else
#endif
            {
                if (typeof(T) == typeof(UnityEngine.Object))
                {
                    if (prefabDict.ContainsKey(assetName))
                    {
                        return (T)prefabDict[assetName];
                    }
                    else {
                        string pathAB = GetABPathInGame(assetName);
                        return GetAsset<T>(assetName, pathAB);
                    }
                }
                else {
                    string pathAB = GetABPathInGame(assetName);
                    return GetAsset<T>(pathAB, assetName);
                }
            }
        }

#if UNITY_EDITOR


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
        /// 添加所有的prefab到引用中
        /// </summary>
        private void InitPrefabDict() {
           
            if (allAssetBundleNames.Length != 0)
            {
                foreach (string abName in allAssetBundleNames)
                {
                    
                    string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(abName);
                    foreach (string assetPath in assetPaths) {
                        if (assetPath.EndsWith("prefab") &&!prefabDict.ContainsKey(assetPath)) {
                            prefabDict.Add(assetPath,UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath));
                        }
                    }
                 
                }
            }
           // isInitDict = true;
        }

        /// <summary>
        /// 添加指定的prefab到引用中
        /// </summary>
        /// <param name="list"></param>
        private void InitPrefabDict(List<string> list)
        {
            if (allAssetBundleNames.Length != 0)
            {
                foreach (string abName in allAssetBundleNames)
                {

                    string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(abName);
                    foreach (string assetPath in assetPaths)
                    {
                        for (int i=0;i<list.Count;i++) {
                        
                            if (assetPath.EndsWith(list[i]+".prefab")&& !prefabDict.ContainsKey(assetPath))
                            {
                                    prefabDict.Add(assetPath, UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath));
                            }
                        }
                        
                    }

                }
            }
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
    打包生成Config文件：
    JSON格式如下：
       // key = "Cube" , "value = "Assets/StreamingAssets/prefabs （Dict存放格式）
       //"fire":"F:/Ackerman/FrameworkProj/Assets/StreamingAssets/art" （Dict存放格式 : 文件名 + 文件路径）
    加载方式：
        根据文件名去遍历config，得到存放路径，再根据路径去加载对应的资源。
        预制体加载：可以先预加载预制体，然后从容器中取出，直接使用预制体
        Compeleted，Problem：预加载给一个非预制体的值，也会进行加载（待解决）
Simulate mode 加载策略：
    1.加载Prefabs类型的资源
    游戏开始时预先加载好，存放在字典中，需要使用的时候再从字典中取出使用。Compeleted
    2.加载非Prefabs类型的资源 如：音乐，图片，shader，脚本
    直接使用PackKit.LoadAssetSync方法进行加载。Compeleted                            
     */
