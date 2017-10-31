using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using GitHubAutoresponder.Responder;

namespace GitHubAutoresponder.Webhook.Tests {
    public class WebhookControllerTests : IDisposable {
        private Mock<IGitHubResponder> gitHubResponder;
        private WebhookController webhookController;

        public WebhookControllerTests() {
            this.gitHubResponder = new Mock<IGitHubResponder>();
            this.webhookController = new WebhookController(this.gitHubResponder.Object);
        }

        public void Dispose() {
            this.gitHubResponder = null;
        }

        [Fact]
        public async Task ItShouldForwardThePayload() {
            Payload payload = new Payload();

            this.gitHubResponder
                .Setup(g => g.RespondAsync(It.IsAny<Payload>()))
                .Returns(Task.FromResult<object>(null));

            await this.webhookController.PostAsync(payload);

            this.gitHubResponder.Verify(g => g.RespondAsync(payload), Times.Once());
        }
    }
}
