// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Extensions;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects.Stages;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Stages;

namespace osu.Game.Rulesets.Karaoke.Objects;

/// <summary>
/// Placing the properties that set by <see cref="KaraokeBeatmapProcessor"/> or being calculated.
/// Those properties will not be saved into the beatmap.
/// </summary>
public partial class Note : IHasWorkingProperty<NoteWorkingProperty, KaraokeBeatmap>, IHasWorkingProperty<NoteStageWorkingProperty, StageInfo>, IHasEffectApplier
{
    [JsonIgnore]
    private readonly NoteWorkingPropertyValidator workingPropertyValidator;

    [JsonIgnore]
    private readonly NoteStageWorkingPropertyValidator stageWorkingPropertyValidator;

    public bool InvalidateWorkingProperty(NoteWorkingProperty workingProperty)
        => workingPropertyValidator.Invalidate(workingProperty);

    public bool InvalidateWorkingProperty(NoteStageWorkingProperty workingProperty)
        => stageWorkingPropertyValidator.Invalidate(workingProperty);

    private void updateStateByWorkingProperty(NoteWorkingProperty workingProperty)
        => workingPropertyValidator.UpdateStateByWorkingProperty(workingProperty);

    private void updateStateByWorkingProperty(NoteStageWorkingProperty workingProperty)
        => stageWorkingPropertyValidator.UpdateStateByWorkingProperty(workingProperty);

    NoteWorkingProperty[] IHasWorkingProperty<NoteWorkingProperty, KaraokeBeatmap>.GetAllInvalidWorkingProperties()
        => workingPropertyValidator.GetAllInvalidFlags();

    NoteStageWorkingProperty[] IHasWorkingProperty<NoteStageWorkingProperty, StageInfo>.GetAllInvalidWorkingProperties()
        => stageWorkingPropertyValidator.GetAllInvalidFlags();

    public void ValidateWorkingProperty(KaraokeBeatmap beatmap)
    {
        foreach (var flag in workingPropertyValidator.GetAllInvalidFlags())
        {
            switch (flag)
            {
                case NoteWorkingProperty.Page:
                    PageIndex = getPageIndex(beatmap, StartTime);
                    break;

                case NoteWorkingProperty.ReferenceLyric:
                    ReferenceLyric = findLyricById(beatmap, ReferenceLyricId);
                    break;

                case NoteWorkingProperty.EffectApplier:
                    EffectApplier = getStageEffectApplier(beatmap, this);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        static int? getPageIndex(KaraokeBeatmap beatmap, double startTime)
            => beatmap.PageInfo.GetPageIndexAt(startTime);

        static Lyric? findLyricById(IBeatmap beatmap, ElementId? id) =>
            id == null ? null : beatmap.HitObjects.OfType<Lyric>().Single(x => x.ID == id);

        static IStageEffectApplier getStageEffectApplier(KaraokeBeatmap beatmap, Note note)
        {
            var stageInfo = beatmap.CurrentStageInfo;
            if (stageInfo == null)
                throw new InvalidCastException();

            return stageInfo.GetStageAppliers(note);
        }
    }

    public void ValidateWorkingProperty(StageInfo stageInfo)
    {
    }

    [JsonIgnore]
    public readonly Bindable<int?> PageIndexBindable = new();

    /// <summary>
    /// Order
    /// </summary>
    [JsonIgnore]
    public int? PageIndex
    {
        get => PageIndexBindable.Value;
        set
        {
            PageIndexBindable.Value = value;
            updateStateByWorkingProperty(NoteWorkingProperty.Page);
        }
    }

    /// <summary>
    /// Start time.
    /// There's no need to save the time because it's calculated by the <see cref="TimeTag"/>
    /// </summary>
    [JsonIgnore]
    public override double StartTime
    {
        get => base.StartTime;
        set => throw new NotSupportedException($"The time will auto-sync via {nameof(ReferenceLyric)} and {nameof(ReferenceTimeTagIndex)}.");
    }

    [JsonIgnore]
    public readonly Bindable<double> DurationBindable = new BindableDouble();

    /// <summary>
    /// Duration.
    /// There's no need to save the time because it's calculated by the <see cref="TimeTag"/>
    /// </summary>
    [JsonIgnore]
    public double Duration
    {
        get => DurationBindable.Value;
        set => throw new NotSupportedException($"The time will auto-sync via {nameof(ReferenceLyric)} and {nameof(ReferenceTimeTagIndex)}.");
    }

    /// <summary>
    /// End time.
    /// There's no need to save the time because it's calculated by the <see cref="TimeTag"/>
    /// </summary>
    [JsonIgnore]
    public double EndTime => StartTime + Duration;

    [JsonIgnore]
    public readonly Bindable<Lyric?> ReferenceLyricBindable = new();

    [JsonIgnore]
    public readonly BindableDictionary<Singer, SingerState[]> SingersBindable = new();

    /// <summary>
    /// Singers
    /// </summary>
    [JsonIgnore]
    public IDictionary<Singer, SingerState[]> Singers
    {
        get => SingersBindable;
        set
        {
            SingersBindable.Clear();
            SingersBindable.AddRange(value);
        }
    }

    /// <summary>
    /// Relative lyric.
    /// Technically parent lyric will not change after assign, but should not restrict in model layer.
    /// </summary>
    [JsonIgnore]
    public Lyric? ReferenceLyric
    {
        get => ReferenceLyricBindable.Value;
        set
        {
            ReferenceLyricBindable.Value = value;
            updateStateByWorkingProperty(NoteWorkingProperty.ReferenceLyric);
        }
    }

    [JsonIgnore]
    public readonly Bindable<IStageEffectApplier> EffectApplierBindable = new();

    /// <summary>
    /// Stage elements.
    /// Will save all the elements that related to the lyric.
    /// The element might include something like style or layout info.
    /// </summary>
    [JsonIgnore]
    public IStageEffectApplier EffectApplier
    {
        get => EffectApplierBindable.Value;
        set
        {
            EffectApplierBindable.Value = value;

            updateStateByWorkingProperty(NoteWorkingProperty.EffectApplier);
        }
    }

    public TimeTag? StartReferenceTimeTag => ReferenceLyric?.TimeTags.ElementAtOrDefault(ReferenceTimeTagIndex);

    public TimeTag? EndReferenceTimeTag => ReferenceLyric?.TimeTags.ElementAtOrDefault(ReferenceTimeTagIndex + 1);
}
