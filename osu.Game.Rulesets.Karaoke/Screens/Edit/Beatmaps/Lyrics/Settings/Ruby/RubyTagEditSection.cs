// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Ruby.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Ruby;

public partial class RubyTagEditSection : LyricPropertiesSection<RubyTag>
{
    protected override LocalisableString Title => "Ruby";

    protected override LyricPropertiesEditor CreateLyricPropertiesEditor() => new RubyTagsEditor();

    protected override LockLyricPropertyBy? IsWriteLyricPropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.RubyTags));

    protected override LocalisableString GetWriteLyricPropertyLockedDescription(LockLyricPropertyBy lockLyricPropertyBy) =>
        lockLyricPropertyBy switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Ruby is sync to another ruby.",
            LockLyricPropertyBy.LockState => "Ruby is locked.",
            _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null),
        };

    protected override LocalisableString GetWriteLyricPropertyLockedTooltip(LockLyricPropertyBy lockLyricPropertyBy) =>
        lockLyricPropertyBy switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Cannot edit the ruby because it's sync to another lyric's text.",
            LockLyricPropertyBy.LockState => "The lyric is locked, so cannot edit the ruby.",
            _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null),
        };

    private partial class RubyTagsEditor : LyricPropertiesEditor
    {
        protected sealed override Drawable CreateDrawable(RubyTag item)
        {
            string relativeToLyricText = RubyTagUtils.GetTextFromLyric(item, CurrentLyric.Text);
            string range = RubyTagUtils.PositionFormattedString(item);

            return new LabelledRubyTagTextBox(CurrentLyric, item).With(t =>
            {
                t.Label = relativeToLyricText;
                t.Description = range;
                t.TabbableContentContainer = this;
            });
        }

        protected override EditorSectionButton? CreateCreateNewItemButton() => null;

        protected override IBindableList<RubyTag> GetItems(Lyric lyric)
            => lyric.RubyTagsBindable;
    }
}
