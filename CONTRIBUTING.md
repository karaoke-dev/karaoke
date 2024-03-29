# Contributing Guidelines

Thank you for showing interest in the development of karaoke ruleset. We aim to provide a good collaborating environment for everyone involved, and as such have decided to list some of the most important things to keep in mind in the process. The guidelines below have been chosen based on past experience.

## Table of contents

1. [Reporting bugs](#reporting-bugs)
2. [Providing general feedback](#providing-general-feedback)
3. [Issue or discussion?](#issue-or-discussion)
4. [Submitting pull requests](#submitting-pull-requests)
5. [Resources](#resources)

## Reporting bugs

A **bug** is a situation in which there is something clearly *and objectively* wrong with the ruleset. Examples of applicable bug reports are:

- The ruleset crashes to desktop when I start a beatmap
- Cannot load karoake beatmap that edited before.
- The ruleset slows down a lot when I play this specific map.
- Text effect looks weird if using directX renderer.

To track bug reports, we primarily use GitHub **issues**. When opening an issue, please keep in mind the following:

- Before opening the issue, please search for any similar existing issues using the text search bar and the issue labels. This includes both open and closed issues (we may have already fixed something, but the fix hasn't yet been released).
- When opening the issue, please fill out as much of the issue template as you can. In particular, please make sure to include logs and screenshots as much as possible. The instructions on how to find the log files are included in the issue template.
- We may ask you for follow-up information to reproduce or debug the problem. Please look out for this and provide follow-up info if we request it.

If we cannot reproduce the issue, it is deemed low priority, or it is deemed to be specific to your setup in some way, the issue may be downgraded to a discussion. This will be done by a maintainer for you.

## Providing general feedback

If you wish to:

- Provide *subjective* feedback on the ruleset (about how the UI looks, about how the default skin works, about ruleset mechanics, about how the PP and scoring systems work, etc.),
- Suggest a new feature to be added to the ruleset.
- Report a non-specific problem with the ruleset that you think may be connected to your hardware or operating system specifically.

then it is generally best to start with a **discussion** first. Discussions are a good avenue to group subjective feedback on a single topic, or gauge interest in a particular feature request.

When opening a discussion, please keep in mind the following:

- Use the search function to see if your idea has been proposed before, or if there is already a thread about a particular issue you wish to raise.
- If proposing a feature, please try to explain the feature in as much detail as possible.
- If you're reporting a non-specific problem, please provide applicable logs, screenshots, or video that illustrate the issue.

If a discussion gathers enough traction, then it may be converted into an issue. This will be done by a maintainer for you.

## Issue or discussion?

We realise that the line between an issue and a discussion may be fuzzy, so while we ask you to use your best judgement based on the description above, please don't think about it too hard either. Feedback in a slightly wrong place is better than no feedback at all.

When in doubt, it's probably best to start with a discussion first. We will escalate to issues as needed.

## Submitting pull requests

The [issue tracker](https://github.com/karaoke-dev/karaoke/issues) should provide plenty of issues to start with. We also have a [`Good for contributor`](https://github.com/karaoke-dev/karaoke/issues?q=is%3Aissue+is%3Aopen+label%3A"Good+for+contributor") label.

In the case of simple issues, a direct PR is okay. However, if you decide to work on an existing issue which doesn't seem trivial, **please ask us first**. This way we can try to estimate if it is a good fit for you and provide the correct direction on how to address it. In addition, note that while we do not rule out external contributors from working on roadmapped issues, we will generally prefer to handle them ourselves unless they're not very time sensitive.

If you'd like to propose a subjective change to one of the visual aspects of the ruleset, or there is a bigger task you'd like to work on, but there is no corresponding issue or discussion thread yet for it, **please open a discussion or issue first** to avoid wasted effort.

Aside from the above, below is a brief checklist of things to watch out when you're preparing your code changes:

- Make sure you're comfortable with the principles of object-oriented programming, the syntax of `C\#` and your `development environment`.
- Make sure you are familiar with [git](https://git-scm.com/) and [the pull request workflow](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/proposing-changes-to-your-work-with-pull-requests).
- Discuss with us first if you're planning to work on a `bigger change`, or `it's UI/UX related`.
- Please do not make code changes via the GitHub web interface.
- Please add tests for your changes. We expect most new features and bugfixes to have test coverage, unless the effort of adding them is prohibitive. The visual testing methodology we use is described in more detail [here](https://github.com/ppy/osu-framework/wiki/Development-and-Testing).

After you're done with your changes and you wish to open the PR, please observe the following recommendations:

- Please submit the pull request from a [topic branch](https://git-scm.com/book/en/v2/Git-Branching-Branching-Workflows#_topic_branch) (not `master`), and keep the *Allow edits from maintainers* check box selected, so that we can push fixes to your PR if necessary.
- Please write the commit messages with useful information in mind. We recommend reading [this article](https://chris.beams.io/posts/git-commit/) for some good tips.
- Unlike osu! project, `force-push` is allowed in here. Use it if you need to.
- Rebase the PR branch to the `master` if PR is too old or has conflicts.

We are highly committed to quality when it comes to the karaoke ruleset project.
This means that contributions from less experienced community members can take multiple rounds of review to get to a mergeable state.

If you're uncertain about some part of the codebase or some inner workings of the karaoke ruleset, please reach out either by leaving a comment in the relevant issue, discussion, or PR thread, or by posting a message in the [development Discord server](https://discord.gg/ga2xZXk).
We will try to help you as much as we can.

## Resources

- [`ppy/osu`](https://github.com/ppy/osu): The game that karaoke ruleset is running on.
- [`ppy/osu` wiki](https://github.com/ppy/osu/wiki): Contains articles about various technical aspects of the game
- [`ppy/osu-framework`](https://github.com/ppy/osu-framework): The game framework that karaoke ruleset is running on.
- [`ppy/osu-framework` wiki](https://github.com/ppy/osu-framework/wiki): Contains introductory information about osu!framework, the bespoke 2D game framework we use for the game.
.
