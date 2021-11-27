// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit
{
    /// <summary>
    /// TODO: eventually make this inherit Screen and add a local screen stack inside the Editor.
    /// </summary>
    public abstract class GenericEditorScreen<TType> : VisibilityContainer
    {
        protected override Container<Drawable> Content => content;
        private readonly Container content;

        public readonly TType Type;

        protected GenericEditorScreen(TType type)
        {
            Type = type;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;

            InternalChild = content = new PopoverContainer { RelativeSizeAxes = Axes.Both };
        }

        protected override void PopIn()
        {
            this.ScaleTo(1f, 200, Easing.OutQuint)
                .FadeIn(200, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            this.ScaleTo(0.98f, 200, Easing.OutQuint)
                .FadeOut(200, Easing.OutQuint);
        }

        #region Clipboard operations

        public BindableBool CanCut { get; } = new BindableBool();

        /// <summary>
        /// Performs a "cut to clipboard" operation appropriate for the given screen.
        /// </summary>
        protected virtual void PerformCut()
        {
        }

        public void Cut()
        {
            if (CanCut.Value)
                PerformCut();
        }

        public BindableBool CanCopy { get; } = new BindableBool();

        /// <summary>
        /// Performs a "copy to clipboard" operation appropriate for the given screen.
        /// </summary>
        protected virtual void PerformCopy()
        {
        }

        public virtual void Copy()
        {
            if (CanCopy.Value)
                PerformCopy();
        }

        public BindableBool CanPaste { get; } = new BindableBool();

        /// <summary>
        /// Performs a "paste from clipboard" operation appropriate for the given screen.
        /// </summary>
        protected virtual void PerformPaste()
        {
        }

        public virtual void Paste()
        {
            if (CanPaste.Value)
                PerformPaste();
        }

        #endregion
    }
}
