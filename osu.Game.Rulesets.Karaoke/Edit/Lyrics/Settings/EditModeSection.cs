// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings
{
    public abstract class EditModeSection<TEditModeState, TEditMode> : EditModeSection<TEditMode>
        where TEditModeState : IHasEditModeState<TEditMode> where TEditMode : Enum
    {
        [Resolved]
        private TEditModeState tEditModeState { get; set; }

        protected sealed override TEditMode DefaultMode() => tEditModeState.EditMode;

        internal sealed override void UpdateEditMode(TEditMode mode)
        {
            tEditModeState.ChangeEditMode(mode);

            base.UpdateEditMode(mode);
        }
    }

    public abstract class EditModeSection<TEditMode> : LyricEditorSection where TEditMode : Enum
    {
        private const int horizontal_padding = 20;

        protected sealed override LocalisableString Title => "Edit mode";

        [Cached]
        private readonly OverlayColourProvider overlayColourProvider;

        [Resolved]
        private OsuColour colours { get; set; }

        [Resolved]
        private ILyricSelectionState lyricSelectionState { get; set; }

        private readonly Selection[] selections;
        private readonly DescriptionTextFlowContainer description;

        protected EditModeSection()
        {
            overlayColourProvider = new OverlayColourProvider(CreateColourScheme());

            Children = new Drawable[]
            {
                new GridContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    RowDimensions = new[]
                    {
                        new Dimension(GridSizeMode.AutoSize)
                    },
                    Content = new[]
                    {
                        selections = createSelections()
                    }
                },
                description = new DescriptionTextFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding { Horizontal = horizontal_padding },
                }
            };

            // should wait until derived class BDL ready.
            Schedule(() =>
            {
                UpdateEditMode(DefaultMode());
            });
        }

        private Selection[] createSelections()
            => EnumUtils.GetValues<TEditMode>().Select(mode =>
            {
                var selection = GetSelectionInstance(mode);
                selection.Mode = mode;
                selection.Text = GetSelectionText(mode);
                selection.Padding = new MarginPadding { Horizontal = 5 };
                selection.Action = UpdateEditMode;

                return selection;
            }).ToArray();

        internal virtual void UpdateEditMode(TEditMode mode)
        {
            // should cancel the selection after change to the new edit mode.
            lyricSelectionState?.EndSelecting(LyricEditorSelectingAction.Cancel);

            // update button style.
            foreach (var button in selections)
            {
                bool highLight = EqualityComparer<TEditMode>.Default.Equals(button.Mode, mode);
                button.Alpha = highLight ? 0.8f : 0.4f;
                button.BackgroundColour = GetSelectionColour(colours, button.Mode, highLight);

                if (!highLight)
                    continue;

                Schedule(() =>
                {
                    // update description text.
                    description.Description = GetSelectionDescription(mode);
                });
            }
        }

        protected abstract OverlayColourScheme CreateColourScheme();

        protected abstract TEditMode DefaultMode();

        protected virtual Selection GetSelectionInstance(TEditMode mode) => new(mode);

        protected abstract LocalisableString GetSelectionText(TEditMode mode);

        protected abstract Color4 GetSelectionColour(OsuColour colours, TEditMode mode, bool active);

        protected abstract DescriptionFormat GetSelectionDescription(TEditMode mode);

        protected class Selection : OsuButton
        {
            public new Action<TEditMode> Action;

            public TEditMode Mode { get; set; }

            public Selection()
            {
                RelativeSizeAxes = Axes.X;
                Content.CornerRadius = 15;

                base.Action = () => Action?.Invoke(Mode);
            }
        }
    }
}
