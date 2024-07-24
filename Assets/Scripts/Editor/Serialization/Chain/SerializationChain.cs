using System.Collections.Generic;
using Editor.Serialization.Implementations;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Editor.Serialization.Chain
{
    public class SerializationChain : ISerializationStrategy
    {
        private List<ISerializationStrategy> strategies;
        
        public SerializationChain()
        {
            strategies = new List<ISerializationStrategy>();

            var serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.NullValueHandling = NullValueHandling.Include;

            var cameraSerialization = new CameraSerializationStrategy(serializer);
            var gameObjectSerialization = new GameObjectSerializationStrategy(serializer);

            strategies.Add(cameraSerialization);
            strategies.Add(gameObjectSerialization);
        }
        
        public bool TrySerialize(GameObject obj, JsonWriter writer)
        {
            for (var i = 0; i < strategies.Count; i++)
            {
                var strategy = strategies[i];
                
                if (strategy.TrySerialize(obj, writer))
                    return true;
            }

            return false;
        }
    }
}