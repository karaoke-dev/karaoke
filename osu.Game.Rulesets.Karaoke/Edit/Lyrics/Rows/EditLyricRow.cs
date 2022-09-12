// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Info;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows
{
    [Cached(typeof(IEditLyricRowState))]
    public class EditLyricRow : LyricEditorRow, IEditLyricRowState
    {
        private const int min_height = 75;

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<int> bindableLyricPropertyWritableVersion;

        public event Action<LyricEditorMode> WritableVersionChanged;

        public EditLyricRow(Lyric lyric)
            : base(lyric)
        {
            AutoSizeAxes = Axes.Y;

            bindableLyricPropertyWritableVersion = lyric.LyricPropertyWritableVersion.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            bindableMode.BindTo(state.BindableMode);

            bindableMode.BindValueChanged(x =>
            {
                WritableVersionChanged?.Invoke(bindableMode.Value);
            });

            bindableLyricPropertyWritableVersion.BindValueChanged(_ =>
            {
                WritableVersionChanged?.Invoke(bindableMode.Value);
            });
        }

        protected override Drawable CreateLyricInfo(Lyric lyric)
        {
            return new InfoControl(lyric)
            {
                // todo : cannot use relative size to both because it will cause size cannot roll-back if make lyric smaller.
                RelativeSizeAxes = Axes.X,
                Height = min_height,
            };
        }

        protected override Drawable CreateContent(Lyric lyric)
        {
            return new SingleLyricEditor(lyric)
            {
                Margin = new MarginPadding { Left = 10 },
                RelativeSizeAxes = Axes.X,
            };
        }
    }
}
