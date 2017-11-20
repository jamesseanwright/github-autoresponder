using System;

namespace GitHubAutoresponder.Webhook {
    public class CommentableException : Exception {
        public CommentableException() : base (
            "The received event payload contained neither a link issue nor pull request"
        ) {}
    }
}
