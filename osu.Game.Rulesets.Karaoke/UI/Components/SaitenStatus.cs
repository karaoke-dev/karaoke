// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class SaitenStatus : Framework.Graphics.Containers.Container
    {
        public SaitenStatus(SaitenStatusMode statusMode)
        {
        }
    }

    public enum SaitenStatusMode
    {
        /// <summary>
        /// Due to android device does not authorize microphone access.
        /// </summary>
        [Description("Android permission declined")]
        AndroidMicrophonePermissionDeclined,

        /// <summary>
        /// Saiten system does not support android device.
        /// Will throw this if osu.framework.microphone does not supportu it yet.
        /// Or official client does not open this permission.
        /// </summary>
        [Description("Android target not supported")]
        AndroidDoesNotSupported,

        /// <summary>
        /// Due to iOS device does not authorize microphone access.
        /// </summary>
        [Description("iOS permission declined")]
        IOSMicrophonePermissionDeclined,

        /// <summary>
        /// Saiten system does not support iOS device.
        /// Will throw this if osu.framework.microphone does not supportu it yet.
        /// Or official client does not open this permission.
        /// </summary>
        [Description("iOS target not supported")]
        IOSDoesNotSupported,

        /// <summary>
        /// Due to osx device does not authorize microphone access.
        /// </summary>
        [Description("osx permission declined")]
        OSXMicrophonePermissionDeclined,

        /// <summary>
        /// Saiten system does not support osx device.
        /// Will throw this if osu.framework.microphone does not supportu it yet.
        /// Or official client does not open this permission.
        /// </summary>
        [Description("osx target not supported")]
        OSXDoesNotSupported,

        /// <summary>
        /// Due to windows device does not authorize microphone access.
        /// Windows client don't need to ask permission.
        /// Open lazer client with admin permission can solve that.
        /// </summary>
        [Description("Windows permission declined")]
        WindowsMicrophonePermissionDeclined,

        /// <summary>
        /// Everything works well.
        /// </summary>
        [Description("Saitening...")]
        Saitening,
    }
}
