// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics;
using System.Linq;
using Markdig.Syntax.Inlines;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers.Markdown;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;

public partial class SwitchMoteText : OsuMarkdownLinkText
{
    [Resolved]
    private ILyricEditorState? state { get; set; }

    private readonly SwitchModeDescriptionAction switchModeDescriptionAction;

    public SwitchMoteText(SwitchModeDescriptionAction switchModeDescriptionAction)
        : base(switchModeDescriptionAction.Text.ToString(), new LinkInline { Title = "Click to change the edit mode." })
    {
        this.switchModeDescriptionAction = switchModeDescriptionAction;

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

        state?.NavigateToFix(switchModeDescriptionAction.Mode);
    }
}
