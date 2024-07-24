using Extensions;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Editor.Serialization.Implementations
{
    public class CameraSerializationStrategy : ISerializationStrategy
    {
        struct CameraProperties
        {
            public string type;
            public string position;
            public string eulerAngles;
            public string rot;

            public CameraProperties(string type, Vector3 position, Vector3 eulerAngles, Quaternion rot)
            {
                this.type = type;
                this.position = position.ToStr();
                this.eulerAngles = eulerAngles.ToStr();
                this.rot = rot.ToStr();
            }
        }
        
        private JsonSerializer _serializer;

        public CameraSerializationStrategy(JsonSerializer serializer)
        {
            _serializer = serializer;
        }
        
        public bool TrySerialize(GameObject obj, JsonWriter writer)
        {
            if (!obj.TryGetComponent<Camera>(out var camera))
                return false;
            
            var props = new CameraProperties(nameof(Camera), camera.transform.position, camera.transform.rotation.eulerAngles, camera.transform.rotation);

            _serializer.Serialize(writer, props);
            return true;
        }
    }
}