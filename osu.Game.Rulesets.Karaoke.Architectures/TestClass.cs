// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.Architectures;

public class TestClass : BaseTest
{
    [Test]
    [Project.Karaoke(true)]
    [Project.KaraokeTest]
    [Project.OsuFramework]
    [Project.OsuGame]
    public void CheckAbstractClassLocation()
    {
        var architecture = GetProjectArchitecture();
        var abstractClasses = GetExecuteProject() switch
        {
            Project.KaraokeAttribute => architecture.Classes.Where(x => x.IsAbstract == true).Except(new[]
            {
                architecture.GetClassOfType(typeof(ScrollingNotePlayfield)),
                architecture.GetClassOfType(typeof(BindableScrollContainer)),
                architecture.GetClassOfType(typeof(OrderRearrangeableListContainer<>)),
                architecture.GetClassOfType(typeof(EditableTimelineSelectionBlueprint<>)),
            }),
            _ => architecture.Classes.Where(x => x.IsAbstract == true),
        };

        Assertion(() =>
        {
            foreach (var abstractClass in abstractClasses)
            {
                var allChildClasses = architecture.Classes.Where(x => x.InheritedClasses.Contains(abstractClass)).ToArray();
                if (allChildClasses.Length == 0)
                    continue;

                Assert.True(isAllowAbstractClassPosition(abstractClass, allChildClasses), $"Those child class: \n{string.Join('\n', allChildClasses.Select(x => x.ToString()))}\n\n is not in the child namespace of: \n{abstractClass}");
            }
        });
        return;

        static bool isAllowAbstractClassPosition(IType abstractClass, Class[] allChildClasses)
        {
            // follow the ModelBackedDrawable in the osu.framework, we allow the abstract class in here.
            if (abstractClass.Namespace.NameContains("Graphics"))
                return true;

            // should only check the case if all child class's namespace is not related to the parent class.
            int childClassInValidNamespace = allChildClasses.Select(x => x.Namespace.FullName).Count(x => x.Contains(abstractClass.Namespace.FullName));
            int childClassInInvalidNamespace = allChildClasses.Select(x => x.Namespace.FullName).Count(x => !x.Contains(abstractClass.Namespace.FullName));

            if (childClassInInvalidNamespace <= childClassInValidNamespace)
                return true;

            return false;
        }
    }
}
