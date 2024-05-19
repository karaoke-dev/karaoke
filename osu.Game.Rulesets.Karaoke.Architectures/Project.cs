// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework;
using osu.Game.Rulesets.Karaoke.Tests;

namespace osu.Game.Rulesets.Karaoke.Architectures;

public class Project
{
    public sealed class KaraokeAttribute : ProjectAttribute
    {
        public override ExecuteType ExecuteType => ExecuteType.Check;

        public override Type RootObjectType => typeof(KaraokeRuleset);

        public KaraokeAttribute(bool execute = false)
            : base(execute)
        {
        }
    }

    public sealed class KaraokeTestAttribute : ProjectAttribute
    {
        public override ExecuteType ExecuteType => ExecuteType.Check;

        public override Type RootObjectType => typeof(VisualTestRunner);

        public KaraokeTestAttribute(bool execute = false)
            : base(execute)
        {
        }
    }

    public sealed class OsuGameAttribute : ProjectAttribute
    {
        public override ExecuteType ExecuteType => ExecuteType.Report;

        public override Type RootObjectType => typeof(OsuGame);

        public OsuGameAttribute(bool execute = false)
            : base(execute)
        {
        }
    }

    public sealed class OsuFrameworkAttribute : ProjectAttribute
    {
        public override ExecuteType ExecuteType => ExecuteType.Report;

        public override Type RootObjectType => typeof(Host);

        public OsuFrameworkAttribute(bool execute = false)
            : base(execute)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public abstract class ProjectAttribute : Attribute
    {
        public bool Execute { get; }

        public abstract ExecuteType ExecuteType { get; }

        public abstract Type RootObjectType { get; }

        protected ProjectAttribute(bool execute)
        {
            Execute = execute;
        }
    }

    public enum ExecuteType
    {
        /// <summary>
        /// Make sure the project follow the architecture rule.
        /// </summary>
        Check,

        /// <summary>
        /// Get the percentage of follow/not follow the architecture rule.
        /// </summary>
        Report,
    }
}
