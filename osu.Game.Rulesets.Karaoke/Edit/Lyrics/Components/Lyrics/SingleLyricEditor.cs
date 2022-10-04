// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics
{
    public class SingleLyricEditor : CompositeDrawable, IHasTooltip
    {
        [Cached]
        private readonly EditorKaraokeSpriteText karaokeSpriteText;

        private readonly Lyric lyric;
        private LocalisableString? lockReason;

        public SingleLyricEditor(Lyric lyric)
        {
            this.lyric = lyric;

            CornerRadius = 5;
            Padding = new MarginPadding { Bottom = 10 };
            InternalChildren = new Drawable[]
            {
                new LyricLayer(lyric, karaokeSpriteText = new EditorKaraokeSpriteText(lyric)),
                new TimeTagLayer(lyric),
                new CaretLayer(lyric),
                new BlueprintLayer(lyric),
            };

            karaokeSpriteText.SizeChanged = () =>
            {
                Height = karaokeSpriteText.DrawHeight;
            };
        }

        [BackgroundDependencyLoader]
        private void load(EditorClock clock, IEditLyricRowState editLyricRowState)
        {
            karaokeSpriteText.Clock = clock;

            editLyricRowState.WritableVersionChanged += mode =>
            {
                var loadReason = GetLyricPropertyLockedReason(lyric, mode);
                lockReason = loadReason;

                // adjust the style.
                bool editable = lockReason == null;
                InternalChildren.OfType<BaseLayer>().ForEach(x => x.UpdateDisableEditState(editable));
            };

            editLyricRowState.DisallowEditEffectTriggered += mode =>
            {
                InternalChildren.OfType<BaseLayer>().ForEach(x => x.TriggerDisallowEditEffect(mode));
            };
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
