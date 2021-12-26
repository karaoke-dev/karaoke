// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public abstract class AutoGenerateSection : Section
    {
        protected sealed override string Title => "Auto generate";

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap, ILyricSelectionState lyricSelectionState, OsuColour colours)
        {
            Schedule(() =>
            {
                var disableSelectingLyrics = GetDisableSelectingLyrics(beatmap.HitObjects.OfType<Lyric>());

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
                                new AutoGenerateButton
                                {
                                    StartSelecting = () => disableSelectingLyrics
                                },
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
                    CreateInvalidLyricAlertTextContainer().With(t =>
                    {
                        t.RelativeSizeAxes = Axes.X;
                        t.AutoSizeAxes = Axes.Y;
                        t.Colour = colours.GrayF;
                        t.Alpha = disableSelectingLyrics.Any() ? 1 : 0;
                        t.Padding = new MarginPadding { Horizontal = 20 };
                    })
                };
            });

            lyricSelectionState.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                    return;

                Apply();
            };
        }

        protected abstract Dictionary<Lyric, string> GetDisableSelectingLyrics(IEnumerable<Lyric> lyrics);

        protected abstract void Apply();

        protected abstract InvalidLyricAlertTextContainer CreateInvalidLyricAlertTextContainer();

        protected abstract ConfigButton CreateConfigButton();

        private class AutoGenerateButton : SelectLyricButton
        {
            protected override string StandardText => "Generate";

            protected override string SelectingText => "Cancel generate";
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

        protected abstract class InvalidLyricAlertTextContainer : CustomizableTextContainer
        {
            [Resolved]
            private ILyricEditorState state { get; set; }

            protected void SwitchToEditorMode(string name, string text, LyricEditorMode targetMode)
            {
                AddIconFactory(name, () => new ClickableSpriteText
                {
                    Text = text,
                    Action = () => state.NavigateToFix(targetMode),
                });
            }

            protected override SpriteText CreateSpriteText()
                => base.CreateSpriteText().With(x => x.Font = x.Font.With(size: 16));

            internal class ClickableSpriteText : OsuSpriteText
            {
                public Action Action { get; set; }

                protected override bool OnClick(ClickEvent e)
                {
                    Action?.Invoke();
                    return base.OnClick(e);
                }

                [BackgroundDependencyLoader]
                private void load(OsuColour colours)
                {
                    Colour = colours.Yellow;
                }
            }
        }
    }
}
