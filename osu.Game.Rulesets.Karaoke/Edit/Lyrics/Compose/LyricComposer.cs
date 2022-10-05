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
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose
{
    public class LyricComposer : CompositeDrawable
    {
        private readonly Bindable<PanelLayout> bindablePanelLayout = new();
        private readonly Bindable<BottomEditorType?> bindableBottomEditorType = new();

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<TimeTagEditMode> bindableTimeTagEditMode = new Bindable<TimeTagEditMode>();

        private readonly IDictionary<PanelType, Bindable<bool>> panelStatus = new Dictionary<PanelType, Bindable<bool>>();
        private readonly IDictionary<PanelType, Panel> panelInstance = new Dictionary<PanelType, Panel>();

        private readonly IDictionary<PanelDirection, List<PanelType>> panelDirections = new Dictionary<PanelDirection, List<PanelType>>
        {
            { PanelDirection.Left, new List<PanelType>() },
            { PanelDirection.Right, new List<PanelType>() },
        };

        [Resolved, AllowNull]
        private LyricEditorColourProvider colourProvider { get; set; }

        private readonly Container centerEditArea;
        private readonly Container mainEditorArea;

        private readonly Container bottomEditArea;
        private readonly Container<BaseBottomEditor> bottomEditorContainer;

        public LyricComposer()
        {
            Box centerEditorBackground;
            Box bottomEditorBackground;

            InternalChildren = new Drawable[]
            {
                centerEditArea = new Container
                {
                    Name = "Edit area and action buttons",
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        centerEditorBackground = new Box
                        {
                            Name = "Background",
                            RelativeSizeAxes = Axes.Both,
                        },
                        mainEditorArea = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new LyricEditor(),
                                new SpecialActionToolbar
                                {
                                    Name = "Toolbar",
                                    Anchor = Anchor.BottomCentre,
                                    Origin = Anchor.BottomCentre,
                                },
                            }
                        }
                    }
                },
                bottomEditArea = new Container
                {
                    Name = "Edit area and action buttons",
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Children = new Drawable[]
                    {
                        bottomEditorBackground = new Box
                        {
                            Name = "Background",
                            RelativeSizeAxes = Axes.Both,
                        },
                        bottomEditorContainer = new Container<BaseBottomEditor>
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                        }
                    }
                },
            };

            bindableMode.BindValueChanged(x =>
            {
                toggleChangeBottomEditor();

                Schedule(() =>
                {
                    centerEditorBackground.Colour = colourProvider.Background1(x.NewValue);
                    bottomEditorBackground.Colour = colourProvider.Background5(x.NewValue);
                });
            }, true);

            bindableTimeTagEditMode.BindValueChanged(_ =>
            {
                toggleChangeBottomEditor();
            });

            initializePanel();

            bindablePanelLayout.BindValueChanged(e =>
            {
                assignPanelPosition(e.NewValue);
            }, true);

            bindableBottomEditorType.BindValueChanged(e =>
            {
                assignBottomEditor(e.NewValue);
            });

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

        [BackgroundDependencyLoader(true)]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager, ILyricEditorState state, ITimeTagModeState timeTagModeState)
        {
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.ShowPropertyPanelInComposer, panelStatus[PanelType.Property]);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.ShowInvalidInfoInComposer, panelStatus[PanelType.InvalidInfo]);

            bindableMode.BindTo(state.BindableMode);

            bindableTimeTagEditMode.BindTo(timeTagModeState.BindableEditMode);
        }

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            if (invalidation.HasFlagFast(Invalidation.DrawSize) && source == InvalidationSource.Parent)
                calculatePanelPosition();

            return base.OnInvalidate(invalidation, source);
        }

        #region Panel

        private void initializePanel()
        {
            foreach (var panelType in EnumUtils.GetValues<PanelType>())
            {
                var instance = getInstance(panelType);

                panelStatus.Add(panelType, new Bindable<bool>(true));
                panelInstance.Add(panelType, instance);

                centerEditArea.Add(instance);
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

            calculateLyricEditorSize();
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
            calculateLyricEditorSize();
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

        private void calculateLyricEditorSize()
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

            mainEditorArea.Padding = padding;

            float getWidth(Panel panel)
                => panel.State.Value == Visibility.Visible ? panel.Width : 0;
        }

        #endregion

        #region Bottom editor

        private void toggleChangeBottomEditor()
        {
            bindableBottomEditorType.Value = getBottomEditorType(bindableMode.Value, bindableTimeTagEditMode.Value);

            static BottomEditorType? getBottomEditorType(LyricEditorMode mode, TimeTagEditMode timeTagEditMode) =>
                mode switch
                {
                    LyricEditorMode.EditTimeTag when timeTagEditMode == TimeTagEditMode.Recording => BottomEditorType.RecordingTimeTag,
                    LyricEditorMode.EditTimeTag when timeTagEditMode == TimeTagEditMode.Adjust => BottomEditorType.AdjustTimeTags,
                    LyricEditorMode.EditNote => BottomEditorType.Note,
                    _ => null
                };
        }

        private void assignBottomEditor(BottomEditorType? bottomEditorType)
        {
            bottomEditorContainer.Clear();

            var bottomEditor = createBottomEditor(bottomEditorType);
            if (bottomEditor != null)
                bottomEditorContainer.Add(bottomEditor);

            calculateBottomEditAreaSize(bottomEditor);

            static BaseBottomEditor? createBottomEditor(BottomEditorType? bottomEditorType) =>
                bottomEditorType switch
                {
                    BottomEditorType.RecordingTimeTag => new RecordingTimeTagBottomEditor(),
                    BottomEditorType.AdjustTimeTags => new AdjustTimeTagBottomEditor(),
                    BottomEditorType.Note => new NoteBottomEditor(),
                    _ => null
                };
        }

        private void calculateBottomEditAreaSize(BaseBottomEditor? bottomEditor)
        {
            float bottomEditorHeight = bottomEditor?.ContentHeight ?? 0;
        }

        #endregion

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

        private enum BottomEditorType
        {
            RecordingTimeTag,

            AdjustTimeTags,

            Note,
        }
    }
}
