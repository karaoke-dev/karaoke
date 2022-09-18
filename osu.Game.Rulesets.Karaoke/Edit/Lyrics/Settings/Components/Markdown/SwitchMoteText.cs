// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Markdig.Syntax.Inlines;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers.Markdown;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components.Markdown
{
    public class SwitchMoteText : OsuMarkdownLinkText
    {
        [Resolved, AllowNull]
        private ILyricEditorState state { get; set; }

        private readonly SwitchMode switchMode;

        public SwitchMoteText(SwitchMode switchMode)
            : base(switchMode.Text.ToString(), new LinkInline { Title = "Click to change the edit mode." })
        {
            this.switchMode = switchMode;

            CornerRadius = 4;
            Masking = true;
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            AddInternal(new Box
            {
                Name = "Background",
                Depth = 1,
                RelativeSizeAxes = Axes.Both,
                Colour = colourProvider.Background6,
            });

            var spriteText = InternalChildren.OfType<OsuSpriteText>().FirstOrDefault();
            Debug.Assert(spriteText != null);

            spriteText.Padding = new MarginPadding { Horizontal = 4 };
        }

        protected override void OnLinkPressed()
        {
            base.OnLinkPressed();

            state.NavigateToFix(switchMode.Mode);
        }
    }
}
