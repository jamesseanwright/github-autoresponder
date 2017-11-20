using System.Threading.Tasks;
using GitHubAutoresponder.Webhook;

namespace GitHubAutoresponder.Responder {
    public class GitHubResponder : IGitHubResponder {
        async Task IGitHubResponder.RespondAsync(Payload payload) {
            await Task.Factory.StartNew(() => {});
        }
    }
}
