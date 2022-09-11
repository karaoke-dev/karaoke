// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2
{
    public class NoteEditPopover : OsuPopover
    {
        public NoteEditPopover(Note note)
        {
            if (note == null)
                throw new ArgumentNullException(nameof(note));

            Child = new OsuScrollContainer
            {
                Height = 320,
                Width = 300,
                Child = new FillFlowContainer<Section>
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Children = new Section[]
                    {
                        new NoteSection(note),
                    }
                }
            };
        }

        private class NoteSection : Section
        {
            private INotePropertyChangeHandler notePropertyChangeHandler { get; set; }

            protected override LocalisableString Title => "Note property";

            public NoteSection(Note note)
            {
                LabelledTextBox text;
                LabelledTextBox rubyText;
                LabelledSwitchButton display;

                Children = new Drawable[]
                {
                    text = new LabelledTextBox
                    {
                        Label = "Text",
                        Description = "The text display on the note.",
                        Current = note.TextBindable,
                        TabbableContentContainer = this
                    },
                    rubyText = new LabelledTextBox
                    {
                        Label = "Ruby text",
                        Description = "Should place something like ruby, 拼音 or ふりがな.",
                        Current = note.RubyTextBindable,
                        TabbableContentContainer = this
                    },
                    display = new LabelledSwitchButton
                    {
                        Label = "Display",
                        Description = "This note will be hidden and not scorable if not display.",
                        Current = note.DisplayBindable,
                    }
                };

                ScheduleAfterChildren(() =>
                {
                    GetContainingInputManager().ChangeFocus(text);
                });

                text.OnCommit += (sender, newText) =>
                {
                    if (!newText)
                        return;

                    string text = sender.Text.Trim();
                    notePropertyChangeHandler?.ChangeText(text);
                };

                rubyText.OnCommit += (sender, newText) =>
                {
                    if (!newText)
                        return;

                    string text = sender.Text.Trim();
                    notePropertyChangeHandler?.ChangeRubyText(text);
                };

                display.Current.BindValueChanged(v =>
                {
                    notePropertyChangeHandler?.ChangeDisplayState(v.NewValue);
                });
            }

            [BackgroundDependencyLoader(true)]
            private void load(HitObjectComposer composer)
            {
                if (notePropertyChangeHandler != null || composer == null)
                    return;

                // todo: not a good way to get change handler, might remove or found another way eventually.
                // cannot get change handler directly in editor screen, so should trying to get from karaoke hit object composer.
                notePropertyChangeHandler = composer.Dependencies.Get<INotePropertyChangeHandler>();
            }
        }
    }
}
