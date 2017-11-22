using System.IO;
using Newtonsoft.Json;

namespace GitHubAutoresponder.Responder {
    public class JsonSerialiser : IJsonSerialiser {
        private JsonSerializer serializer;

        public JsonSerialiser() {
            this.serializer = new JsonSerializer {
                ContractResolver = JsonContractResolver.Resolver
            };
        }

        public Stream Serialise(object obj) {
            MemoryStream stream = new MemoryStream();

            StreamWriter streamWriter = new StreamWriter(stream);
            JsonTextWriter writer = new JsonTextWriter(streamWriter);

            this.serializer.Serialize(writer, obj);

            return stream;
        }
    }
}
