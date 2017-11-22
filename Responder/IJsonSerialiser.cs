using System.IO;

namespace GitHubAutoresponder.Responder {
    public interface IJsonSerialiser {
        Stream Serialise(object obj);
    }
}
