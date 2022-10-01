// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.EnumExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Layout;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose
{
    public class LyricComposer : CompositeDrawable
    {
        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly Bindable<PanelLayout> bindablePanelLayout = new();

        private readonly IDictionary<PanelType, Bindable<bool>> panelStatus = new Dictionary<PanelType, Bindable<bool>>();

        private readonly IDictionary<PanelType, Panel> panelInstance = new Dictionary<PanelType, Panel>();

        private readonly IDictionary<PanelDirection, List<PanelType>> panelDirections = new Dictionary<PanelDirection, List<PanelType>>
        {
            { PanelDirection.Left, new List<PanelType>() },
            { PanelDirection.Right, new List<PanelType>() },
        };

        [Resolved, AllowNull]
        private LyricEditorColourProvider colourProvider { get; set; }

        private readonly Box background;
        private readonly Container mainEditArea;
        private readonly Container lyricEditor;

        public LyricComposer()
        {
            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                },
                mainEditArea = new Container
                {
                    Name = "Edit area and action buttons",
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        lyricEditor = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new[]
                            {
                                new SpecialActionToolbar
                                {
                                    Name = "Toolbar",
                                    Anchor = Anchor.BottomCentre,
                                    Origin = Anchor.BottomCentre,
                                }
                            }
                        }
                    }
                },
            };

            bindableMode.BindValueChanged(x =>
            {
                Schedule(() =>
                {
                    background.Colour = colourProvider.Background1(x.NewValue);
                });
            }, true);

            initializePanel();

            bindablePanelLayout.BindValueChanged(e =>
            {
                assignPanelPosition(e.NewValue);
            }, true);

            foreach (var (type, bindable) in panelStatus)
            {
                bindable.BindValueChanged(e =>
                {
                    bool show = e.NewValue;

                    if (show)
                    {
                        closeOtherPanelsInTheSameDirection(type);
                    }

                    togglePanel(type, show);
                }, true);
            }
        }

        private void initializePanel()
        {
            foreach (var panelType in EnumUtils.GetValues<PanelType>())
            {
                var instance = getInstance(panelType);

                panelStatus.Add(panelType, new Bindable<bool>(true));
                panelInstance.Add(panelType, instance);

                mainEditArea.Add(instance);
            }

            static Panel getInstance(PanelType panelType) =>
                panelType switch
                {
                    PanelType.Property => new PropertyPanel(),
                    PanelType.InvalidInfo => new InvalidPanel(),
                    _ => throw new ArgumentOutOfRangeException(nameof(panelType), panelType, null)
                };
        }

        private void togglePanel(PanelType panel, bool show)
        {
            panelInstance[panel].State.Value = show ? Visibility.Visible : Visibility.Hidden;

            calculateEditAreaSize();
        }

        [BackgroundDependencyLoader(true)]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager, ILyricEditorState state)
        {
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.ShowPropertyPanelInComposer, panelStatus[PanelType.Property]);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.ShowInvalidInfoInComposer, panelStatus[PanelType.InvalidInfo]);

            bindableMode.BindTo(state.BindableMode);
        }

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            if (invalidation.HasFlagFast(Invalidation.DrawSize) && source == InvalidationSource.Parent)
                calculatePanelPosition();

            return base.OnInvalidate(invalidation, source);
        }

        private void calculatePanelPosition()
        {
            float radio = DrawWidth / DrawHeight;
            bindablePanelLayout.Value = radio < 2 ? PanelLayout.LeftOnly : PanelLayout.LeftAndRight;
        }

        private void assignPanelPosition(PanelLayout panelLayout)
        {
            panelDirections[PanelDirection.Left].Clear();
            panelDirections[PanelDirection.Right].Clear();

            switch (panelLayout)
            {
                case PanelLayout.LeftAndRight:
                    panelDirections[PanelDirection.Left].Add(PanelType.Property);
                    panelDirections[PanelDirection.Right].Add(PanelType.InvalidInfo);
                    break;

                case PanelLayout.LeftOnly:
                    panelDirections[PanelDirection.Left].Add(PanelType.Property);
                    panelDirections[PanelDirection.Left].Add(PanelType.InvalidInfo);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(panelLayout), panelLayout, null);
            }

            foreach (var (direction, panelTypes) in panelDirections)
            {
                foreach (Panel instance in panelTypes.Select(panelType => panelInstance[panelType]))
                {
                    instance.Direction = direction;
                }
            }

            closeOtherPanelsInTheSameDirection(PanelType.Property);
            calculateEditAreaSize();
        }

        private void closeOtherPanelsInTheSameDirection(PanelType exceptPanel)
        {
            var closePanelList = panelDirections.First(x => x.Value.Contains(exceptPanel)).Value;

            foreach (var panel in closePanelList.Where(x => x != exceptPanel))
            {
                var status = panelStatus[panel];
                status.Value = false;
            }
        }

        private void calculateEditAreaSize()
        {
            var padding = new MarginPadding();

            foreach (var (position, panelTypes) in panelDirections)
            {
                var instances = panelTypes.Select(panelType => panelInstance[panelType]).ToArray();
                float maxWidth = instances.Any() ? instances.Max(getWidth) : 0;

                switch (position)
                {
                    case PanelDirection.Left:
                        padding.Left = maxWidth;
                        break;

                    case PanelDirection.Right:
                        padding.Right = maxWidth;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(position), position, null);
                }
            }

            lyricEditor.Padding = padding;

            float getWidth(Panel panel)
                => panel.State.Value == Visibility.Visible ? panel.Width : 0;
        }

        private enum PanelType
        {
            Property,

            InvalidInfo,
        }

        private enum PanelLayout
        {
            LeftAndRight,

            LeftOnly,
        }
    }
}
