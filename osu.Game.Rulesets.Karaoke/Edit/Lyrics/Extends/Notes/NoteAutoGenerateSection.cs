// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
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

        [BackgroundDependencyLoader]
        private void load(LyricCheckerManager lyricCheckerManager)
        {
            bindableReports = lyricCheckerManager.BindableReports.GetBoundCopy();
            bindableReports.BindCollectionChanged((a, b) =>
            {
                var hasTimeTagIssue = bindableReports.Values.SelectMany(x => x)
                                                     .OfType<TimeTagIssue>().Any();

                if (hasTimeTagIssue)
                {
                    Children = new[]
                    {
                        new OsuSpriteText
                        {
                            Text = "Seems there's some time-tag issue in lyric. \nGo to time-tag edit mode then clear those issues."
                        }
                    };
                }
                else
                {
                    Children = new[]
                    {
                        new AutoGenerateButton
                        {
                            Text = "Generate",
                            Action = () =>
                            {
                                // todo : // auto-generate time-tag.
                            }
                        },
                    };
                }
            }, true);
        }

        public class AutoGenerateButton : OsuButton
        {
            public AutoGenerateButton()
            {
                RelativeSizeAxes = Axes.X;
                Content.CornerRadius = 15;
            }
        }
    }
}
