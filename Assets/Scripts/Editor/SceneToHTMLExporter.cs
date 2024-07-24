using System;
using System.IO;
using Editor.Serialization.Chain;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class SceneToHTMLExporter
{
    [MenuItem("Exporter/ToHTML")]
    public static void ExportScene()
    {
        var scene = SceneManager.GetActiveScene();
        var sceneName = scene.name;

        var serialization = new SerializationChain();
        var path = Path.Combine(Application.dataPath, "SceneWebDrawer/public/SceneData.json");

        StreamWriter sw = null;
        JsonTextWriter writer = null;
        try
        {
            sw = new StreamWriter(path);
            writer = new JsonTextWriter(sw);

            writer.WriteStartObject();

            writer.WritePropertyName("values");
            
            writer.WriteStartArray();

            var allSceneObjects = scene.GetRootGameObjects();
            for (var i = 0; i < allSceneObjects.Length; i++)
            {
                var obj = allSceneObjects[i];
                SerializeObject(obj, serialization, writer);
            }
            
            writer.WriteEndArray();
            writer.WriteEndObject();
            
            writer.Flush();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            writer?.Close();
        }
    }

    private static void SerializeObject(GameObject obj, SerializationChain serialization, JsonTextWriter writer)
    {
        serialization.TrySerialize(obj, writer);

        for (int i = 0; i < obj.transform.childCount; ++i)
        {
            SerializeObject(obj.transform.GetChild(i).gameObject, serialization, writer);
        }
    }
}
