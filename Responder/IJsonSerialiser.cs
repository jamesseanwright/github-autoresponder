using System.IO;

namespace GitHubAutoresponder.Responder {
    public interface IJsonSerialiser {
        string Serialise(object obj);
    }
}
