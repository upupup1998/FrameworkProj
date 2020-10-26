using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TestRun : MonoBehaviour
{
    public AudioSource audioSource;
    string[] allAssetBundleNames;
    // Start is called before the first frame update
    void Start()
    {
        //ProductFactory.Create("mouse").Sell();
        //ProductFactory.Create("cup").Get();
        //Observer observer = new Observer();
        //Enemy enemy=  new Enemy(observer);
        //Hero hero =  new Hero(observer);
        //observer.NoticeAll();
        TestPackKit();
    }
    void TestPackKit()
    {
        PackKit.Instance.Init(this);
        PackKit.Instance.LoadPrefabSync("Cube").Create();
    }
    void TestAssets() {
#if UNITY_EDITOR
        //string []str= AssetDatabase.GetAssetPathsFromAssetBundle("cube");
        //string []str= AssetDatabase.GetAllAssetBundleNames();获取所有的ab包的名字
        //string []str= AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName("cube_prefab", "cube");
        //foreach (string s in str) {
        //    print(s);
        //    AssetDatabase.LoadAssetAtPath<Object>(s).Create();
        //}
        print("解包");
        allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();//获取所有的ab包的名字 ui cube sphere


        audioSource.clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Prefabs/Sound/AudioTrainingSub1_1.mp3");
        audioSource.Play();
        if (!string.IsNullOrEmpty(FindAB("cube"))) {
            AssetDatabase.LoadAssetAtPath<Object>(FindAB("cube")).Create();
        }
       
#endif
    }
    /// <summary>
    ///通过遍历ab包，给定资源名来获取资源路径
    /// </summary>
    /// <param name="assetName"></param>
    string FindAB(string assetName) {
        if (allAssetBundleNames.Length != 0)
        {
            foreach (string s in allAssetBundleNames)
            {
                string[] s1 = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(s, assetName);
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
        else {
            return string.Empty;
        }
        return string.Empty;
    }
    void TestFF() {
        //string str = "hjellldjsakldjklsajdlkajsdkjaskldjklasjdkljaskljdl1";
        //FFactory.WriteStrToFile(str,Application.dataPath+"/Data/","zrm",()=> {
        //    Debug.Log("Write Success!");
        //});
        //FFactory.ReadStrToFile(Application.dataPath + "/Data/","zrm").Print();
        //FFactory.ReadStrToFile(Application.dataPath + "/Data/zrm").Print();
        //FFactory.WriteByteToFile(FFactory.ReadImgToFile(Application.dataPath + "/colorformat.png"), Application.dataPath + "/Data/", "zrm.png",()=> {
        //    Debug.Log("Write Imgs Success!");
        //});
    }
    // Update is called once per frame
    void Update()
    {
        
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
     */
