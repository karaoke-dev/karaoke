// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Notes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    /// <summary>
    /// In <see cref="LyricEditorMode.CreateNote"/> mode, able to let user generate notes by <see cref="TimeTag"/>
    /// But need to make sure that lyric should not have any <see cref="TimeTagIssue"/>
    /// If found any issue, will navigate to target lyric.
    /// </summary>
    public class NoteAutoGenerateSection : Section
    {
        protected override string Title => "Auto generate";

        private BindableDictionary<Lyric, Issue[]> bindableReports;

        private OsuSpriteText alertSpriteText;

        [BackgroundDependencyLoader]
        private void load(LyricCheckerManager lyricCheckerManager, NoteManager noteManager, LyricSelectionState lyricSelectionState)
        {
            Children = new Drawable[]
            {
                new AutoGenerateButton
                {
                    StartSelecting = () =>
                        bindableReports.Where(x => x.Value.OfType<TimeTagIssue>().Any())
                                       .ToDictionary(k => k.Key, i => "Before generate time-tag, need to assign language first.")
                },
                alertSpriteText = new OsuSpriteText(),
            };

            bindableReports = lyricCheckerManager.BindableReports.GetBoundCopy();
            bindableReports.BindCollectionChanged((a, b) =>
            {
                var hasTimeTagIssue = bindableReports.Values.SelectMany(x => x)
                                                     .OfType<TimeTagIssue>().Any();

                alertSpriteText.Text = hasTimeTagIssue ? "Seems there's some time-tag issue in lyric. \nGo to time-tag edit mode then clear those issues." : null;
            }, true);

            lyricSelectionState.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                    return;

                var lyrics = lyricSelectionState.SelectedLyrics.ToList();
                noteManager.AutoGenerateNotes(lyrics);
            };
        }
    }
}
