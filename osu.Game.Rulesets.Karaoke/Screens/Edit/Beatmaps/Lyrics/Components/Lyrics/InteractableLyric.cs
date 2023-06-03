// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics;

public abstract partial class InteractableLyric : CompositeDrawable, IHasTooltip
{
    [Cached(typeof(IPreviewLyricPositionProvider))]
    private readonly PreviewKaraokeSpriteText karaokeSpriteText;

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    protected readonly IBindable<LyricEditorMode> BindableMode = new Bindable<LyricEditorMode>();
    private readonly IBindable<int> bindableLyricPropertyWritableVersion;

    private readonly Lyric lyric;
    private LocalisableString? lockReason;

    protected InteractableLyric(Lyric lyric)
    {
        this.lyric = lyric;

        bindableLyricPropertyWritableVersion = lyric.LyricPropertyWritableVersion.GetBoundCopy();

        InternalChildren = new Drawable[]
        {
            new LyricLayer(lyric, karaokeSpriteText = new PreviewKaraokeSpriteText(lyric)),
        };

        AddRangeInternal(CreateLayers(lyric));

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

    protected abstract IEnumerable<BaseLayer> CreateLayers(Lyric lyric);

    [BackgroundDependencyLoader]
    private void load(EditorClock clock, ILyricEditorState state)
    {
        BindableMode.BindTo(state.BindableMode);

        karaokeSpriteText.Clock = clock;
    }

    protected override bool OnMouseMove(MouseMoveEvent e)
    {
        if (!lyricCaretState.CaretEnabled)
            return false;

        float xPosition = ToLocalSpace(e.ScreenSpaceMousePosition).X;

        object? caretIndex = getCaretIndexByPosition(xPosition);

        if (caretIndex != null)
        {
            lyricCaretState.MoveHoverCaretToTargetPosition(lyric, caretIndex);
        }
        else if (lyricCaretState.CaretPosition is not IIndexCaretPosition)
        {
            // still need to handle the case with non-index caret position.
            lyricCaretState.MoveHoverCaretToTargetPosition(lyric);
        }

        return base.OnMouseMove(e);
    }

    private object? getCaretIndexByPosition(float position) =>
        lyricCaretState.CaretPosition switch
        {
            CuttingCaretPosition => karaokeSpriteText.GetCharIndicatorByPosition(position),
            TypingCaretPosition => karaokeSpriteText.GetCharIndicatorByPosition(position),
            NavigateCaretPosition => null,
            TimeTagIndexCaretPosition => karaokeSpriteText.GetCharIndexByPosition(position),
            TimeTagCaretPosition => karaokeSpriteText.GetTimeTagByPosition(position),
            _ => null
        };

    protected override void OnHoverLost(HoverLostEvent e)
    {
        base.OnHoverLost(e);

        if (!lyricCaretState.CaretEnabled)
            return;

        // lost hover caret and time-tag caret
        lyricCaretState.ClearHoverCaretPosition();
    }

    protected override bool OnClick(ClickEvent e)
    {
        return lyricCaretState.ConfirmHoverCaretPosition();
    }

    private void triggerWritableVersionChanged()
    {
        var loadReason = GetLyricPropertyLockedReason(lyric, BindableMode.Value);
        lockReason = loadReason;

        // adjust the style.
        bool editable = lockReason == null;
        InternalChildren.OfType<BaseLayer>().ForEach(x => x.UpdateDisableEditState(editable));
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
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static LockLyricPropertyBy? getLyricPropertyLockedReasons(Lyric lyric, LyricEditorMode mode)
    {
        return mode switch
        {
            LyricEditorMode.View => null,
            LyricEditorMode.Texting => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.Text), nameof(Lyric.RubyTags), nameof(Lyric.RomajiTags), nameof(Lyric.TimeTags)),
            LyricEditorMode.Reference => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.ReferenceLyric), nameof(Lyric.ReferenceLyricConfig)),
            LyricEditorMode.Language => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.Language)),
            LyricEditorMode.EditRuby => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.RubyTags)),
            LyricEditorMode.EditRomaji => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.RomajiTags)),
            LyricEditorMode.EditTimeTag => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.TimeTags)),
            LyricEditorMode.EditNote => HitObjectWritableUtils.GetCreateOrRemoveNoteLockedBy(lyric),
            LyricEditorMode.Singer => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.SingerIds)),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }
}
