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
        TestAssets();
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
        allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
       
        
        audioSource.clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Prefabs/Sound/AudioTrainingSub1_1.mp3");
        audioSource.Play();
#endif
    }
    void FindAB(string assetName) {
        foreach (string s in allAssetBundleNames)
        {
            string[] s1 = AssetDatabase.GetAssetPathsFromAssetBundle(s);
            
            foreach (string s2 in s1)
            {
                print(s2);
            }
        }
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
