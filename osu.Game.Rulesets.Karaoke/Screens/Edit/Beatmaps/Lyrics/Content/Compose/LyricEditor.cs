// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose;

public partial class LyricEditor : CompositeDrawable
{
    private readonly IBindable<Lyric?> bindableFocusedLyric = new Bindable<Lyric?>();
    private readonly IBindable<float> bindableFontSize = new Bindable<float>();

    private readonly LyricEditorSkin skin;
    private readonly DragContainer dragContainer;

    public LyricEditor()
    {
        RelativeSizeAxes = Axes.Both;

        InternalChild = new SkinProvidingContainer(skin = new LyricEditorSkin(null))
        {
            RelativeSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                dragContainer = new DragContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new ScrollBackButton
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Size = new Vector2(40),
                    Margin = new MarginPadding(20),
                },
            },
        };

        bindableFocusedLyric.BindValueChanged(e =>
        {
            refreshPreviewLyric(e.NewValue);
        });

        bindableFontSize.BindValueChanged(e =>
        {
            skin.FontSize = e.NewValue;
            refreshPreviewLyric(bindableFocusedLyric.Value);
        });
    }

    private void refreshPreviewLyric(Lyric? lyric)
    {
        dragContainer.Clear();

        if (lyric == null)
            return;

        const int border = 36;

        dragContainer.Add(new InteractableLyric(lyric)
        {
            TextSizeChanged = (self, size) =>
            {
                self.Width = size.X + border * 2;
                self.Height = size.Y + border * 2;
            },
            Loaders = new LayerLoader[]
            {
                new LayerLoader<GridLayer>
                {
                    OnLoad = layer =>
                    {
                        layer.Spacing = 10;
                    },
                },
                new LayerLoader<LyricLayer>
                {
                    OnLoad = layer =>
                    {
                        layer.LyricPosition = new Vector2(border);
                    },
                },
                new LayerLoader<EditLyricLayer>(),
                new LayerLoader<TimeTagLayer>(),
                new LayerLoader<CaretLayer>(),
                new LayerLoader<BlueprintLayer>(),
            },
        });
    }

    [BackgroundDependencyLoader]
    private void load(ILyricCaretState lyricCaretState, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
    {
        bindableFocusedLyric.BindTo(lyricCaretState.BindableFocusedLyric);

        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.FontSizeInComposer, bindableFontSize);
    }

    private partial class DragContainer : Container
    {
        protected override bool OnDragStart(DragStartEvent e) => true;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

        protected override bool ComputeIsMaskedAway(RectangleF maskingBounds) => false;

        protected override void OnDrag(DragEvent e)
        {
            if (!e.AltPressed)
                return;

            Position += e.Delta;
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            if (!e.AltPressed)
                return false;

            var triggerKey = e.ScrollDelta.Y > 0 ? KaraokeEditAction.DecreasePreviewFontSize : KaraokeEditAction.IncreasePreviewFontSize;
            return trigger(triggerKey);

            bool trigger(KaraokeEditAction action)
            {
                var inputManager = this.FindClosestParent<KeyBindingContainer<KaraokeEditAction>>();
                if (inputManager == null)
                    return false;

                inputManager.TriggerPressed(action);
                inputManager.TriggerReleased(action);
                return true;
            }
        }
    }

    public partial class ScrollBackButton : OsuHoverContainer, IHasPopover
    {
        private const int fade_duration = 500;

        private Visibility state;

        public Visibility State
        {
            get => state;
            set
            {
                if (value == state)
                    return;

                state = value;
                Enabled.Value = state == Visibility.Visible;
                this.FadeTo(state == Visibility.Visible ? 1 : 0, fade_duration, Easing.OutQuint);
            }
        }

        protected override IEnumerable<Drawable> EffectTargets => new[] { background };

        private Color4 flashColour;

        private readonly Container content;
        private readonly Box background;
        private readonly SpriteIcon spriteIcon;

        protected override HoverSounds CreateHoverSounds(HoverSampleSet sampleSet) => new();

        public ScrollBackButton()
        {
            Add(content = new CircularContainer
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Masking = true,
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Shadow,
                    Offset = new Vector2(0f, 1f),
                    Radius = 3f,
                    Colour = Color4.Black.Opacity(0.25f),
                },
                Children = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    spriteIcon = new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(15),
                        Icon = FontAwesome.Solid.Lightbulb,
                    },
                },
            });

            TooltipText = "Hover to see the tutorial";
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider, AudioManager audio)
        {
            IdleColour = colourProvider.Background6;
            HoverColour = colourProvider.Background5;
            flashColour = colourProvider.Light1;
        }

        protected override bool OnClick(ClickEvent e)
        {
            background.FlashColour(flashColour, 800, Easing.OutQuint);

            this.ShowPopover();
            return base.OnClick(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            content.ScaleTo(1.1f, 2000, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            content.ScaleTo(1, 1000, Easing.OutElastic);
            base.OnHoverLost(e);
        }

        public Popover GetPopover() => new DescriptionPopover();

        private partial class DescriptionPopover : OsuPopover
        {
            public DescriptionPopover()
            {
                Child = new DescriptionTextFlowContainer
                {
                    Size = new Vector2(200, 100),
                    Description = "Press `alt` and `drag the compose area` or `scroll the mouse wheel` can move the lyric position or change the font size.",
                };
            }
        }
    }
}
