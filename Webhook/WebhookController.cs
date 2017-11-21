using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using GitHubAutoresponder.Responder;
using Microsoft.AspNetCore.Mvc;

namespace GitHubAutoresponder.Webhook {
    [Route("api/[controller]")]
    public class WebhookController : Controller {
        private IGitHubResponder gitHubResponder;

        public WebhookController(IGitHubResponder gitHubResponder) {
            this.gitHubResponder = gitHubResponder;
        }

        [HttpPost]
        public async Task<ContentResult> PostAsync([FromBody]Payload payload) {
            if (!ModelState.IsValid) {
                ContentResult errorResult = Content(
                    ModelState.ToString(),
                    MediaTypeNames.Text.Plain,
                    Encoding.UTF8
                );

                errorResult.StatusCode = (int) HttpStatusCode.BadRequest;

                return errorResult;
            }

            await this.gitHubResponder.RespondAsync(payload);

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
