// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers
{
    /// <summary>
    /// it's a base class for testing all change handler.
    /// Should inherit <see cref="OsuTestScene"/> because all change handler need the injecting to get the value.
    /// </summary>
    [HeadlessTest]
    public abstract class BaseChangeHandlerTest<TChangeHandler> : OsuTestScene where TChangeHandler : Component, new()
    {
        private TChangeHandler changeHandler;

        private int transactionCount;

        [BackgroundDependencyLoader]
        private void load()
        {
            var beatmap = new KaraokeBeatmap
            {
                BeatmapInfo =
                {
                    Ruleset = new KaraokeRuleset().RulesetInfo,
                },
            };
            var editorBeatmap = new EditorBeatmap(beatmap);
            Dependencies.Cache(editorBeatmap);
            editorBeatmap.TransactionEnded += () =>
            {
                transactionCount++;
            };

            Child = changeHandler = new TChangeHandler();
        }

        protected void TriggerHandlerChanged(Action<TChangeHandler> c)
        {
            AddStep("Trigger change handler", () =>
            {
                // should reset transaction number in here because it will increase if load testing object.
                transactionCount = 0;
                c(changeHandler);
            });
        }

        protected void AssertTransactionOnlyTriggerOnce()
        {
            AddStep("Should only trigger transaction once", () =>
            {
                Assert.AreEqual(1, transactionCount);
            });
        }
    }
}
