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
                Response.StatusCode = (int) HttpStatusCode.BadRequest;

                return Content(
                    ModelState.ToString(),
                    MediaTypeNames.Text.Plain,
                    Encoding.UTF8
                );
            }

            await this.gitHubResponder.RespondAsync(payload);

            Response.StatusCode = (int) HttpStatusCode.OK;

            return Content(
                "OK",
                MediaTypeNames.Text.Plain,
                Encoding.UTF8
            );
        }
    }
}
