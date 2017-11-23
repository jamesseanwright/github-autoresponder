using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using GitHubAutoresponder.Responder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GitHubAutoresponder.Webhook {
    [Route("api/[controller]")]
    public class WebhookController : Controller {
        private IGitHubResponder gitHubResponder;
        private IModelStateConverter modelStateConverter;

        public WebhookController(IGitHubResponder gitHubResponder, IModelStateConverter modelStateConverter) {
            this.gitHubResponder = gitHubResponder;
            this.modelStateConverter = modelStateConverter;
        }

        [HttpPost]
        public async Task<ContentResult> PostAsync([FromBody]Payload payload) {
            if (!ModelState.IsValid) {
                return this.CreateValidationErrorResult();
            }

            bool isSuccessful = await this.gitHubResponder.RespondAsync(payload);

            return isSuccessful? this.CreateSuccessResult() : this.CreateUpstreamErrorResult();
        }

        private ContentResult CreateValidationErrorResult() {
            ContentResult result = Content(
                modelStateConverter.AsString(ModelState),
                MediaTypeNames.Text.Plain,
                Encoding.UTF8
            );

            result.StatusCode = (int) HttpStatusCode.BadRequest;

            return result;
        }

        private ContentResult CreateUpstreamErrorResult() {
            ContentResult result = Content(
                "The GitHub API returned an error",
                MediaTypeNames.Text.Plain,
                Encoding.UTF8
            );

            result.StatusCode = (int) HttpStatusCode.BadGateway;

            return result;
        }

        private ContentResult CreateSuccessResult() {
            ContentResult result = Content(
                "OK",
                MediaTypeNames.Text.Plain,
                Encoding.UTF8
            );

            result.StatusCode = (int) HttpStatusCode.OK;

            return result;
        }
    }
}
