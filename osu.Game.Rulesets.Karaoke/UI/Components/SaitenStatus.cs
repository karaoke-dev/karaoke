// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using System.Linq;
using Markdig;
using Markdig.Syntax;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Containers.Markdown;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    [Cached(Type = typeof(IMarkdownTextComponent))]
    public class SaitenStatus : FillFlowContainer, IMarkdownTextComponent
    {
        private const float size = 22;

        public SpriteText CreateSpriteText() => new SpriteText();

        public SaitenStatus(SaitenStatusMode statusMode)
        {
            Spacing = new Vector2(5);
            Direction = FillDirection.Horizontal;
            AutoSizeAxes = Axes.Both;
            SaitenStatusMode = statusMode;
        }

        private SaitenStatusMode statusMode;

        public SaitenStatusMode SaitenStatusMode
        {
            get => statusMode;
            set
            {
                statusMode = value;
                Children = new[]
                {
                    createIcon(statusMode == SaitenStatusMode.Saitening),
                    createStatusSpriteText(GetSaitenStatusText(statusMode))
                };
            }
        }

        protected virtual string GetSaitenStatusText(SaitenStatusMode statusMode)
        {
            switch (statusMode)
            {
                case SaitenStatusMode.AndroidMicrophonePermissionDeclined:
                    return "Go to setting to open permission for lazer.";

                case SaitenStatusMode.AndroidDoesNotSupported:
                    return "Android device haven't support saiten system yet :(";

                case SaitenStatusMode.IOSMicrophonePermissionDeclined:
                    return "Go to setting to open permission for lazer.";

                case SaitenStatusMode.IOSDoesNotSupported:
                    return "iOS device haven't support saiten system yet :(";

                case SaitenStatusMode.OSXMicrophonePermissionDeclined:
                    return "Go to setting to open permission for lazer.";

                case SaitenStatusMode.OSXDoesNotSupported:
                    return "Osx device haven't support saiten system yet :(";

                case SaitenStatusMode.WindowsMicrophonePermissionDeclined:
                    return "Open lazer with admin permission to enable saiten system.";

                case SaitenStatusMode.NotSaitening:
                    return "This beatmap is not saitenable.";

                case SaitenStatusMode.AutoPlay:
                    return "Auto play mode.";

                case SaitenStatusMode.Edit:
                    return "Edit mode.";

                case SaitenStatusMode.Saitening:
                    return "Saitening...";

                case SaitenStatusMode.NotInitialized:
                    return "Seems microphone device is not ready.";

                default:
                    return "Weird... Should not goes to here either :oops:";
            }
        }

        private Drawable createIcon(bool saitenable) => new SpriteIcon
        {
            Size = new Vector2(size),
            Icon = saitenable ? FontAwesome.Regular.DotCircle : FontAwesome.Regular.PauseCircle,
            Colour = saitenable ? Color4.Red : Color4.LightGray
        };

        private Drawable createStatusSpriteText(string markdownText) => new StatusSpriteText(markdownText)
        {
            RelativeSizeAxes = Axes.None,
            AutoSizeAxes = Axes.Both
        };

        internal class StatusSpriteText : MarkdownTextFlowContainer
        {
            public StatusSpriteText(string text)
            {
                var block = Markdown.Parse(text).OfType<ParagraphBlock>().FirstOrDefault();

                if (block != null)
                    AddInlineText(block.Inline);
            }
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
