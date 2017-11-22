using System.IO;
using Newtonsoft.Json;

namespace GitHubAutoresponder.Responder {
    public class JsonSerialiser : IJsonSerialiser {
        private JsonSerializer serializer;

        public JsonSerialiser() {
            this.serializer = new JsonSerializer();
        }

        public Stream Serialise(object obj) {
            MemoryStream stream = new MemoryStream();

            using (StreamWriter streamWriter = new StreamWriter(stream))
            using (JsonTextWriter writer = new JsonTextWriter(streamWriter)) {
                this.serializer.Serialize(writer, obj);
            }

            return stream;
        }
    }
}
