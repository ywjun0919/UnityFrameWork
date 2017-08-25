

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
}
