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
        const string USER_AGENT_HEADER = "GitHubAutoResponder/1.0";

        private IResponseFactory responseFactory;
        private HttpClient httpClient;
        private IJsonSerialiser jsonSerialiser;

        public GitHubResponder(
            IResponseFactory responseFactory,
            HttpClient httpClient,
            IJsonSerialiser jsonSerialiser
        ) {
            // var handler = new HttpClientHandler {
            //     Proxy = new WebProxy("http://localhost:8888"),
            // };

            // handler.ClientCertificates.Add(new X509Certificate("/home/james/dev/charles-ssl.cer"));

            this.httpClient = httpClient;
            this.jsonSerialiser = jsonSerialiser;
            this.responseFactory = responseFactory;

            this.httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, USER_AGENT_HEADER);
        }

        async Task<bool> IGitHubResponder.RespondAsync(Payload payload) {
            Response body = this.responseFactory.CreateFromPayload(payload);
            Stream responseStream = this.jsonSerialiser.Serialise(body);
            StreamContent content = new StreamContent(responseStream);

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
