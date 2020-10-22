using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
/// <summary>
/// Framework
/// </summary>
namespace Framework {
    public enum DataType {
        String,
        Byte,
        Int,
        Float,
        
    }
    /// <summary>
    /// 文件读取，写入
    /// </summary>
    public class FFactory
    {
        /// <summary>
        /// 将字符写入到一个新文件中
        /// </summary>
        /// <param name="str"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="action"></param>
        public static void WriteStrToFile(string str,string path,string fileName,Action action=null)
        {
            if (string.IsNullOrEmpty(str) || str == "") return;
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            byte [] buffer = Encoding.UTF8.GetBytes(str);
            FileStream fs = new FileStream(path+fileName,FileMode.OpenOrCreate);
            fs.Write(buffer,0,buffer.Length);
            fs.Close();
            fs.Dispose();
            if (action!=null) {
                action();
            }
        }
        /// <summary>
        /// 将字节写入到一个新文件中
        /// </summary>
        /// <param name="str"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="action"></param>
        public static void WriteByteToFile(byte[] str, string path, string fileName, Action action = null)
        {
            if (str.Length==0) return;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
           
            FileStream fs = new FileStream(path + fileName, FileMode.OpenOrCreate);
            fs.Write(str, 0, str.Length);
            fs.Close();
            fs.Dispose();
            if (action != null)
            {
                action();
            }
        }

        /// <summary>
        /// 读取文件中的字符
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string ReadStrToFile(string path, string fileName, Action action = null) {
          
            if (!Directory.Exists(path))
            {
                Debug.Log("File is not exist!");
                return null;
            }
            byte [] buffer = File.ReadAllBytes(path+fileName);
            if (buffer.Length==0) {
                Debug.Log("File is null!");
                return null;
            }
            if (action != null)
            {
                action();
            }
            return Encoding.UTF8.GetString(buffer);
        }
        /// <summary>
        /// 读取文件中的字符 文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string ReadStrToFile(string path,  Action action = null)
        {

            if (!File.Exists(path))
            {
                Debug.Log("File is not exist!");
                return null;
            }
            byte[] buffer = File.ReadAllBytes(path );
            if (buffer.Length == 0)
            {
                Debug.Log("File is null!");
                return null;
            }
            if (action != null)
            {
                action();
            }
            return Encoding.UTF8.GetString(buffer);
        }
        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static byte [] ReadImgToFile(string path, Action action = null)
        {

            if (!File.Exists(path))
            {
                Debug.Log("Images is not exist!");
                return null;
            }
            byte[] buffer = File.ReadAllBytes(path);
            if (buffer.Length == 0)
            {
                Debug.Log("Images is null!");
                return null;
            }
            if (action != null)
            {
                action();
            }
            return buffer;
        }
    }
}