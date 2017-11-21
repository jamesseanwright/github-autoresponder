using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using GitHubAutoresponder.Responder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;

namespace GitHubAutoresponder.Webhook.Tests {
    public class WebhookControllerTests : IDisposable {
        private Mock<IGitHubResponder> gitHubResponder;
        private WebhookController webhookController;

        public WebhookControllerTests() {
            this.gitHubResponder = new Mock<IGitHubResponder>();
            this.webhookController = new WebhookController(this.gitHubResponder.Object);

            this.webhookController.ControllerContext = new ControllerContext(
                new ActionContext(
                    new DefaultHttpContext(),
                    new RouteData(),
                    new ControllerActionDescriptor()
                )
            );
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

            ContentResult result = await this.webhookController.PostAsync(payload);

            Assert.StrictEqual<int>((int) HttpStatusCode.OK, this.webhookController.Response.StatusCode);
            this.gitHubResponder.Verify(g => g.RespondAsync(payload), Times.Once());
        }

        [Fact]
        public async Task ItShouldInvokeBadRequestWhenPayloadIsInvalid() {
            Payload payload = new Payload();

            this.gitHubResponder
                .Setup(g => g.RespondAsync(It.IsAny<Payload>()))
                .Returns(Task.FromResult<object>(null));

            this.webhookController.ModelState.AddModelError("key", "Some model error");

            ContentResult result = await this.webhookController.PostAsync(payload);

            Assert.StrictEqual<int?>((int) HttpStatusCode.BadRequest, this.webhookController.Response.StatusCode);
        }
    }
}
