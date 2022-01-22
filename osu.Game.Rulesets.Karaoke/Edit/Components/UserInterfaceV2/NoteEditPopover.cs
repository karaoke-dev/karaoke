// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2
{
    public class NoteEditPopover : OsuPopover
    {
        [Resolved(canBeNull: true)]
        private INotesChangeHandler notesChangeHandler { get; set; }

        public NoteEditPopover(Note note)
        {
            LabelledTextBox text;
            LabelledTextBox rubyText;
            LabelledSwitchButton display;

            Children = new Drawable[]
            {
                new FillFlowContainer
                {
                    Width = 200,
                    Direction = FillDirection.Vertical,
                    AutoSizeAxes = Axes.Y,
                    Children = new Drawable[]
                    {
                        text = new LabelledTextBox
                        {
                            Label = "Text",
                            Description = "The text display on the note.",
                            Current = note.TextBindable
                        },
                        rubyText = new LabelledTextBox
                        {
                            Label = "Ruby text",
                            Description = "Should place something like ruby, 拼音 or ふりがな.",
                            Current = note.RubyTextBindable
                        },
                        display = new LabelledSwitchButton
                        {
                            Label = "Display",
                            Description = "This note will be hidden and not scorable if not display.",
                            Current = note.DisplayBindable
                        }
                    }
                }
            };

            text.OnCommit += (sender, newText) =>
            {
                string text = sender.Text.Trim();
                notesChangeHandler?.ChangeText(text);
            };

            rubyText.OnCommit += (sender, newText) =>
            {
                string text = sender.Text.Trim();
                notesChangeHandler?.ChangeRubyText(text);
            };

            display.Current.BindValueChanged(v =>
            {
                notesChangeHandler?.ChangeDisplayState(v.NewValue);
            });
        }

        [BackgroundDependencyLoader(true)]
        private void load(HitObjectComposer composer)
        {
            if (notesChangeHandler != null || composer == null)
                return;

            // todo: not a good way to get change handler, might remove or found another way eventually.
            // cannot get change handler directly in editor screen, so should trying to get from karaoke hit object composer.
            notesChangeHandler = composer.Dependencies.Get<INotesChangeHandler>();
        }
    }
}
