// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Stages;

public class ClassicStageInfoTest
{
    #region Style

    [Test]
    public void TestAddStyle()
    {
        var stageInfo = new ClassicStageInfo();
        stageInfo.AddStyle(x =>
        {
            x.Name = "Style1";
        });

        Assert.AreEqual(1, stageInfo.AvailableStyles.Count);
        Assert.AreEqual(1, stageInfo.AvailableStyles[0].ID);
        Assert.AreEqual("Style1", stageInfo.AvailableStyles[0].Name);

        stageInfo.AddStyle();

        Assert.AreEqual(2, stageInfo.AvailableStyles.Count);
        Assert.AreEqual(2, stageInfo.AvailableStyles[1].ID);
    }

    [Test]
    public void TestEditStyle()
    {
        var stageInfo = new ClassicStageInfo();
        stageInfo.AddStyle();

        int id = stageInfo.AvailableStyles[0].ID;
        stageInfo.EditStyle(id, x =>
        {
            x.Name = "Style1";
        });

        Assert.AreEqual(1, stageInfo.AvailableStyles.Count);
        Assert.AreEqual(1, stageInfo.AvailableStyles[0].ID);
        Assert.AreEqual("Style1", stageInfo.AvailableStyles[0].Name);
    }

    [Test]
    public void TestGetMappingLyricIdsByStyle()
    {
        var stageInfo = new ClassicStageInfo();
        stageInfo.AddStyle();
        stageInfo.AddStyle();

        var style1 = stageInfo.AvailableStyles[0];
        var style2 = stageInfo.AvailableStyles[1];
        var lyric1 = new Lyric { ID = 1 };
        var lyric2 = new Lyric { ID = 2 };
        var lyric3 = new Lyric { ID = 3 };

        stageInfo.ApplyStyleMapping(style1, lyric1);
        stageInfo.ApplyStyleMapping(style1, lyric2);
        stageInfo.ApplyStyleMapping(style2, lyric3);

        int[] mappingLyricIdsByStyle1 = stageInfo.GetMappingLyricIds(style1).ToArray();
        int[] mappingLyricIdsByStyle2 = stageInfo.GetMappingLyricIds(style2).ToArray();
        Assert.AreEqual(new[] { 1, 2 }, mappingLyricIdsByStyle1);
        Assert.AreEqual(new[] { 3 }, mappingLyricIdsByStyle2);
    }

    [Test]
    public void TestRemoveStyle()
    {
        var stageInfo = new ClassicStageInfo();
        stageInfo.AddStyle();
        stageInfo.AddStyle();

        var style1 = stageInfo.AvailableStyles[0];
        var style2 = stageInfo.AvailableStyles[1];
        var lyric1 = new Lyric { ID = 1 };
        var lyric2 = new Lyric { ID = 2 };
        var lyric3 = new Lyric { ID = 3 };

        stageInfo.ApplyStyleMapping(style1, lyric1);
        stageInfo.ApplyStyleMapping(style1, lyric2);
        stageInfo.ApplyStyleMapping(style2, lyric3);

        // Only remove style 1.
        stageInfo.RemoveStyle(style1);

        // should not have any mappings.
        var mappingLyricIdsByStyle1 = stageInfo.GetMappingLyricIds(style1);
        Assert.IsEmpty(mappingLyricIdsByStyle1);

        // should still remain mappings.
        var mappingLyricIdsByStyle2 = stageInfo.GetMappingLyricIds(style2);
        Assert.AreEqual(1, mappingLyricIdsByStyle2.Count());
    }

    [Test]
    public void TestApplyStyleMapping()
    {
        var stageInfo = new ClassicStageInfo();
        stageInfo.AddStyle();
        stageInfo.AddStyle();

        var style1 = stageInfo.AvailableStyles[0];
        var style2 = stageInfo.AvailableStyles[1];
        var lyric1 = new Lyric { ID = 1 };
        var lyric2 = new Lyric { ID = 2 };
        var lyric3 = new Lyric { ID = 3 };

        stageInfo.ApplyStyleMapping(style1, lyric1);
        stageInfo.ApplyStyleMapping(style1, lyric2);
        stageInfo.ApplyStyleMapping(style2, lyric3);

        int[] mappingLyricIdsByStyle1 = stageInfo.GetMappingLyricIds(style1).ToArray();
        int[] mappingLyricIdsByStyle2 = stageInfo.GetMappingLyricIds(style2).ToArray();
        Assert.AreEqual(new[] { 1, 2 }, mappingLyricIdsByStyle1);
        Assert.AreEqual(new[] { 3 }, mappingLyricIdsByStyle2);
    }

    [Test]
    public void TestClearStyleMapping()
    {
        var stageInfo = new ClassicStageInfo();
        stageInfo.AddStyle();

        var style1 = stageInfo.AvailableStyles[0];
        var lyric1 = new Lyric { ID = 1 };

        stageInfo.ApplyStyleMapping(style1, lyric1);
        stageInfo.ClearStyleMapping(lyric1);

        var mappings = stageInfo.StyleMappings;
        Assert.IsEmpty(mappings);
    }

    #endregion

    #region Layout

    [Test]
    public void TestAddLyricLayout()
    {
        var stageInfo = new ClassicStageInfo();
        stageInfo.AddLyricLayout(x =>
        {
            x.Name = "Layout1";
        });

        Assert.AreEqual(1, stageInfo.AvailableLyricLayouts.Count);
        Assert.AreEqual(1, stageInfo.AvailableLyricLayouts[0].ID);
        Assert.AreEqual("Layout1", stageInfo.AvailableLyricLayouts[0].Name);

        stageInfo.AddLyricLayout();

        Assert.AreEqual(2, stageInfo.AvailableLyricLayouts.Count);
        Assert.AreEqual(2, stageInfo.AvailableLyricLayouts[1].ID);
    }

    [Test]
    public void TestEditLyricLayout()
    {
        var stageInfo = new ClassicStageInfo();
        stageInfo.AddLyricLayout();

        int id = stageInfo.AvailableLyricLayouts[0].ID;
        stageInfo.EditLyricLayout(id, x =>
        {
            x.Name = "Layout1";
        });

        Assert.AreEqual(1, stageInfo.AvailableLyricLayouts.Count);
        Assert.AreEqual(1, stageInfo.AvailableLyricLayouts[0].ID);
        Assert.AreEqual("Layout1", stageInfo.AvailableLyricLayouts[0].Name);
    }

    [Test]
    public void TestGetMappingLyricIdsByLayout()
    {
        var stageInfo = new ClassicStageInfo();
        stageInfo.AddLyricLayout();
        stageInfo.AddLyricLayout();

        var layout1 = stageInfo.AvailableLyricLayouts[0];
        var layout2 = stageInfo.AvailableLyricLayouts[1];
        var lyric1 = new Lyric { ID = 1 };
        var lyric2 = new Lyric { ID = 2 };
        var lyric3 = new Lyric { ID = 3 };

        stageInfo.ApplyLyricLayoutMapping(layout1, lyric1);
        stageInfo.ApplyLyricLayoutMapping(layout1, lyric2);
        stageInfo.ApplyLyricLayoutMapping(layout2, lyric3);

        int[] mappingLyricIdsByLayout1 = stageInfo.GetMappingLyricIds(layout1).ToArray();
        int[] mappingLyricIdsByLayout2 = stageInfo.GetMappingLyricIds(layout2).ToArray();
        Assert.AreEqual(new[] { 1, 2 }, mappingLyricIdsByLayout1);
        Assert.AreEqual(new[] { 3 }, mappingLyricIdsByLayout2);
    }

    [Test]
    public void TestRemoveLyricLayout()
    {
        var stageInfo = new ClassicStageInfo();
        stageInfo.AddLyricLayout();
        stageInfo.AddLyricLayout();

        var layout1 = stageInfo.AvailableLyricLayouts[0];
        var layout2 = stageInfo.AvailableLyricLayouts[1];
        var lyric1 = new Lyric { ID = 1 };
        var lyric2 = new Lyric { ID = 2 };
        var lyric3 = new Lyric { ID = 3 };

        stageInfo.ApplyLyricLayoutMapping(layout1, lyric1);
        stageInfo.ApplyLyricLayoutMapping(layout1, lyric2);
        stageInfo.ApplyLyricLayoutMapping(layout2, lyric3);

        // Only remove layout 1.
        stageInfo.RemoveLyricLayout(layout1);

        // should not have any mappings.
        var mappingLyricIdsByLayout1 = stageInfo.GetMappingLyricIds(layout1);
        Assert.IsEmpty(mappingLyricIdsByLayout1);

        // should still remain mappings.
        var mappingLyricIdsByLayout2 = stageInfo.GetMappingLyricIds(layout2);
        Assert.AreEqual(1, mappingLyricIdsByLayout2.Count());
    }

    [Test]
    public void TestApplyLyricLayoutMapping()
    {
        var stageInfo = new ClassicStageInfo();
        stageInfo.AddLyricLayout();
        stageInfo.AddLyricLayout();

        var layout1 = stageInfo.AvailableLyricLayouts[0];
        var layout2 = stageInfo.AvailableLyricLayouts[1];
        var lyric1 = new Lyric { ID = 1 };
        var lyric2 = new Lyric { ID = 2 };
        var lyric3 = new Lyric { ID = 3 };

        stageInfo.ApplyLyricLayoutMapping(layout1, lyric1);
        stageInfo.ApplyLyricLayoutMapping(layout1, lyric2);
        stageInfo.ApplyLyricLayoutMapping(layout2, lyric3);

        int[] mappingLyricIdsByLayout1 = stageInfo.GetMappingLyricIds(layout1).ToArray();
        int[] mappingLyricIdsByLayout2 = stageInfo.GetMappingLyricIds(layout2).ToArray();
        Assert.AreEqual(new[] { 1, 2 }, mappingLyricIdsByLayout1);
        Assert.AreEqual(new[] { 3 }, mappingLyricIdsByLayout2);
    }

    [Test]
    public void TestClearLyricLayoutMapping()
    {
        var stageInfo = new ClassicStageInfo();
        stageInfo.AddLyricLayout();

        var layout1 = stageInfo.AvailableLyricLayouts[0];
        var lyric1 = new Lyric { ID = 1 };

        stageInfo.ApplyLyricLayoutMapping(layout1, lyric1);
        stageInfo.ClearLyricLayoutMapping(lyric1);

        var mappings = stageInfo.LyricLayoutMappings;
        Assert.IsEmpty(mappings);
    }

    #endregion

    #region Stage element

    #endregion
}
