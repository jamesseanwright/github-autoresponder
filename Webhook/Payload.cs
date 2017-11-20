namespace GitHubAutoresponder.Webhook {
    public class Payload {
        public string Action { get; set; }

        public Sender Sender { get; set; }

        // TODO: review access modifiers
        public Commentable Issue { get; set; }
        public Commentable PullRequest { get; set; }

        public Commentable Commentable {
            get {
                if (Issue != null) {
                    return Issue;
                }

                if (PullRequest != null) {
                    return PullRequest;
                }

                throw new CommentableException();
            }
        }
    }
}
