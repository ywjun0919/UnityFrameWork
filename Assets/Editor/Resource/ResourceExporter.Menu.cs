

using UnityEditor;

public partial class ResourceExporter
{
    #region UI Menu

    [MenuItem("Assets/Build Selected for Windows/UI")]
    static void ExportSelectedUIsForWindows()
    {
        ExportSelectedUIs(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/Build Selected for Android/UI")]
    static void ExportSelectedUIsForAndroid()
    {
        //ForAndroidSfxProcessBug(BuildTarget.Android);
        ExportSelectedUIs(BuildTarget.Android);
    }
    [MenuItem("Assets/Build Selected for iOS/UI")]
    static void ExportSelectedUIsForiOS()
    {
        ExportSelectedUIs(BuildTarget.iOS);
    }
    #endregion

    #region Texture Menu

    [MenuItem("Assets/Build Selected for Windows/Texture")]
    static void ExportSelectedTexturesForWindows()
    {
        ExportSelectedTextures(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/Build Selected for Android/Texture")]
    static void ExportSelectedTexturesForAndroid()
    {
        ExportSelectedTextures(BuildTarget.Android);
    }
    [MenuItem("Assets/Build Selected for iOS/Texture")]
    static void ExportSelectedTexturesForiOS()
    {
        ExportSelectedTextures(BuildTarget.iOS);
    }

    [MenuItem("Build/Windows/Build Textures for Windows")]
    static void ExportAllTexturesForWindows()
    {
        ExportAllTextures(BuildTarget.StandaloneWindows);
    }


    [MenuItem("Build/Android/Build Textures for Android")]
    static void ExportAllTexturesForAndroid()
    {
        ExportAllTextures(BuildTarget.Android);
    }


    [MenuItem("Build/iOS/Build Textures for iOS")]
    static void ExportAllTexturesForiOS()
    {
        ExportAllTextures(BuildTarget.iOS);
    }
    #endregion
}
