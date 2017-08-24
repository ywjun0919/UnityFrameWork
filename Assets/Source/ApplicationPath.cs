using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ApplicationPath
    {
        public static string DepencyPath {
            get
            {
                return GetPath(GameSetting.dependencyFilePath);
            }
        }

        public static string GetPath(string relativePath)
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android: break;
                case RuntimePlatform.IPhonePlayer: break;
                case RuntimePlatform.WindowsEditor:
                    path = string.Format("{0}/../{1}/Data/{2}",Application.dataPath,GetDataPath(),relativePath);  break;
                default:
                    path = string.Format("{0}/../{1}/Data/{2}", Application.dataPath, GetDataPath(), relativePath); break;
            }
            return path;
        }

        private static string GetDataPath()
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:break;
                case RuntimePlatform.IPhonePlayer:break;
                case RuntimePlatform.WindowsEditor:path = "GameWindows"; break;
                default:path = "GameWindwos";break;
            }
            return path;
        }
     
    }
}

