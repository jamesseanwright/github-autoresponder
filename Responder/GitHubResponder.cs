using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GitHubAutoresponder.Webhook;
using Microsoft.Net.Http.Headers;

namespace GitHubAutoresponder.Responder {
    public class GitHubResponder : IGitHubResponder {
        const string CONTENT_TYPE_HEADER = "application/vnd.github.v3+json";
        const string USER_AGENT_HEADER = "GitHubAutoResponder";

        private IResponseFactory responseFactory;
        private HttpClient httpClient;
        private IJsonSerialiser jsonSerialiser;

        public GitHubResponder(
            IResponseFactory responseFactory,
            HttpClient httpClient,
            IJsonSerialiser jsonSerialiser
        ) {
            this.httpClient = httpClient;
            this.jsonSerialiser = jsonSerialiser;
            this.responseFactory = responseFactory;

            this.httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, USER_AGENT_HEADER);
            this.httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, "Basic amFtZXNzZWFud3JpZ2h0OjFmYmJkYzBmZmJlMjA3ZWI3ZGU5MDQyYmEyMWM3YWYzM2U5NjAxN2Q=");
        }

        async Task<bool> IGitHubResponder.RespondAsync(Payload payload) {
            Response body = this.responseFactory.CreateFromPayload(payload);
            string serializedResponse = this.jsonSerialiser.Serialise(body);
            StringContent content = new StringContent(serializedResponse);

            content.Headers.Remove(HeaderNames.ContentType); // remove default Content-Type header
            content.Headers.Add(HeaderNames.ContentType, CONTENT_TYPE_HEADER);

            HttpResponseMessage response = await this.httpClient.PostAsync(
                payload.Commentable.CommentsUrl,
                content
            );

            string b = await response.Content.ReadAsStringAsync();

            return (int) response.StatusCode < 400;
        }
    }
}
