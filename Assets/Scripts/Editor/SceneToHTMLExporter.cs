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

            var allSceneObjects = Object.FindObjectsOfType<GameObject>();
            for (var i = 0; i < allSceneObjects.Length; i++)
            {
                var obj = allSceneObjects[i];
                serialization.TrySerialize(obj, writer);
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
}
