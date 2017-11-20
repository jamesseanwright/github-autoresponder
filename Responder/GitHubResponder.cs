using System;
using System.Threading.Tasks;
using GitHubAutoresponder.Webhook;

namespace GitHubAutoresponder.Responder {
    public class GitHubResponder : IGitHubResponder {
        private IResponseFactory responseFactory;

        public GitHubResponder(IResponseFactory responseFactory) {
            this.responseFactory = responseFactory;
        }

        async Task IGitHubResponder.RespondAsync(Payload payload) {
            await Task.Factory.StartNew(() => {});
        }
    }
}
