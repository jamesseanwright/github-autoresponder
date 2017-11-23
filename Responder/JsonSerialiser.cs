using System;
using System.IO;
using GitHubAutoresponder.Shared;
using Newtonsoft.Json;

namespace GitHubAutoresponder.Responder {
    public class JsonSerialiser : IJsonSerialiser {
        private JsonSerializerSettings settings;

        public JsonSerialiser() {
            this.settings = new JsonSerializerSettings {
                ContractResolver = JsonContractResolver.Resolver
            };
        }

        public string Serialise(object obj) {
            return JsonConvert.SerializeObject(obj, this.settings);
        }
    }
}
