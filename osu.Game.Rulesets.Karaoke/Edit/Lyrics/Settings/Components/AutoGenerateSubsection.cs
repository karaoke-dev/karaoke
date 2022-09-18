// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components
{
    public abstract class AutoGenerateSubsection : FillFlowContainer
    {
        private const int horizontal_padding = 20;

        private readonly LyricAutoGenerateProperty autoGenerateProperty;

        protected AutoGenerateSubsection(LyricAutoGenerateProperty autoGenerateProperty)
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            this.autoGenerateProperty = autoGenerateProperty;
        }

        [BackgroundDependencyLoader]
        private void load(ILyricAutoGenerateChangeHandler lyricAutoGenerateChangeHandler, OsuColour colours)
        {
            // should wait until BDL in the parent class has been loaded.
            Schedule(() =>
            {
                Children = new Drawable[]
                {
                    new GridContainer
                    {
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        ColumnDimensions = new[]
                        {
                            new Dimension(),
                            new Dimension(GridSizeMode.Absolute, 5),
                            new Dimension(GridSizeMode.Absolute, 36)
                        },
                        RowDimensions = new[]
                        {
                            new Dimension(GridSizeMode.AutoSize)
                        },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                new AutoGenerateButton(autoGenerateProperty),
                                null,
                                CreateConfigButton().With(x =>
                                {
                                    x.Anchor = Anchor.Centre;
                                    x.Origin = Anchor.Centre;
                                    x.Size = new Vector2(36);
                                })
                            }
                        },
                    },
                    new DescriptionTextFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Padding = new MarginPadding { Horizontal = horizontal_padding },
                        Description = CreateInvalidLyricDescriptionFormat()
                    }
                };
            });
        }

        protected abstract DescriptionFormat CreateInvalidLyricDescriptionFormat();

        protected abstract ConfigButton CreateConfigButton();

        private class AutoGenerateButton : SelectLyricButton
        {
            [Resolved]
            private ILyricAutoGenerateChangeHandler lyricAutoGenerateChangeHandler { get; set; }

            private readonly LyricAutoGenerateProperty autoGenerateProperty;

            public AutoGenerateButton(LyricAutoGenerateProperty autoGenerateProperty)
            {
                this.autoGenerateProperty = autoGenerateProperty;
            }

            protected override LocalisableString StandardText => "Generate";

            protected override LocalisableString SelectingText => "Cancel generate";

            protected override IDictionary<Lyric, LocalisableString> GetDisableSelectingLyrics()
            {
                return lyricAutoGenerateChangeHandler.GetNotGeneratableLyrics(autoGenerateProperty);
            }

            protected override void Apply()
            {
                lyricAutoGenerateChangeHandler.AutoGenerate(autoGenerateProperty);
            }
        }

        protected abstract class ConfigButton : IconButton, IHasPopover
        {
            protected ConfigButton()
            {
                Icon = FontAwesome.Solid.Cog;
                Action = openConfigSetting;

                void openConfigSetting()
                    => this.ShowPopover();
            }

            public abstract Popover GetPopover();
        }

        protected abstract class MultiConfigButton : ConfigButton
        {
            private KaraokeRulesetEditGeneratorSetting? selectedSetting;

            protected MultiConfigButton()
            {
                Action = this.ShowPopover;
            }

            public override Popover GetPopover()
            {
                if (selectedSetting == null)
                    return createSelectionPopover();

                return GetPopoverBySettingType(selectedSetting.Value);
            }

            protected abstract IEnumerable<KaraokeRulesetEditGeneratorSetting> AvailableSettings { get; }

            protected abstract string GetDisplayName(KaraokeRulesetEditGeneratorSetting setting);

            protected abstract Popover GetPopoverBySettingType(KaraokeRulesetEditGeneratorSetting setting);

            private Popover createSelectionPopover()
                => new OsuPopover
                {
                    Child = new FillFlowContainer<OsuButton>
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(10),
                        Children = AvailableSettings.Select(x =>
                        {
                            string name = GetDisplayName(x);
                            return new OsuButton
                            {
                                Text = name,
                                Width = 150,
                                Action = () =>
                                {
                                    selectedSetting = x;
                                    this.ShowPopover();

                                    // after show config pop-over, should make the state back for able to show this dialog next time.
                                    selectedSetting = null;
                                },
                            };
                        }).ToList()
                    }
                };
        }
    }
}
