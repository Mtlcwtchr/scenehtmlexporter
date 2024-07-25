using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Editor.Serialization
{
    public interface ISerializationStrategy
    {
        bool TrySerialize(GameObject obj, JsonWriter writer);
    }
}