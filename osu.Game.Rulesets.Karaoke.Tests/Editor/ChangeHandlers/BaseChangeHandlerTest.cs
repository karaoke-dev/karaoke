// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Types;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers;

/// <summary>
/// it's a base class for testing all change handler.
/// Should inherit <see cref="OsuTestScene"/> because all change handler need the injecting to get the value.
/// </summary>
[HeadlessTest]
public abstract partial class BaseChangeHandlerTest<TChangeHandler> : EditorClockTestScene where TChangeHandler : Component
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
        var editorChangeHandler = new MockEditorChangeHandler(editorBeatmap);
        Dependencies.Cache(editorBeatmap);
        Dependencies.CacheAs<IEditorChangeHandler>(editorChangeHandler);
        editorChangeHandler.TransactionEnded += () =>
        {
            transactionCount++;
        };

        Children = new Drawable[]
        {
            editorBeatmap,
            changeHandler = CreateChangeHandler(),
        };
    }

    protected virtual TChangeHandler CreateChangeHandler()
        => Activator.CreateInstance<TChangeHandler>();

    [SetUp]
    public virtual void SetUp()
    {
        AddStep("Setup", () =>
        {
            var editorBeatmap = Dependencies.Get<EditorBeatmap>();
            editorBeatmap.Clear();
            editorBeatmap.SelectedHitObjects.Clear();
        });

        // Should set-up karaoke beatmap before testing.
        // still able to call the SetUpKaraokeBeatmap or SetUpEditorBeatmap in the test case.
        SetUpKaraokeBeatmap(_ => { });
    }

    protected virtual bool IncludeAutoGenerator => false;

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
    {
        if (!IncludeAutoGenerator)
        {
            return base.CreateChildDependencies(parent);
        }

        var baseDependencies = new DependencyContainer(base.CreateChildDependencies(parent));
        baseDependencies.Cache(new KaraokeRulesetEditGeneratorConfigManager());
        return baseDependencies;
    }

    protected virtual void SetUpEditorBeatmap(Action<EditorBeatmap> action)
    {
        AddStep("Prepare testing beatmap", () =>
        {
            var editorBeatmap = Dependencies.Get<EditorBeatmap>();
            action(editorBeatmap);
        });
    }

    protected virtual void SetUpKaraokeBeatmap(Action<KaraokeBeatmap> action)
    {
        SetUpEditorBeatmap(editorBeatmap =>
        {
            var karaokeBeatmap = EditorBeatmapUtils.GetPlayableBeatmap(editorBeatmap);
            action(karaokeBeatmap);
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
            Assert.Throws<T>(() => c(ch));
        });
    }

    protected void AssertEditorBeatmap(Action<EditorBeatmap> assert)
    {
        AddStep("Is result matched", () =>
        {
            var editorBeatmap = Dependencies.Get<EditorBeatmap>();
            assert(editorBeatmap);
        });

        AssertStatus();
    }

    protected void AssertKaraokeBeatmap(Action<KaraokeBeatmap> assert)
    {
        AssertEditorBeatmap(editorBeatmap =>
        {
            var karaokeBeatmap = EditorBeatmapUtils.GetPlayableBeatmap(editorBeatmap);
            assert(karaokeBeatmap);
        });
    }

    protected void PrepareHitObject(Func<HitObject> hitObject, bool selected = true)
        => PrepareHitObjects(() => new[] { hitObject() }, selected);

    protected void PrepareHitObjects(Func<IEnumerable<HitObject>> selectedHitObjects, bool selected = true)
    {
        AddStep("Prepare testing hit objects", () =>
        {
            var hitobjects = selectedHitObjects().ToList();
            var editorBeatmap = Dependencies.Get<EditorBeatmap>();

            editorBeatmap.AddRange(hitobjects);

            if (selected)
            {
                editorBeatmap.SelectedHitObjects.AddRange(hitobjects);
            }
        });
    }

    protected void AssertHitObject<THitObject>(Action<THitObject> assert) where THitObject : HitObject
    {
        AssertHitObjects<THitObject>(hitObjects =>
        {
            foreach (var hitObject in hitObjects)
            {
                assert(hitObject);
            }
        });
    }

    protected void AssertHitObjects<THitObject>(Action<IEnumerable<THitObject>> assert) where THitObject : HitObject
    {
        AddStep("Is result matched", () =>
        {
            var editorBeatmap = Dependencies.Get<EditorBeatmap>();
            assert(editorBeatmap.HitObjects.OfType<THitObject>());
        });

        AssertStatus();
    }

    protected void AssertStatus()
    {
        // even if there's no property changed in the lyric editor, should still trigger the change handler.
        // because every change handler call should cause one undo step.
        // also, technically should not call the change handler if there's no possible to change the properties.
        AssertTransactionOnlyTriggerOnce();

        // We should make sure that the stage info is in the latest state.
        // Should trigger the beatmap editor to run the beatmap processor if not the latest.
        AssertCalculatedPropertyInStageInfoValid();

        // We should make sure that if the working property is changed by the change handler.
        // Should trigger the beatmap editor to run the beatmap processor to re-fill the working property.
        AssertWorkingPropertyInHitObjectValid();
    }

    protected void AssertTransactionOnlyTriggerOnce()
    {
        AddStep("Transaction should be only triggered once.", () =>
        {
            Assert.AreEqual(1, transactionCount);
        });
    }

    protected void AssertCalculatedPropertyInStageInfoValid()
    {
        AddWaitStep("Waiting for working property being re-filled in the beatmap processor.", 1);
        AddAssert("Check if working property in the hit object is valid", () =>
        {
            var editorBeatmap = Dependencies.Get<EditorBeatmap>();
            var karaokeBeatmap = EditorBeatmapUtils.GetPlayableBeatmap(editorBeatmap);
            if (karaokeBeatmap.CurrentStageInfo is IHasCalculatedProperty calculatedProperty)
                return calculatedProperty.IsUpdated();

            // ignore check if current stage info no need to calculate the property.
            return true;
        });
    }

    protected void AssertWorkingPropertyInHitObjectValid()
    {
        AddWaitStep("Waiting for working property being re-filled in the beatmap processor.", 1);
        AddAssert("Check if working property in the hit object is valid", () =>
        {
            var editorBeatmap = Dependencies.Get<EditorBeatmap>();

            return editorBeatmap.HitObjects.OfType<KaraokeHitObject>().All(hitObject => hitObject switch
            {
                Lyric lyric => !hasInvalidWorkingProperty(lyric),
                Note note => !hasInvalidWorkingProperty(note),
                _ => throw new NotSupportedException(),
            });
        });
    }

    private static bool hasInvalidWorkingProperty(Lyric lyric)
    {
        if (lyric is not (IHasWorkingProperty<LyricWorkingProperty, KaraokeBeatmap> workingProperty and IHasWorkingProperty<LyricStageWorkingProperty, StageInfo> stageWorkingProperty))
            throw new NotSupportedException();

        return workingProperty.HasInvalidWorkingProperty() || stageWorkingProperty.HasInvalidWorkingProperty();
    }

    private static bool hasInvalidWorkingProperty(Note note)
    {
        if (note is not (IHasWorkingProperty<NoteWorkingProperty, KaraokeBeatmap> workingProperty and IHasWorkingProperty<NoteStageWorkingProperty, StageInfo> stageWorkingProperty))
            throw new NotSupportedException();

        return workingProperty.HasInvalidWorkingProperty() || stageWorkingProperty.HasInvalidWorkingProperty();
    }

    private partial class MockEditorChangeHandler : TransactionalCommitComponent, IEditorChangeHandler
    {
        public event Action? OnStateChange;

        public MockEditorChangeHandler(EditorBeatmap editorBeatmap)
        {
            editorBeatmap.TransactionBegan += BeginChange;
            editorBeatmap.TransactionEnded += EndChange;
            editorBeatmap.SaveStateTriggered += SaveState;
        }

        protected override void UpdateState()
        {
            OnStateChange?.Invoke();
        }
    }
}
