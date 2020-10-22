using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ProductFactory.Create("mouse").Sell();
        //ProductFactory.Create("cup").Get();
        //Observer observer = new Observer();
        //Enemy enemy=  new Enemy(observer);
        //Hero hero =  new Hero(observer);
        //observer.NoticeAll();
        TestFF();
    }
    void TestFF() {
        //string str = "hjellldjsakldjklsajdlkajsdkjaskldjklasjdkljaskljdl1";
        //FFactory.WriteStrToFile(str,Application.dataPath+"/Data/","zrm",()=> {
        //    Debug.Log("Write Success!");
        //});
        //FFactory.ReadStrToFile(Application.dataPath + "/Data/","zrm").Print();
        //FFactory.ReadStrToFile(Application.dataPath + "/Data/zrm").Print();
        FFactory.WriteByteToFile(FFactory.ReadImgToFile(Application.dataPath + "/colorformat.png"), Application.dataPath + "/Data/", "zrm.png",()=> {
            Debug.Log("Write Imgs Success!");
        });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
