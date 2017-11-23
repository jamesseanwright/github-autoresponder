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
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GitHubAutoresponder.Webhook.Tests {
    public class WebhookControllerTests : IDisposable {
        private Mock<IGitHubResponder> gitHubResponder;
        private Mock<IModelStateConverter> modelStateConverter;
        private WebhookController webhookController;

        public WebhookControllerTests() {
            this.gitHubResponder = new Mock<IGitHubResponder>();
            this.modelStateConverter = new Mock<IModelStateConverter>();

            this.webhookController = new WebhookController(
                this.gitHubResponder.Object,
                this.modelStateConverter.Object
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
                .ReturnsAsync(true);

            ContentResult result = await this.webhookController.PostAsync(payload);

            Assert.StrictEqual<int?>((int) HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("OK", result.Content);
            this.gitHubResponder.Verify(g => g.RespondAsync(payload), Times.Once());
        }

        [Fact]
        public async Task ItShouldRespondWithBadRequestWhenPayloadIsInvalid() {
            Payload payload = new Payload();

            this.modelStateConverter
                .Setup(m => m.AsString(this.webhookController.ModelState))
                .Returns("Model validation errors");

            this.webhookController.ModelState.AddModelError("key", "Some model error");

            ContentResult result = await this.webhookController.PostAsync(payload);

            Assert.StrictEqual<int?>((int) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Model validation errors", result.Content);
        }

        [Fact]
        public async Task ItShouldRespondWithBadGatewayWhenUpstreamReturnsError() {
            Payload payload = new Payload();

            this.gitHubResponder
                .Setup(g => g.RespondAsync(It.IsAny<Payload>()))
                .ReturnsAsync(false);

            ContentResult result = await this.webhookController.PostAsync(payload);

            Assert.StrictEqual<int?>((int) HttpStatusCode.BadGateway, result.StatusCode);
        }
    }
}
