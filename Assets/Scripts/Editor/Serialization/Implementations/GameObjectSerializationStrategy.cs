using Extensions;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Editor.Serialization.Implementations
{
    public class GameObjectSerializationStrategy : ISerializationStrategy
    {
        struct GameObjectProperties
        {
            public string type;
            public string name;
            public string parent;
            public string position;
            public string eulerAngles;
            public string rot;
            public string scale;

            public GameObjectProperties(string type, string name, string parent, Vector3 position, Vector3 eulerAngles, Quaternion rot, Vector3 scale)
            {
                this.type = type;
                this.name = name;
                this.parent = parent;
                this.position = position.ToStr();
                this.eulerAngles = eulerAngles.ToStr();
                this.rot = rot.ToStr();
                this.scale = scale.ToStr();
            }
        }
        
        private JsonSerializer _serializer;

        public GameObjectSerializationStrategy(JsonSerializer serializer)
        {
            _serializer = serializer;
        }
        
        public bool TrySerialize(GameObject obj, JsonWriter writer)
        {
            if (!obj.TryGetComponent<MeshFilter>(out var meshFilter) || meshFilter.sharedMesh == null)
                return false;
            
            var props = new GameObjectProperties("SceneObject", obj.name, obj.transform.parent?.name, obj.transform.localPosition, obj.transform.localRotation.eulerAngles, obj.transform.localRotation, obj.transform.localScale);

            _serializer.Serialize(writer, props);
            return true;
        }
    }
}