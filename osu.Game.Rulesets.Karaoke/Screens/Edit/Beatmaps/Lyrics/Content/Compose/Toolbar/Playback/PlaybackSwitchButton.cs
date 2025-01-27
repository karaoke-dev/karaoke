// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar.Playback;

public partial class PlaybackSwitchButton : CompositeDrawable
{
    private readonly BindableNumber<double> freqAdjust = new BindableDouble(1);

    [Resolved]
    private EditorClock editorClock { get; set; } = null!;

    public PlaybackSwitchButton()
    {
        Height = SpecialActionToolbar.HEIGHT;
        AutoSizeAxes = Axes.X;
        InternalChild = new PlaybackTabControl
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Current = freqAdjust,
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        editorClock.AudioAdjustments.AddAdjustment(AdjustableProperty.Frequency, freqAdjust);
    }

    protected override void Dispose(bool isDisposing)
    {
        editorClock.AudioAdjustments.RemoveAdjustment(AdjustableProperty.Frequency, freqAdjust);

        base.Dispose(isDisposing);
    }

    private partial class PlaybackTabControl : OsuTabControl<double>
    {
        private static readonly double[] tempo_values = { 0.25, 0.5, 0.75, 1 };

        protected override Dropdown<double>? CreateDropdown() => null;

        protected override TabItem<double> CreateTabItem(double value) => new PlaybackTabItem(value);

        protected override TabFillFlowContainer CreateTabFlow() => new()
        {
            AutoSizeAxes = Axes.Both,
            Spacing = new Vector2(SpecialActionToolbar.SPACING),
            Direction = FillDirection.Horizontal,
        };

        public PlaybackTabControl()
        {
            AutoSizeAxes = Axes.Both;

            tempo_values.ForEach(AddItem);
        }

        public partial class PlaybackTabItem : TabItem<double>
        {
            private const float fade_duration = 200;

            private readonly OsuSpriteText text;

            public PlaybackTabItem(double value)
                : base(value)
            {
                Size = new Vector2(SpecialActionToolbar.ICON_SIZE);

                Children = new Drawable[]
                {
                    text = new OsuSpriteText
                    {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Text = $"{value:0%}",
                    },
                };

                updateState();
            }

            protected override void OnActivated() => updateState();
            protected override void OnDeactivated() => updateState();

            private void updateState()
            {
                bool active = Active.Value;

                text.Alpha = active ? 1 : 0.5f;
                text.Font = OsuFont.GetFont(size: 14, weight: active ? FontWeight.Bold : FontWeight.Medium);
            }
        }
    }
}
