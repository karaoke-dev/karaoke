// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics
{
    public abstract class InteractableLyric : CompositeDrawable, IHasTooltip
    {
        [Cached]
        private readonly InteractableKaraokeSpriteText karaokeSpriteText;

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

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
                new LyricLayer(lyric, karaokeSpriteText = new InteractableKaraokeSpriteText(lyric)),
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

            float position = ToLocalSpace(e.ScreenSpaceMousePosition).X;

            switch (lyricCaretState.BindableCaretPositionAlgorithm.Value)
            {
                case CuttingCaretPositionAlgorithm:
                    int cuttingLyricStringIndex = Math.Clamp(TextIndexUtils.ToStringIndex(karaokeSpriteText.GetHoverIndex(position)), 0, lyric.Text.Length - 1);
                    lyricCaretState.MoveHoverCaretToTargetPosition(new CuttingCaretPosition(lyric, cuttingLyricStringIndex));
                    break;

                case TypingCaretPositionAlgorithm:
                    int typingStringIndex = TextIndexUtils.ToStringIndex(karaokeSpriteText.GetHoverIndex(position));
                    lyricCaretState.MoveHoverCaretToTargetPosition(new TypingCaretPosition(lyric, typingStringIndex));
                    break;

                case NavigateCaretPositionAlgorithm:
                    lyricCaretState.MoveHoverCaretToTargetPosition(new NavigateCaretPosition(lyric));
                    break;

                case TimeTagIndexCaretPositionAlgorithm:
                    var textIndex = karaokeSpriteText.GetHoverIndex(position);
                    lyricCaretState.MoveHoverCaretToTargetPosition(new TimeTagIndexCaretPosition(lyric, textIndex));
                    break;

                case TimeTagCaretPositionAlgorithm:
                    var timeTag = karaokeSpriteText.GetHoverTimeTag(position);
                    lyricCaretState.MoveHoverCaretToTargetPosition(new TimeTagCaretPosition(lyric, timeTag));
                    break;
            }

            return base.OnMouseMove(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!lyricCaretState.CaretEnabled)
                return;

            // lost hover caret and time-tag caret
            lyricCaretState.ClearHoverCaretPosition();
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (!lyricCaretState.CaretEnabled)
                return false;

            // place hover caret to target position.
            var position = lyricCaretState.BindableHoverCaretPosition.Value;
            if (position == null)
                return false;

            lyricCaretState.MoveCaretToTargetPosition(position);

            return true;
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
                LyricEditorMode.Singer => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.Singers)),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }
    }
}
