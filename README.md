# GitHub Autoresponder

An ASP.NET Core webhook for automatically responding to GitHub issues and PRs.

![Demo screen capture](https://raw.githubusercontent.com/jamesseanwright/github-autoresponder/master/DocImages/screencap.gif)


## Try Me!

[Open an issue](https://github.com/jamesseanwright/github-autoresponder/issues/new) against this repository to receive an automated response.


## Setup

* [Install .NET Core](https://www.microsoft.com/net/learn/get-started)
* `git clone https://github.com/jamesseanwright/github-autoresponder.git`
* `cd github-autoresponder`
* `dotnet restore`

Once the dependencies have been installed, one can run:

* `dotnet xunit` - execute the unit tests
* `dotnet start` - start the server

The sole exposed endpoint can be found at `/api/webhook`, which:

* responds to `HTTP POST`
* accepts GitHub [webhook-conforming payloads](https://developer.github.com/webhooks/#example-delivery)
* validates requests based upon the signature found in the `X-Hub-Signature` header


### Environment Variables

This service requires two environment variables to function:

#### `GHAR_SECRET`

The [secret token](https://developer.github.com/webhooks/securing/#setting-your-secret-token) specified when registering the webhook in your repository's settings.

#### `GHAR_CREDENTIALS`

A **Base64-encoded** string following a `<responding_username>:<personal_api_token>` format e.g. `"jamesseanwright:somerandomsha1"`

* `responding_username` - the username used to respond to issues and PRs
* `personal_api_token` - the [personal API token](https://github.com/blog/1509-personal-api-tokens) to be used, along with the username, to authenticate requests made to the GitHub API
  * **Note** that the only required scope is _public\_repo_


### Registering the Webhook Against a Repository

![Registering a webhook](https://raw.githubusercontent.com/jamesseanwright/github-autoresponder/master/DocImages/register-webhook.gif)

1. Go to one of your repositories
2. Click _Settings_
3. Click _Webhooks_
4. In _Payload URL_, enter the absolute URL via which the webhook can be accessed, including the `/api/webhook` path
5. Select _application/json_ from the _Content type_ dropdown
6. Enter the webhook secret that you most likely generated in the previous section (see _`GHAR_SECRET`_)
7. Under _Which events would you like to trigger this webhook?_, choose _Let me select individual events_, and select: _Issues_; and _Pull request_
8. Click _Add webhook_ to complete the registration


### Changing the Response Message

Currently, the response message is a hard-coded string that lives in the [`ResponseFactory` class](https://github.com/jamesseanwright/github-autoresponder/blob/master/Responder/ResponseFactory.cs). Eventually, this will be separated into a Markdown file to be read at startup.
