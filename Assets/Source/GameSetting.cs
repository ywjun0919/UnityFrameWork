using UnityEngine;
using System.Collections;

namespace Game
{
    public static class GameSetting
    {

        public const string relativePath = "{0}/../{1}/";
        public const string assetPath = "Assets/GameAssets/";
        public const string toolsPath = "Assets/GameTools/";
        public const string sourcePath = "Assets/GameSources/";

        public static string codePath { get { return string.Format(relativePath + "code/", Application.dataPath); } }
        public static string csvPath { get { return string.Format(relativePath + "csv/", Application.dataPath); } }
        public static string dataPath { get { return string.Format(relativePath + "GameWindows/Data/", Application.dataPath); } }
        public static string editorDataPath { get { return string.Format(relativePath + "GameWindows/EditorData/", Application.dataPath); } }

        public const string UI_ROOT_OBJECT_NAME = "UI Root (2D)";
        public const string UI_ROOT_NODE_NAME = "UI_Root";
        public const string UI_ROOT_CAMERA_NAME = "Camera";

        public static Vector2 defaultResolution = new Vector2(1280f, 720f);

        public const string TRANSIT_SCENE_NAME = "scene_transit";
        public const string LOGIN_SCENE_NAME = "scene_login";



        public static string[] clickLayerMask = new[]
            {"LayerNPC", "LayerMonster", "LayerPlayer", "LayerRolePlayer","LayerCharacter", "LayerGround","LayerInteraction"};

        public static string layerGround = "LayerGround";

        //依赖文件路径
        public const string dependencyFilePath = "config/dependency.json";
        //最大依赖深度
        public const int maxDependencyDepth = 2;
        //预加载资源列表
        public const string preloadListFilePath = "config/preloadlist.txt";
        //配置数据地址
        public const string cfgDataPath = "config/csv/";
        //资源数据文件
        public const string resourceDataPath = "resource/resourceconfig.data";
        //缓存池配置
        public const string poolDataPath = "resource/resourcepoolconfig.data";
        //资源加载配置
        public const string loadDataPath = "resource/loadconfig.data";
        //AvatarColor配置
        public const string avatarColorsDataPath = "resource/avatarcolors.data";
        //AvatarColor配置
        public const string settingDataPath = "setting/settingconfig.data";
        //Head
        public const string headDataPath = "role/head.data";
        //Eye
        public const string eyeDataPath = "role/eye.data";
        //每帧最大加载时间
        public const float maxLoadTimePerframe = 0.02f;
        //每大同时运行加载任务数量
        public const int maxLoadTaskCount = 10;
        //资源清理检查时间间隔
        public const float checkCleanInterval = 1f;
        //卸载无用资源间隔
        public const float unLoadUnUsedInterval = 60f;
        //Gc间隔
        public const float GcCollectInterval = 10f;


    }

}

