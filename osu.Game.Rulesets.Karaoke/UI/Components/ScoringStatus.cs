// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.ComponentModel;
using System.Linq;
using Markdig;
using Markdig.Syntax;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Containers.Markdown;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Graphics.Containers.Markdown;
using osu.Game.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class ScoringStatus : FillFlowContainer, IMarkdownTextComponent
    {
        private const float size = 22;

        private readonly SpriteIcon icon;
        private readonly MarkdownTextFlowContainer messageText;

        public SpriteText CreateSpriteText() => new OsuSpriteText();

        public ScoringStatus(ScoringStatusMode statusMode)
        {
            Spacing = new Vector2(5);
            Direction = FillDirection.Horizontal;
            AutoSizeAxes = Axes.Both;
            ScoringStatusMode = statusMode;

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

        private ScoringStatusMode statusMode;

        public ScoringStatusMode ScoringStatusMode
        {
            get => statusMode;
            set
            {
                statusMode = value;

                Schedule(() =>
                {
                    bool scorable = statusMode == ScoringStatusMode.Scoring;
                    icon.Icon = scorable ? FontAwesome.Regular.DotCircle : FontAwesome.Regular.PauseCircle;
                    icon.Colour = scorable ? Color4.Red : Color4.LightGray;

                    string text = GetScoringStatusText(statusMode).ToString();
                    var block = Markdown.Parse(text).OfType<ParagraphBlock>().FirstOrDefault();

                    messageText.Clear();
                    if (block != null)
                        messageText.AddInlineText(block.Inline);
                });
            }
        }

        protected virtual LocalisableString GetScoringStatusText(ScoringStatusMode statusMode)
        {
            return statusMode switch
            {
                ScoringStatusMode.AndroidMicrophonePermissionDeclined => "Go to setting to open permission for lazer.",
                ScoringStatusMode.AndroidDoesNotSupported => "Android device haven't support scoring system yet :(",
                ScoringStatusMode.IOSMicrophonePermissionDeclined => "Go to setting to open permission for lazer.",
                ScoringStatusMode.IOSDoesNotSupported => "iOS device haven't support scoring system yet :(",
                ScoringStatusMode.OSXMicrophonePermissionDeclined => "Go to setting to open permission for lazer.",
                ScoringStatusMode.OSXDoesNotSupported => "Osx device haven't support scoring system yet :(",
                ScoringStatusMode.WindowsMicrophonePermissionDeclined => "Open lazer with admin permission to enable scoring system.",
                ScoringStatusMode.NotScoring => "This beatmap is not scorable.",
                ScoringStatusMode.AutoPlay => "Auto play mode.",
                ScoringStatusMode.Edit => "Edit mode.",
                ScoringStatusMode.Scoring => "Scoring...",
                ScoringStatusMode.NotInitialized => "Seems microphone device is not ready.",
                _ => "Weird... Should not goes to here either :oops:"
            };
        }
    }

    public enum ScoringStatusMode
    {
        /// <summary>
        /// Due to android device does not authorize microphone access.
        /// </summary>
        [Description("Android permission declined.")]
        AndroidMicrophonePermissionDeclined,

        /// <summary>
        /// Scoring system does not support android device.
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
        /// Scoring system does not support iOS device.
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
        /// Scoring system does not support osx device.
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
        [Description("No scoring.")]
        NotScoring,

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
        [Description("Scoring...")]
        Scoring,

        /// <summary>
        /// Microphone scoring is not initialized.
        /// </summary>
        [Description("Not initialized.")]
        NotInitialized,
    }
}
