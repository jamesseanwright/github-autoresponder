using GitHubAutoresponder.Responder;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubAutoresponder {
    public static class Dependencies {
        public static void Inject(IServiceCollection services) {
            services.AddSingleton(typeof (IGitHubResponder), typeof (GitHubResponder));
            services.AddSingleton(typeof (IResponseFactory), typeof (ResponseFactory));
        }
    }
}
