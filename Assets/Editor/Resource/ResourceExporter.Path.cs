using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public partial class ResourceExporter
{
    private static string RelativePathFmt = "{0}/../{1}/";
    public static string GetBundleSaveDir(BuildTarget target)
    {
        string path;
        switch (target)
        {
            case BuildTarget.Android:
            case BuildTarget.iOS:
                path = string.Format(RelativePathFmt, Application.dataPath, GetPlatfomrPath(target));
                break;
            default:
                path = string.Format(RelativePathFmt+"Data/", Application.dataPath, GetPlatfomrPath(target));
                break;
        }
        return path;
    }
    static string GetPlatfomrPath(BuildTarget target)
    {
        string platformPath = string.Empty;
        switch (target)
        {
            case BuildTarget.Android:
                platformPath = "dist/android";
                break;
            case BuildTarget.iOS:
                platformPath = "dist/ios";
                break;
            default:
                {
                    platformPath = "GameWindows";
                }
                break;
        }
        return platformPath;
    }


}
