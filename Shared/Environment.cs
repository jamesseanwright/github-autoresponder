namespace GitHubAutoresponder.Shared {
    public class Environment : IEnvironment
    {
        public string EncodededCredentials => System.Environment.GetEnvironmentVariable("GHAR_CREDENTIALS");
    }
}
