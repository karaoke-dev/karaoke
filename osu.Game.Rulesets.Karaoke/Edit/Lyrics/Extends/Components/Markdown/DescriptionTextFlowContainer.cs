// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

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

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components.Markdown
{
    public class DescriptionTextFlowContainer : Container, IMarkdownTextComponent
    {
        private readonly DescriptionMarkdownTextFlowContainer description;

        public DescriptionTextFlowContainer()
        {
            AddInternal(description = new DescriptionMarkdownTextFlowContainer(this)
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
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

        internal class DescriptionMarkdownTextFlowContainer : OsuMarkdownTextFlowContainer
        {
            private readonly DescriptionTextFlowContainer descriptionTextFlowContainer;

            public DescriptionMarkdownTextFlowContainer(DescriptionTextFlowContainer parent)
            {
                descriptionTextFlowContainer = parent;
            }

            protected override void AddLinkText(string text, LinkInline linkInline)
            {
                switch (text)
                {
                    case DescriptionFormat.LINK_KEY_INPUT:
                    {
                        var keys = descriptionTextFlowContainer.Description.Keys;
                        string key = linkInline.Url;
                        if (keys == null || !keys.TryGetValue(key, out InputKey inputKey))
                            throw new ArgumentNullException(nameof(keys));

                        AddDrawable(new InputKeyText(inputKey));
                        return;
                    }

                    case DescriptionFormat.LINK_KEY_EDIT_MODE:
                    {
                        var editModes = descriptionTextFlowContainer.Description.EditModes;
                        string key = linkInline.Url;
                        if (editModes == null || !editModes.TryGetValue(key, out SwitchMode mode))
                            throw new ArgumentNullException(nameof(editModes));

                        AddDrawable(new SwitchMoteText(mode));
                        return;
                    }

                    default:
                        base.AddLinkText(text, linkInline);
                        break;
                }
            }
        }
    }
}
