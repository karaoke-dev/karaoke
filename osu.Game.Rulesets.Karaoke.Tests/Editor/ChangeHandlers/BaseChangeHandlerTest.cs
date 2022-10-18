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
        private TChangeHandler changeHandler = null!;

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
            Dependencies.CacheAs<IEditorChangeHandler>(new MockEditorChangeHandler());
            editorBeatmap.TransactionEnded += () =>
            {
                transactionCount++;
            };

            Child = changeHandler = new TChangeHandler();
        }

        protected void SetUpEditorBeatmap(Action<EditorBeatmap> action)
        {
            AddStep("Prepare testing beatmap", () =>
            {
                var editorBeatmap = Dependencies.Get<EditorBeatmap>();
                action.Invoke(editorBeatmap);
            });
        }

        protected void SetUpKaraokeBeatmap(Action<KaraokeBeatmap> assert)
        {
            SetUpEditorBeatmap(editorBeatmap =>
            {
                if (editorBeatmap.PlayableBeatmap is not KaraokeBeatmap karaokeBeatmap)
                    throw new InvalidCastException();

                assert.Invoke(karaokeBeatmap);
            });
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

        protected void TriggerHandlerChangedWithException<T>(Action<TChangeHandler> c) where T : Exception
        {
            TriggerHandlerChanged(ch =>
            {
                Assert.Catch<T>(() => c(ch));
            });
        }

        protected void AssertEditorBeatmap(Action<EditorBeatmap> assert)
        {
            AddStep("Is result matched", () =>
            {
                var editorBeatmap = Dependencies.Get<EditorBeatmap>();
                assert(editorBeatmap);
            });

            // even if there's no property changed in the lyric editor, should still trigger the change handler.
            // because every change handler call should cause one undo step.
            // also, technically should not call the change handler if there's no possible to change the properties.
            AssertTransactionOnlyTriggerOnce();
        }

        protected void AssertKaraokeBeatmap(Action<KaraokeBeatmap> assert)
        {
            AssertEditorBeatmap(editorBeatmap =>
            {
                if (editorBeatmap.PlayableBeatmap is not KaraokeBeatmap karaokeBeatmap)
                    throw new InvalidCastException();

                assert.Invoke(karaokeBeatmap);
            });
        }

        protected void AssertTransactionOnlyTriggerOnce()
        {
            AddStep("Should only trigger transaction once", () =>
            {
                Assert.AreEqual(1, transactionCount);
            });
        }

        private class MockEditorChangeHandler : TransactionalCommitComponent, IEditorChangeHandler
        {
            public event Action? OnStateChange;

            protected override void UpdateState()
            {
                OnStateChange?.Invoke();
            }
        }
    }
}
