// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
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
        private void load(EditorBeatmap beatmap, LyricSelectionState lyricSelectionState, OsuColour colours)
        {
            Schedule(() =>
            {
                var disableSelectingLyrics = GetDisableSelectingLyrics(beatmap.HitObjects.OfType<Lyric>().ToArray());

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
                                new IconButton
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Size = new Vector2(36),
                                    Icon = FontAwesome.Solid.Cog,
                                    Action = OpenConfigSetting,
                                },
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

                var selectedLyrics = lyricSelectionState.SelectedLyrics.ToArray();
                Apply(selectedLyrics);
            };
        }

        protected abstract Dictionary<Lyric, string> GetDisableSelectingLyrics(Lyric[] lyrics);

        protected abstract void Apply(Lyric[] lyrics);

        protected abstract InvalidLyricAlertTextContainer CreateInvalidLyricAlertTextContainer();

        protected virtual void OpenConfigSetting()
        {
            // todo : change to abstract class and force to implement.
        }

        private class AutoGenerateButton : SelectLyricButton
        {
            protected override string StandardText => "Generate";

            protected override string SelectingText => "Cancel generate";
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
