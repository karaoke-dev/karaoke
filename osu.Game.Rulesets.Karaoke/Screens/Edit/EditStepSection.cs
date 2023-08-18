// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Overlays.Toolbar;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

public abstract partial class EditStepSection<TEditStep> : EditorSection where TEditStep : struct, Enum
{
    private const int horizontal_padding = 20;

    protected sealed override LocalisableString Title => "Edit step";

    [Cached]
    private readonly OverlayColourProvider overlayColourProvider;

    [Resolved]
    private OsuColour colours { get; set; } = null!;

    private readonly Selection[] selections;
    private readonly DescriptionTextFlowContainer lyricEditorDescription;

    protected EditStepSection()
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
                    new Dimension(GridSizeMode.AutoSize),
                },
                Content = new[]
                {
                    selections = createSelections(),
                },
            },
            lyricEditorDescription = CreateDescriptionTextFlowContainer().With(x =>
            {
                x.RelativeSizeAxes = Axes.X;
                x.AutoSizeAxes = Axes.Y;
                x.Padding = new MarginPadding { Horizontal = horizontal_padding };
            }),
        };

        // should wait until derived class BDL ready.
        Schedule(() =>
        {
            UpdateEditStep(DefaultStep());
        });
    }

    private Selection[] createSelections()
        => Enum.GetValues<TEditStep>().Select(step =>
        {
            var selection = CreateSelection(step);
            selection.Step = step;
            selection.Text = GetSelectionText(step);
            selection.Padding = new MarginPadding { Horizontal = 5 };
            selection.Action = UpdateEditStep;

            return selection;
        }).ToArray();

    internal virtual void UpdateEditStep(TEditStep step)
    {
        // update button style.
        foreach (var button in selections)
        {
            bool highLight = EqualityComparer<TEditStep>.Default.Equals(button.Step, step);
            button.Alpha = highLight ? 0.8f : 0.4f;
            button.BackgroundColour = GetSelectionColour(colours, button.Step, highLight);

            if (!highLight)
                continue;

            Schedule(() =>
            {
                // update description text.
                lyricEditorDescription.Description = GetSelectionDescription(step);
            });
        }
    }

    protected virtual DescriptionTextFlowContainer CreateDescriptionTextFlowContainer() => new();

    protected abstract OverlayColourScheme CreateColourScheme();

    protected abstract TEditStep DefaultStep();

    protected abstract Selection CreateSelection(TEditStep step);

    protected abstract LocalisableString GetSelectionText(TEditStep step);

    protected abstract Color4 GetSelectionColour(OsuColour colours, TEditStep step, bool active);

    protected abstract DescriptionFormat GetSelectionDescription(TEditStep step);

    protected partial class Selection : EditorSectionButton
    {
        public new Action<TEditStep>? Action;

        public TEditStep Step { get; set; }

        public Selection()
        {
            base.Action = () => Action?.Invoke(Step);
        }
    }

    protected abstract partial class VerifySelection : Selection
    {
        protected readonly IBindableList<Issue> Issues = new BindableList<Issue>();

        protected VerifySelection()
        {
            CountCircle countCircle;

            AddInternal(countCircle = new CountCircle
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.Centre,
                X = -5,
            });

            Issues.BindCollectionChanged((_, _) =>
            {
                int count = Issues.Count;
                countCircle.Alpha = count == 0 ? 0 : 1;
                countCircle.Count = count;
            });
        }
    }

    /// <summary>
    /// Copied from <see cref="ToolbarNotificationButton"/>
    /// </summary>
    private partial class CountCircle : CompositeDrawable
    {
        private readonly OsuSpriteText countText;
        private readonly Circle circle;

        private int count;

        public int Count
        {
            get => count;
            set
            {
                if (count == value)
                    return;

                if (value != count)
                {
                    circle.FlashColour(Color4.White, 600, Easing.OutQuint);
                    this.ScaleTo(1.1f).Then().ScaleTo(1, 600, Easing.OutElastic);
                }

                count = value;
                countText.Text = value.ToString("#,0");
            }
        }

        public CountCircle()
        {
            AutoSizeAxes = Axes.X;
            Height = 20;

            InternalChildren = new Drawable[]
            {
                circle = new Circle
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Red,
                },
                countText = new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Y = -1,
                    Font = OsuFont.GetFont(size: 18, weight: FontWeight.Bold),
                    Padding = new MarginPadding(5),
                    Colour = Color4.White,
                    UseFullGlyphHeight = true,
                },
            };
        }
    }
}
