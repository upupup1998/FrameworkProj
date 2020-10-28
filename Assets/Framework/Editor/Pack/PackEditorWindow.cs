using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace Framework {
    public class PackEditorWindow : EditorWindow
    {
        [MenuItem("Framework/Window")]
        public static void PackWindow() {
            PackEditorWindow editorWindow = EditorWindow.GetWindow(typeof(PackEditorWindow)) as PackEditorWindow;
            editorWindow.position = new Rect(1785,325,550,550);
            editorWindow.Show();

        }
        /// <summary>
        /// init 
        /// </summary>
        private void OnEnable()
        {
            //Debug.Log("Init...");
            
        }
        private void OnDestroy()
        {
            //Debug.Log("Destroy...");
            list.Clear();
            list =null;
        }
        /// <summary>
        /// GUI draw Editor window
        /// </summary>
        private void OnGUI()
        {
            list = new List<string>();
            GUILayout.Label("PackKit");
            GUILayout.BeginVertical("BOX");
           
            PackSettings.SimulateAssetBundle= GUILayout.Toggle(PackSettings.SimulateAssetBundle, LocaleText.SimulationMode);
            PackSettings.ABPath = EditorGUILayout.TextField("Package Path", PackSettings.ABPath);

            if (GUILayout.Button("Package")) {
                //打AB包
                SignAssets.PackageAbs();
                //生成config文件，AB文件索引
                Asset asset = new Asset();
              //  Debug.Log(asset.GetHashCode());
                asset.dict = new Dictionary<string, string>();
               // Debug.Log(asset.dict.Count);
                string[] allABNames = AssetDatabase.GetAllAssetBundleNames();
                foreach (string s in allABNames)
                {
                    string[] allAssetsPath = AssetDatabase.GetAssetPathsFromAssetBundle(s);

                    foreach (string s2 in allAssetsPath)
                    {
                        
                        string str1 = s2.Substring(s2.LastIndexOf("/") + 1, s2.LastIndexOf(".") - 1 - s2.LastIndexOf("/"));
                        string str2 = PackSettings.ABPath + "/" + s;
                        asset.dict.Add(str1, str2);
                    }
                }
                string str = JsonConvert.SerializeObject(asset);
                byte[] buffer = Encoding.UTF8.GetBytes(str);
                FileStream fs = File.Create(Application.streamingAssetsPath + "/Config");
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
                fs.Dispose();
             
                asset.dict.Clear();
                Debug.Log("Pack Success,Generate Config.");

            }
            if (GUILayout.Button("Clear All Package")) {
                SignAssets.ClearAbs();
            } 
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 15;
            GUILayout.Label("AssetBundles List",style);
            LoadABList();
            GUILayout.EndVertical();
            list.Clear();
        }
        /// <summary>
        /// file is mark?
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsMark(string path) {
            var re = AssetImporter.GetAtPath(path);
            DirectoryInfo info = new DirectoryInfo(path);
            if (list.Contains(path)) {
                return false;
            }
            list.Add(path);
            return string.Equals(re.assetBundleName, info.Name.Replace(".", "_").ToLower());
        }
        List<string> list;
        /// <summary>
        /// get parent folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetParentFolder(string path) {
            if (path.Equals(string.Empty)) {
                return string.Empty;
            }

            return Path.GetDirectoryName(path);
        }
        /// <summary>
        /// Load AssetBundle List
        /// </summary>
        private void LoadABList() {
            var abs = AssetDatabase.GetAllAssetBundleNames();
            foreach (string ab in abs) {
                var result = AssetDatabase.GetAssetPathsFromAssetBundle(ab);
           
                foreach (string r in result) {
                   
                    if (IsMark(r)) {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(r);
                        if (GUILayout.Button("Select", GUILayout.Width(80), GUILayout.Height(20))) {
                            Selection.objects = new[]
                                {
                                    AssetDatabase.LoadAssetAtPath<Object>(r)
                                };
                        }
                        if (GUILayout.Button("Cancel Mark", GUILayout.Width(80), GUILayout.Height(20)))
                        {
                            SignAssets.MarkAB(r);
                        }
                        
                        GUILayout.EndHorizontal();
                    }
                    if (IsMark(GetParentFolder(r))) {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(GetParentFolder(r));
                        if (GUILayout.Button("Select", GUILayout.Width(80), GUILayout.Height(20)))
                        {
                            Selection.objects = new[]
                                {
                                    AssetDatabase.LoadAssetAtPath<Object>(r)
                                };
                        }
                        if (GUILayout.Button("Cancel Mark", GUILayout.Width(80), GUILayout.Height(20)))
                        {
                            SignAssets.MarkAB(r,true);
                        }
                    
                      
                        GUILayout.EndHorizontal();
                    }
                }
         
            }
        }
        private void GetGUIStyle() {

            GUIStyle style = new GUIStyle();
            
        }
    }
    /// <summary>
    /// const text
    /// </summary>
    public class LocaleText
    {

        
        public static string SimulationMode
        {
            get
            {
                return "Simulation Mode";
            }
        }
    }
}