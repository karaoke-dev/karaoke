// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

/// <summary>
/// It's a switchable screen for able to switch workspace inside the <see cref="WorkspaceScreenStack{TItem}"/>
/// TODO: eventually make this inherit Screen and add a local screen stack inside the Editor.
/// </summary>
public abstract partial class WorkspaceScreen<TItem> : VisibilityContainer
{
    public readonly TItem Item;

    protected WorkspaceScreen(TItem item)
    {
        Item = item;
    }
}
