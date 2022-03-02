// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Markdig.Syntax.Inlines;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input;
using osu.Game.Database;
using osu.Game.Graphics.Containers.Markdown;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Input.Bindings;
using osu.Game.Overlays;
using osu.Game.Overlays.Settings.Sections.Input;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components.Description
{
    /// <summary>
    /// For showing the key and adjust the key binding.
    /// </summary>
    public class InputKeyText : OsuMarkdownLinkText, IHasPopover
    {
        private readonly InputKey inputKey;

        [Resolved]
        private ReadableKeyCombinationProvider keyCombinationProvider { get; set; }

        [Resolved]
        private RealmAccess realm { get; set; }

        public InputKeyText(InputKey inputKey)
            : base(inputKey.Text.ToString(), new LinkInline { Title = "Click to change the key." })
        {
            this.inputKey = inputKey;

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

            updateDisplayText();

            // todo: IDK why not being triggered.
            keyCombinationProvider.KeymapChanged += updateDisplayText;
        }

        private void updateDisplayText()
        {
            string text = string.IsNullOrEmpty(inputKey.Text.ToString())
                ? getKeyName(inputKey.AdjustableActions.FirstOrDefault())
                : inputKey.Text.ToString();

            var spriteText = InternalChildren.OfType<OsuSpriteText>().FirstOrDefault();
            Debug.Assert(spriteText != null);

            spriteText.Text = text;
            spriteText.Padding = new MarginPadding { Horizontal = 4 };

            string getKeyName(KaraokeEditAction action)
            {
                var ruleset = new KaraokeRuleset();
                string rulesetName = ruleset.ShortName;
                const int edit_input_variant = KaraokeRuleset.EDIT_INPUT_VARIANT;

                var keyBinding = realm.Run(r => r.All<RealmKeyBinding>()
                                                 .Where(b => b.RulesetName == rulesetName && b.Variant == edit_input_variant)
                                                 .Detach()).FirstOrDefault(x => (int)x.Action == (int)action);

                if (keyBinding == null)
                    throw new ArgumentNullException(nameof(keyBinding));

                return keyCombinationProvider.GetReadableString(keyBinding.KeyCombination);
            }
        }

        protected override void OnLinkPressed()
        {
            // open the popover
            this.ShowPopover();
        }

        public Popover GetPopover()
        {
            var popover = new OsuPopover
            {
                Child = new PopoverKeyBindingsSubsection(inputKey.AdjustableActions)
                {
                    Width = 300,
                    RelativeSizeAxes = Axes.None,
                }
            };

            // because it's not possible to get the key change event, so at least update the key after popover closed.
            popover.State.BindValueChanged(x =>
            {
                if (x.NewValue == Visibility.Hidden)
                    updateDisplayText();
            });

            return popover;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (keyCombinationProvider != null)
                keyCombinationProvider.KeymapChanged -= updateDisplayText;
        }

        private class PopoverKeyBindingsSubsection : VariantBindingsSubsection
        {
            public PopoverKeyBindingsSubsection(IEnumerable<KaraokeEditAction> actions)
                : base(new KaraokeRuleset().RulesetInfo, KaraokeRuleset.EDIT_INPUT_VARIANT)
            {
                // should only show the keys in the list.
                Defaults = Defaults.Where(x => x.Action is KaraokeEditAction action && actions.Contains(action));
            }
        }
    }
}
