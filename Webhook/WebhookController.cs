using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task PostAsync([FromBody]Payload payload) {
            await this.gitHubResponder.RespondAsync(payload);
            Ok();
        }
    }
}
