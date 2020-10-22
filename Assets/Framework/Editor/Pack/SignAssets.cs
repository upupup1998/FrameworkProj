using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Framework;
public class SignAssets 
{
    //标记ab包
    [MenuItem("Assets/SignAB")]
    public static void Abs()
    {
        MarkAB(GetSelectedPath());
    }
    public static string GetSelectedPath()
    {
        var path = string.Empty;
        foreach (var obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                return path;
            }
        }

        return path;
    }
    public static void MarkAB(string path,bool isFolder=false)
    {
        Debug.Log("MarkABPath="+path);
        if (!string.IsNullOrEmpty(path))
        {
            if (isFolder)
            {
                path = Path.GetDirectoryName(path);
            }
            else {
               
            }
            var ai = AssetImporter.GetAtPath(path);
            var dir = new DirectoryInfo(path);


            if (ai.assetBundleName == "" && ai.assetBundleVariant == "")
            {
                ai.assetBundleName = dir.Name.Replace(".", "_");
                ai.assetBundleVariant = "";
                Debug.Log("标记" + ai.assetBundleName + "成功");
            }
            else
            {
                Debug.Log("取消标记" + ai.assetBundleName);
                ai.assetBundleVariant = "";
                ai.assetBundleName = "";


            }

            AssetDatabase.RemoveUnusedAssetBundleNames();
        }
    }
   // [MenuItem("Ackerman/Tools/打包")]
    public static void PackageAbs()
    {
        string directory = Application.streamingAssetsPath + "/TestPackage";

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Debug.Log("目录=" + directory);
        }
        AssetBundleManifest assetBundleManifest = BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        if (assetBundleManifest != null)
        {
            Debug.Log("ab打包完成!");
        }
        // DirectoryInfo代表文件夹的一个类 可实例化 ,Dircetory 静态类 不可实例化
        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        FileInfo[] files = directoryInfo.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Name.EndsWith("manifest") || file.Name.StartsWith("TestPackage"))
            {
                File.Delete(file.FullName);
            }
        }
    }
   // [MenuItem("Ackerman/Tools/清除AB包")]
    public static void ClearAbs()
    {
        string directory =PackSettings.ABPath;
        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        FileInfo[] files = directoryInfo.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i].FullName);
        }
        Debug.Log("清除AB包完成!");
    }
}
