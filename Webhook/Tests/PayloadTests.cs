using Xunit;
using GitHubAutoresponder.Webhook.Tests;
using System;

namespace GitHubAutoresponder.Webhook.Tests {
    public class PayloadTests
    {
        [Fact]
        public void CommentableShouldReturnIssue() {
            Commentable issue = new Commentable();

            Payload payload = new Payload {
                Issue = issue
            };

            Assert.StrictEqual<Commentable>(issue, payload.Commentable);
        }

        [Fact]
        public void CommentableShouldReturnPullRequestIfIssueIsNull() {
            Commentable pullRequest = new Commentable();

            Payload payload = new Payload {
                PullRequest = pullRequest
            };

            Assert.StrictEqual<Commentable>(pullRequest, payload.Commentable);
        }

        [Fact]
        public void CommentableShouldThrowIfIssueAndPullRequestAreBothNull() {
            Payload payload = new Payload();

            Assert.Throws<CommentableException>(() => payload.Commentable);
        }
    }
}
