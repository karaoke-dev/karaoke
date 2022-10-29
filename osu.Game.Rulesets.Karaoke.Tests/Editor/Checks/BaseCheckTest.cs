// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Configs;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    public class BaseCheckTest<TCheck> where TCheck : class, ICheck, new()
    {
        private TCheck check = null!;

        [SetUp]
        public void Setup()
        {
            check = new TCheck();

            // check template in the list should not be duplicated.
            var possibleTemplates = check.PossibleTemplates;
            Assert.AreEqual(possibleTemplates.Count(), possibleTemplates.Select(x => x.GetType()).Distinct().Count());
        }

        protected void SetConfig<TConfig>(TConfig config) where TConfig : IHasConfig<TConfig>, new()
        {
            if (check is not IHasCheckConfig<TConfig> checkWithConfig)
                throw new InvalidCastException();

            checkWithConfig.Config = config;
        }

        protected void AssertOk(BeatmapVerifierContext context)
        {
            Assert.That(Run(context), Is.Empty);
        }

        protected void AssertNotOk<TIssueTemplate>(BeatmapVerifierContext context)
        {
            var issues = Run(context).ToList();

            Assert.That(issues, Has.Count.EqualTo(1));
            Assert.AreEqual(typeof(TIssueTemplate), issues.Single().Template.GetType());
        }

        protected IEnumerable<Issue> Run(BeatmapVerifierContext context)
        {
            return check.Run(context);
        }
    }
}
