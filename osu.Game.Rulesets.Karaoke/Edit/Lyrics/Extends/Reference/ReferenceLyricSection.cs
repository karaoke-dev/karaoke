// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Reference
{
    public class ReferenceLyricSection : LyricPropertySection
    {
        protected override LocalisableString Title => "Reference lyric";

        private readonly LabelledLyricSelector labelledLyricSelector;

        public ReferenceLyricSection()
        {
            Children = new[]
            {
                labelledLyricSelector = new LabelledLyricSelector
                {
                    Label = "Referenced lyric",
                    Description = "Select the similar lyric that want to reference or sync the property."
                }
            };
        }

        protected override void OnLyricChanged(Lyric? lyric)
        {
            if (lyric == null)
                return;

            labelledLyricSelector.Current = lyric.ReferenceLyricBindable;
        }
    }
}
