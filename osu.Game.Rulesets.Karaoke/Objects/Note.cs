// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Karaoke.Objects;

public partial class Note : KaraokeHitObject, IHasPage, IHasDuration, IHasText, IDeepCloneable<Note>
{
    private void updateStateByDataProperty(NoteWorkingProperty workingProperty)
        => workingPropertyValidator.UpdateStateByDataProperty(workingProperty);

    [JsonIgnore]
    public readonly Bindable<string> TextBindable = new();

    /// <summary>
    /// Text display on the note.
    /// </summary>
    /// <example>
    /// 花
    /// </example>
    public string Text
    {
        get => TextBindable.Value;
        set => TextBindable.Value = value;
    }

    [JsonIgnore]
    public readonly Bindable<string?> RubyTextBindable = new();

    /// <summary>
    /// Ruby text.
    /// Should placing something like ruby, 拼音 or ふりがな.
    /// Will be display only if <see cref="KaraokeRulesetSetting.DisplayNoteRubyText"/> is true.
    /// </summary>
    /// <example>
    /// はな
    /// </example>
    public string? RubyText
    {
        get => RubyTextBindable.Value;
        set => RubyTextBindable.Value = value;
    }

    [JsonIgnore]
    public readonly Bindable<bool> DisplayBindable = new();

    /// <summary>
    /// Display this note
    /// </summary>
    public bool Display
    {
        get => DisplayBindable.Value;
        set => DisplayBindable.Value = value;
    }

    [JsonIgnore]
    public readonly Bindable<Tone> ToneBindable = new();

    /// <summary>
    /// Tone of this note
    /// </summary>
    public virtual Tone Tone
    {
        get => ToneBindable.Value;
        set => ToneBindable.Value = value;
    }

    [JsonIgnore]
    public readonly Bindable<double> StartTimeOffsetBindable = new BindableDouble();

    /// <summary>
    /// Offset time relative to the start time.
    /// </summary>
    public double StartTimeOffset
    {
        get => StartTimeOffsetBindable.Value;
        set => StartTimeOffsetBindable.Value = value;
    }

    [JsonIgnore]
    public readonly Bindable<double> EndTimeOffsetBindable = new BindableDouble();

    /// <summary>
    /// Offset time relative to the end time.
    /// Negative value means the adjusted time is smaller than actual.
    /// </summary>
    public double EndTimeOffset
    {
        get => EndTimeOffsetBindable.Value;
        set => EndTimeOffsetBindable.Value = value;
    }

    private int? referenceLyricId;

    public int? ReferenceLyricId
    {
        get => referenceLyricId;
        set
        {
            referenceLyricId = value;
            updateStateByDataProperty(NoteWorkingProperty.ReferenceLyric);
        }
    }

    [JsonIgnore]
    public readonly Bindable<int> ReferenceTimeTagIndexBindable = new();

    public int ReferenceTimeTagIndex
    {
        get => ReferenceTimeTagIndexBindable.Value;
        set => ReferenceTimeTagIndexBindable.Value = value;
    }

    public Note()
    {
        workingPropertyValidator = new NoteWorkingPropertyValidator(this);

        initInternalBindingEvent();
        initReferenceLyricEvent();
    }

    public override Judgement CreateJudgement() => new KaraokeNoteJudgement();

    protected override HitWindows CreateHitWindows() => new KaraokeNoteHitWindows();

    public Note DeepClone()
    {
        string serializeString = this.Serialize();
        var note = serializeString.Deserialize<Note>();
        note.ReferenceLyric = ReferenceLyric;

        return note;
    }
}
