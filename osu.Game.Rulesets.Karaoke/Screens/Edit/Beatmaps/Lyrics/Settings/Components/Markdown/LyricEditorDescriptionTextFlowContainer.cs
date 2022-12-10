// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Containers.Markdown;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers.Markdown;
using osu.Game.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown
{
    public partial class LyricEditorDescriptionTextFlowContainer : Container, IMarkdownTextComponent
    {
        private readonly DescriptionMarkdownTextFlowContainer description;

        public LyricEditorDescriptionTextFlowContainer()
        {
            AddInternal(description = new DescriptionMarkdownTextFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                AddTextAction = processLinkText,
            });
        }

        private DescriptionFormat descriptionFormat;

        public DescriptionFormat Description
        {
            get => descriptionFormat;
            set
            {
                descriptionFormat = value;

                var markdownDocument = Markdig.Markdown.Parse(descriptionFormat.Text.ToString());
                description.Clear();

                if (markdownDocument.FirstOrDefault() is ParagraphBlock paragraphBlock)
                    description.AddInlineText(paragraphBlock.Inline);
            }
        }

        public SpriteText CreateSpriteText() => new OsuSpriteText
        {
            Font = OsuFont.GetFont(size: 14, weight: FontWeight.Regular)
        };

        private OsuMarkdownLinkText? processLinkText(string text, string? url)
        {
            if (text != DescriptionFormat.LINK_KEY_ACTION)
                return null;

            var keys = Description.Actions;
            if (url == null || !keys.TryGetValue(url, out var inputKey))
                throw new ArgumentNullException(nameof(keys));

            return GetLinkTextByDescriptionAction(inputKey);
        }

        protected virtual OsuMarkdownLinkText GetLinkTextByDescriptionAction(IDescriptionAction descriptionAction) =>
            descriptionAction switch
            {
                InputKeyDescriptionAction inputKey => new InputKeyText(inputKey),
                SwitchModeDescriptionAction switchMode => new SwitchMoteText(switchMode),
                _ => throw new InvalidCastException()
            };

        internal partial class DescriptionMarkdownTextFlowContainer : OsuMarkdownTextFlowContainer
        {
            public Func<string, string?, OsuMarkdownLinkText?>? AddTextAction;

            protected override void AddLinkText(string text, LinkInline linkInline)
            {
                var linkText = AddTextAction?.Invoke(text, linkInline.Url);

                if (linkText != null)
                {
                    AddDrawable(linkText);
                }
                else
                {
                    base.AddLinkText(text, linkInline);
                }
            }
        }
    }
}
