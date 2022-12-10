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
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

public abstract partial class EditModeSection<TEditMode> : EditorSection where TEditMode : Enum
{
    private const int horizontal_padding = 20;

    [Cached]
    private readonly OverlayColourProvider overlayColourProvider;

    [Resolved]
    private OsuColour colours { get; set; }

    private readonly Selection[] selections;
    private readonly DescriptionTextFlowContainer lyricEditorDescription;

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
            lyricEditorDescription = CreateDescriptionTextFlowContainer().With(x =>
            {
                x.RelativeSizeAxes = Axes.X;
                x.AutoSizeAxes = Axes.Y;
                x.Padding = new MarginPadding { Horizontal = horizontal_padding };
            })
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
            var selection = CreateSelection(mode);
            selection.Mode = mode;
            selection.Text = GetSelectionText(mode);
            selection.Padding = new MarginPadding { Horizontal = 5 };
            selection.Action = UpdateEditMode;

            return selection;
        }).ToArray();

    internal virtual void UpdateEditMode(TEditMode mode)
    {
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
                lyricEditorDescription.Description = GetSelectionDescription(mode);
            });
        }
    }

    protected abstract DescriptionTextFlowContainer CreateDescriptionTextFlowContainer();

    protected abstract OverlayColourScheme CreateColourScheme();

    protected abstract TEditMode DefaultMode();

    protected abstract Selection CreateSelection(TEditMode mode);

    protected abstract LocalisableString GetSelectionText(TEditMode mode);

    protected abstract Color4 GetSelectionColour(OsuColour colours, TEditMode mode, bool active);

    protected abstract DescriptionFormat GetSelectionDescription(TEditMode mode);

    protected partial class Selection : OsuButton
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
