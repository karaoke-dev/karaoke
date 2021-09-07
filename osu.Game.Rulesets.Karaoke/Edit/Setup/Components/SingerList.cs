// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup.Components
{
    /// <summary>
    /// A component which displays a collection of singers in individual <see cref="SingerDisplay"/>s.
    /// </summary>
    public class SingerList : CompositeDrawable
    {
        public BindableList<Singer> Singers { get; } = new();

        private string singerNamePrefix = "Singer";

        public string SingerNamePrefix
        {
            get => singerNamePrefix;
            set
            {
                if (singerNamePrefix == value)
                    return;

                singerNamePrefix = value;

                if (IsLoaded)
                    reindexItems();
            }
        }

        private FillFlowContainer palette;

        private IEnumerable<SingerDisplay> singerDisplays => palette.OfType<SingerDisplay>();

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            AutoSizeDuration = fade_duration;
            AutoSizeEasing = Easing.OutQuint;

            InternalChild = palette = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Spacing = new Vector2(10),
                Direction = FillDirection.Full
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Singers.BindCollectionChanged((_, args) =>
            {
                if (args.Action != NotifyCollectionChangedAction.Replace)
                    updatePalette();
            }, true);
            FinishTransforms(true);
        }

        private const int fade_duration = 200;

        private void updatePalette()
        {
            palette.Clear();

            for (int i = 0; i < Singers.Count; ++i)
            {
                // copy to avoid accesses to modified closure.
                int singerIndex = i;
                SingerDisplay display;

                palette.Add(display = new SingerDisplay
                {
                    Current = { Value = Singers[singerIndex] }
                });

                // todo : might check does this like works because singer is object.
                display.Current.BindValueChanged(singer => Singers[singerIndex] = singer.NewValue);
                display.DeleteRequested += singerDeletionRequested;
            }

            palette.Add(new AddSingerButton
            {
                // todo : use better way to create singer with right id.
                Action = () => Singers.Add(new Singer())
            });

            reindexItems();
        }

        // todo : might have dialog to ask should delete singer or not if contains lyric.
        private void singerDeletionRequested(SingerDisplay display) => Singers.RemoveAt(palette.IndexOf(display));

        private void reindexItems()
        {
            int index = 1;

            foreach (var singerDisplay in singerDisplays)
            {
                // todo : might call singer manager to update singer id?
                index += 1;
            }
        }

        internal class AddSingerButton : CompositeDrawable
        {
            public Action Action
            {
                set => circularButton.Action = value;
            }

            private readonly OsuClickableContainer circularButton;

            public AddSingerButton()
            {
                AutoSizeAxes = Axes.Y;
                Width = 100;

                InternalChild = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 10),
                    Children = new Drawable[]
                    {
                        circularButton = new OsuClickableContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 100,
                            CornerRadius = 50,
                            Masking = true,
                            BorderThickness = 5,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Colour4.Transparent,
                                    AlwaysPresent = true
                                },
                                new SpriteIcon
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.Plus
                                }
                            }
                        },
                        new OsuSpriteText
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Text = "New"
                        }
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                circularButton.BorderColour = colours.BlueDarker;
            }
        }
    }
}
