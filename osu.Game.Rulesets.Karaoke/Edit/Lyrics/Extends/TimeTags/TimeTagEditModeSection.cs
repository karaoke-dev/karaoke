// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagEditModeSection : Section
    {
        protected override string Title => "Edit mode";

        [BackgroundDependencyLoader]
        private void load(OsuColour colour)
        {
            Children = new[]
            {
                new EditModeButton(TimeTagEditMode.View)
                {
                    Text = "View",
                    Action = updateEditMode,
                },
                new EditModeButton(TimeTagEditMode.Create)
                {
                    Text = "Create",
                    Action = updateEditMode,
                },
                new EditModeButton(TimeTagEditMode.Edit)
                {
                    Text = "Edit",
                    Action = updateEditMode,
                }
            };

            updateEditMode(TimeTagEditMode.View);

            void updateEditMode(TimeTagEditMode mode)
            {
                foreach (var child in Children.OfType<EditModeButton>())
                {
                    var highLight = child.Mode == mode;
                    child.Alpha = highLight ? 0.8f : 0.4f;

                    switch (child.Mode)
                    {
                        case TimeTagEditMode.View:
                            child.BackgroundColour = highLight ? colour.Blue : colour.BlueDarker;
                            break;

                        case TimeTagEditMode.Create:
                            child.BackgroundColour = highLight ? colour.Green : colour.GreenDarker;
                            break;

                        case TimeTagEditMode.Edit:
                            child.BackgroundColour = highLight ? colour.Yellow : colour.YellowDarker;
                            break;
                    }
                }
            }
        }

        public class EditModeButton : OsuButton
        {
            public new Action<TimeTagEditMode> Action;

            public TimeTagEditMode Mode { get; }

            public EditModeButton(TimeTagEditMode mode)
            {
                Mode = mode;
                RelativeSizeAxes = Axes.X;
                Content.CornerRadius = 15;

                base.Action = () => Action.Invoke(mode);
            }

            public new bool Enabled
            {
                get => base.Enabled.Value;
                set => base.Enabled.Value = value;
            }
        }
    }
}
