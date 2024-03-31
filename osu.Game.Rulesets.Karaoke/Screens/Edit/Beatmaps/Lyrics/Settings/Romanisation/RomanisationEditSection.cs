// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romanisation.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romanisation;

public partial class RomanisationEditSection : LyricPropertiesSection<TimeTag>
{
    protected override LocalisableString Title => "Romanisation";

    protected override LyricPropertiesEditor CreateLyricPropertiesEditor() => new RomanisationTagsEditor();

    protected override LockLyricPropertyBy? IsWriteLyricPropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.TimeTags));

    protected override LocalisableString GetWriteLyricPropertyLockedDescription(LockLyricPropertyBy lockLyricPropertyBy) =>
        lockLyricPropertyBy switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Romanisation is sync to another romanisation.",
            LockLyricPropertyBy.LockState => "Romanisation is locked.",
            _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null),
        };

    protected override LocalisableString GetWriteLyricPropertyLockedTooltip(LockLyricPropertyBy lockLyricPropertyBy) =>
        lockLyricPropertyBy switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Cannot edit the romanisation because it's sync to another lyric's text.",
            LockLyricPropertyBy.LockState => "The lyric is locked, so cannot edit the romanisation.",
            _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null),
        };

    private partial class RomanisationTagsEditor : LyricPropertiesEditor
    {
        protected override Drawable? CreateDrawable(TimeTag item)
        {
            if (!isEditable(item))
            {
                return null;
            }

            return new LabelledRomanisedTextBox(CurrentLyric, item)
            {
                TabbableContentContainer = this,
            };

            static bool isEditable(TimeTag timeTag)
                => timeTag.Index.State == TextIndex.IndexState.Start;
        }

        protected override EditorSectionButton? CreateCreateNewItemButton() => null;

        protected override IBindableList<TimeTag> GetItems(Lyric lyric)
            => lyric.TimeTagsBindable;
    }
}
