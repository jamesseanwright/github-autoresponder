using GitHubAutoresponder.Responder;
using GitHubAutoresponder.Webhook;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubAutoresponder {
    public static class Dependencies {
        public static void Register(IServiceCollection services) {
            services.AddSingleton(typeof (IGitHubResponder), typeof (GitHubResponder));
            services.AddSingleton(typeof (IResponseFactory), typeof (ResponseFactory));
            services.AddSingleton(typeof (IModelStateConverter), typeof (ModelStateConverter));
        }
    }
}
