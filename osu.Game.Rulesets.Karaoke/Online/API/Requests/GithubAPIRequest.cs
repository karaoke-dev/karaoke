// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Threading.Tasks;
using Octokit;
using osu.Game.Online.API;

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests;

public abstract class GithubAPIRequest<T> where T : class
{
    protected virtual GitHubClient CreateGitHubClient() => new(new ProductHeaderValue(organizationName));

    /// <summary>
    /// Invoked on successful completion of an API request.
    /// This will be scheduled to the API's internal scheduler (run on update thread automatically).
    /// </summary>
    public event APISuccessHandler<T>? Success;

    /// <summary>
    /// Invoked on failure to complete an API request.
    /// This will be scheduled to the API's internal scheduler (run on update thread automatically).
    /// </summary>
    public event APIFailureHandler? Failure;

    private readonly object completionStateLock = new();

    /// <summary>
    /// The state of this request, from an outside perspective.
    /// This is used to ensure correct notification events are fired.
    /// </summary>
    public APIRequestCompletionState CompletionState { get; private set; }

    private readonly string organizationName;

    protected GithubAPIRequest(string organizationName)
    {
        this.organizationName = organizationName;
    }

    public async Task Perform()
    {
        var client = CreateGitHubClient();

        try
        {
            var response = await Perform(client).ConfigureAwait(false);
            triggerSuccess(response);
        }
        catch (Exception e)
        {
            triggerFailure(e);
        }
    }

    protected abstract Task<T> Perform(IGitHubClient client);

    private void triggerSuccess(T response)
    {
        lock (completionStateLock)
        {
            if (CompletionState != APIRequestCompletionState.Waiting)
                return;

            CompletionState = APIRequestCompletionState.Completed;
        }

        Success?.Invoke(response);
    }

    private void triggerFailure(Exception e)
    {
        lock (completionStateLock)
        {
            if (CompletionState != APIRequestCompletionState.Waiting)
                return;

            CompletionState = APIRequestCompletionState.Failed;
        }

        Failure?.Invoke(e);
    }
}
