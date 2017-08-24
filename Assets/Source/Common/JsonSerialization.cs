using System;
using System.IO;
using System.Text;

public static class JsonSerialization
{
    static JsonSerialization()
    {
        LitJson.JsonMapper.RegisterExporter<float>((obj, writer) => writer.Write(Convert.ToDouble(obj)));
        LitJson.JsonMapper.RegisterImporter<double, float>(Convert.ToSingle);
    }

    public static string ToString(object obj)
    {
        var writer = new LitJson.JsonWriter { PrettyPrint = true };
        LitJson.JsonMapper.ToJson(obj, writer);
        return writer.ToString();
    }

    public static string ToString(object obj, bool prettyPrint)
    {
        var writer = new LitJson.JsonWriter {PrettyPrint = prettyPrint};
        LitJson.JsonMapper.ToJson(obj, writer);
        return writer.ToString();
    }
    
    public static T ToObject<T>(string content)
    {
        return LitJson.JsonMapper.ToObject<T>(content);
    }

    public static bool ToFile(string path, object obj)
    {
        if (File.Exists(path)) { File.Delete(path); }
        var content = ToString(obj);
        File.WriteAllText(path, content, Encoding.UTF8);
        return true;
    }

    public static bool FromFile<T>(string path, out T obj)
    {
        if (!File.Exists(path))
        {
            obj = default(T);
            return false;
        }
        var content = File.ReadAllText(path);
        obj = ToObject<T>(content);
        return true;
    }
}
