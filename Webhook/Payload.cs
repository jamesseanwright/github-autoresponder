using System.ComponentModel.DataAnnotations;

namespace GitHubAutoresponder.Webhook {
    public class Payload {
        [Required]
        public string Action { get; set; }

        [Required]
        public Sender Sender { get; set; }

        // TODO: review access modifiers
        [Required]
        public Repository Repository { get; set; }

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
