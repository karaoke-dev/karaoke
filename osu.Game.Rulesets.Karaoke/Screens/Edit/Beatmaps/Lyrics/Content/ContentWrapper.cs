// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.List;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content;

/// <summary>
/// Wrapper for able to change the different type of <see cref="LyricEditorLayout"/>
/// </summary>
public partial class ContentWrapper : CompositeDrawable
{
    private readonly LoadingSpinner loading;

    public ContentWrapper()
    {
        InternalChildren = new[]
        {
            loading = new LoadingSpinner(true)
            {
                Depth = int.MinValue,
            },
        };
    }

    public void SwitchLayout(LyricEditorLayout layout)
    {
        loading.Show();

        // should switch the layout after loaded.
        Schedule(() =>
        {
            var newContent = getContent(layout).With(x =>
            {
                x.RelativeSizeAxes = Axes.Both;
            });

            var wrapper = new DelayedLoadWrapper(newContent).With(x =>
            {
                x.RelativeSizeAxes = Axes.Both;
                x.RelativePositionAxes = Axes.Y;
                x.Y = -0.5f;
                x.Alpha = 0;
            });

            LoadComponentAsync(wrapper, content =>
            {
                const double remove_old_editor_time = 300;
                const double new_animation_time = 1000;

                var oldComponent = InternalChildren.Where(x => x != loading).OfType<DelayedLoadWrapper>().FirstOrDefault();
                oldComponent?.MoveToY(-0.5f, remove_old_editor_time).FadeOut(remove_old_editor_time).OnComplete(x =>
                {
                    x.Expire();
                });

                AddInternal(content);
                content.Delay(oldComponent != null ? remove_old_editor_time : 0)
                       .Then()
                       .FadeIn(new_animation_time)
                       .MoveToY(0, new_animation_time)
                       .OnComplete(_ =>
                       {
                           loading.Hide();
                       });
            });
        });
    }

    private static Container getContent(LyricEditorLayout layout) =>
        layout switch
        {
            LyricEditorLayout.List => new Container
            {
                Children = new[]
                {
                    new PreviewLyricList
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                },
            },
            LyricEditorLayout.Compose => new Container
            {
                Children = new Drawable[]
                {
                    new LyricComposer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Size = new Vector2(1, 0.6f),
                    },
                    new DetailLyricList
                    {
                        RelativePositionAxes = Axes.Y,
                        Position = new Vector2(0, 0.6f),
                        Size = new Vector2(1, 0.4f),
                        RelativeSizeAxes = Axes.Both,
                    },
                },
            },
            _ => throw new ArgumentOutOfRangeException(nameof(layout), layout, null),
        };
}
