// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

[Cached(typeof(IInteractableLyricState))]
public abstract partial class InteractableLyric : CompositeDrawable, IHasTooltip, IInteractableLyricState
{
    [Cached(typeof(IPreviewLyricPositionProvider))]
    private readonly PreviewKaraokeSpriteText karaokeSpriteText;

    protected readonly IBindable<LyricEditorMode> BindableMode = new Bindable<LyricEditorMode>();
    private readonly IBindable<int> bindableLyricPropertyWritableVersion;

    protected readonly Lyric Lyric;
    private LocalisableString? lockReason;

    protected InteractableLyric(Lyric lyric)
    {
        Lyric = lyric;

        bindableLyricPropertyWritableVersion = lyric.LyricPropertyWritableVersion.GetBoundCopy();

        karaokeSpriteText = new PreviewKaraokeSpriteText(lyric);

        karaokeSpriteText.SizeChanged = () =>
        {
            Height = karaokeSpriteText.DrawHeight;
        };

        BindableMode.BindValueChanged(x =>
        {
            triggerWritableVersionChanged();
        });

        bindableLyricPropertyWritableVersion.BindValueChanged(_ =>
        {
            triggerWritableVersionChanged();
        });
    }

    public IEnumerable<Layer> Layers
    {
        get => InternalChildren.OfType<Layer>();
        init
        {
            AddRangeInternal(value);

            // todo: should apply proxy instead, but it let disable edit state not working.
            var lyricLayers = value.OfType<LyricLayer>().Single();
            lyricLayers.ApplyDrawableLyric(karaokeSpriteText);
        }
    }

    public void TriggerDisallowEditEffect()
    {
        InternalChildren.OfType<Layer>().ForEach(x => x.TriggerDisallowEditEffect(BindableMode.Value));
    }

    [BackgroundDependencyLoader]
    private void load(EditorClock clock, ILyricEditorState state)
    {
        BindableMode.BindTo(state.BindableMode);

        karaokeSpriteText.Clock = clock;
    }

    private void triggerWritableVersionChanged()
    {
        var loadReason = GetLyricPropertyLockedReason(Lyric, BindableMode.Value);
        lockReason = loadReason;

        // adjust the style.
        bool editable = lockReason == null;
        InternalChildren.OfType<Layer>().ForEach(x => x.UpdateDisableEditState(editable));
    }

    public LocalisableString TooltipText => lockReason ?? string.Empty;

    public static LocalisableString? GetLyricPropertyLockedReason(Lyric lyric, LyricEditorMode mode)
    {
        var reasons = getLyricPropertyLockedReasons(lyric, mode);

        return reasons switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Cannot modify this property due to this lyric is property is sync from another lyric.",
            LockLyricPropertyBy.LockState => "This property is locked and not editable",
            null => default(LocalisableString?),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    private static LockLyricPropertyBy? getLyricPropertyLockedReasons(Lyric lyric, LyricEditorMode mode)
    {
        return mode switch
        {
            LyricEditorMode.View => null,
            LyricEditorMode.EditText => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Objects.Lyric.Text), nameof(Objects.Lyric.RubyTags), nameof(Objects.Lyric.TimeTags)),
            LyricEditorMode.EditReferenceLyric => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Objects.Lyric.ReferenceLyric), nameof(Objects.Lyric.ReferenceLyricConfig)),
            LyricEditorMode.EditLanguage => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Objects.Lyric.Language)),
            LyricEditorMode.EditRuby => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Objects.Lyric.RubyTags)),
            LyricEditorMode.EditTimeTag => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Objects.Lyric.TimeTags)),
            LyricEditorMode.EditRomanisation => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Objects.Lyric.TimeTags)),
            LyricEditorMode.EditNote => HitObjectWritableUtils.GetCreateOrRemoveNoteLockedBy(lyric),
            LyricEditorMode.EditSinger => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Objects.Lyric.SingerIds)),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };
    }
}
