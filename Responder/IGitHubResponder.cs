using System.Threading.Tasks;
using GitHubAutoresponder.Webhook;

namespace GitHubAutoresponder.Responder {
    public interface IGitHubResponder {
        Task RespondAsync(Payload payload);
    }
}
