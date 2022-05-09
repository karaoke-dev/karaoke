// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using System.Linq;
using Markdig;
using Markdig.Syntax;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Containers.Markdown;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Containers.Markdown;
using osu.Game.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class SaitenStatus : FillFlowContainer, IMarkdownTextComponent
    {
        private const float size = 22;

        private readonly SpriteIcon icon;
        private readonly MarkdownTextFlowContainer messageText;

        public SpriteText CreateSpriteText() => new OsuSpriteText();

        public SaitenStatus(SaitenStatusMode statusMode)
        {
            Spacing = new Vector2(5);
            Direction = FillDirection.Horizontal;
            AutoSizeAxes = Axes.Both;
            SaitenStatusMode = statusMode;

            Children = new Drawable[]
            {
                icon = new SpriteIcon
                {
                    Size = new Vector2(size),
                },
                messageText = new OsuMarkdownTextFlowContainer
                {
                    RelativeSizeAxes = Axes.None,
                    AutoSizeAxes = Axes.Both
                }
            };
        }

        private SaitenStatusMode statusMode;

        public SaitenStatusMode SaitenStatusMode
        {
            get => statusMode;
            set
            {
                statusMode = value;

                Schedule(() =>
                {
                    bool saitenable = statusMode == SaitenStatusMode.Saitening;
                    icon.Icon = saitenable ? FontAwesome.Regular.DotCircle : FontAwesome.Regular.PauseCircle;
                    icon.Colour = saitenable ? Color4.Red : Color4.LightGray;

                    string text = GetSaitenStatusText(statusMode);
                    var block = Markdown.Parse(text).OfType<ParagraphBlock>().FirstOrDefault();

                    messageText.Clear();
                    if (block != null)
                        messageText.AddInlineText(block.Inline);
                });
            }
        }

        protected virtual string GetSaitenStatusText(SaitenStatusMode statusMode)
        {
            return statusMode switch
            {
                SaitenStatusMode.AndroidMicrophonePermissionDeclined => "Go to setting to open permission for lazer.",
                SaitenStatusMode.AndroidDoesNotSupported => "Android device haven't support saiten system yet :(",
                SaitenStatusMode.IOSMicrophonePermissionDeclined => "Go to setting to open permission for lazer.",
                SaitenStatusMode.IOSDoesNotSupported => "iOS device haven't support saiten system yet :(",
                SaitenStatusMode.OSXMicrophonePermissionDeclined => "Go to setting to open permission for lazer.",
                SaitenStatusMode.OSXDoesNotSupported => "Osx device haven't support saiten system yet :(",
                SaitenStatusMode.WindowsMicrophonePermissionDeclined => "Open lazer with admin permission to enable saiten system.",
                SaitenStatusMode.NotSaitening => "This beatmap is not saitenable.",
                SaitenStatusMode.AutoPlay => "Auto play mode.",
                SaitenStatusMode.Edit => "Edit mode.",
                SaitenStatusMode.Saitening => "Saitening...",
                SaitenStatusMode.NotInitialized => "Seems microphone device is not ready.",
                _ => "Weird... Should not goes to here either :oops:"
            };
        }
    }

    public enum SaitenStatusMode
    {
        /// <summary>
        /// Due to android device does not authorize microphone access.
        /// </summary>
        [Description("Android permission declined.")]
        AndroidMicrophonePermissionDeclined,

        /// <summary>
        /// Saiten system does not support android device.
        /// Will throw this if osu.framework.microphone does not support it yet.
        /// Or official client does not open this permission.
        /// </summary>
        [Description("Android target not supported.")]
        AndroidDoesNotSupported,

        /// <summary>
        /// Due to iOS device does not authorize microphone access.
        /// </summary>
        [Description("iOS permission declined.")]
        IOSMicrophonePermissionDeclined,

        /// <summary>
        /// Saiten system does not support iOS device.
        /// Will throw this if osu.framework.microphone does not support it yet.
        /// Or official client does not open this permission.
        /// </summary>
        [Description("iOS target not supported.")]
        IOSDoesNotSupported,

        /// <summary>
        /// Due to osx device does not authorize microphone access.
        /// </summary>
        [Description("osx permission declined.")]
        OSXMicrophonePermissionDeclined,

        /// <summary>
        /// Saiten system does not support osx device.
        /// Will throw this if osu.framework.microphone does not support it yet.
        /// Or official client does not open this permission.
        /// </summary>
        [Description("osx target not supported.")]
        OSXDoesNotSupported,

        /// <summary>
        /// Due to windows device does not authorize microphone access.
        /// Windows client don't need to ask permission.
        /// Open lazer client with admin permission can solve that.
        /// </summary>
        [Description("Windows permission declined.")]
        WindowsMicrophonePermissionDeclined,

        /// <summary>
        /// No microphone device in this computer/macbook.
        /// </summary>
        [Description("No microphone device.")]
        NoMicrophoneDevice,

        /// <summary>
        /// Beatmap is not scoring.
        /// </summary>
        [Description("No saitening.")]
        NotSaitening,

        /// <summary>
        /// Beatmap is not scoring.
        /// </summary>
        [Description("Autoplay.")]
        AutoPlay,

        /// <summary>
        /// In edit mode.
        /// </summary>
        [Description("Edit mode.")]
        Edit,

        /// <summary>
        /// Everything works well.
        /// </summary>
        [Description("Saitening...")]
        Saitening,

        /// <summary>
        /// Microphone saiten is not initialized.
        /// </summary>
        [Description("Not initialized.")]
        NotInitialized,
    }
}
