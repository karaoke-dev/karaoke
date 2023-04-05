// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages;

public abstract class StageElement
{
    private readonly Bindable<int> orderVersion = new();

    public IBindable<int> OrderVersion => orderVersion;

    protected StageElement(int id)
    {
        ID = id;
    }

    /// <summary>
    /// Index of the element.
    /// </summary>
    public int ID { get; protected set; }

    [JsonIgnore]
    public readonly Bindable<string> NameBindable = new();

    /// <summary>
    /// Name of the element.
    /// </summary>
    public string Name
    {
        get => NameBindable.Value;
        set => NameBindable.Value = value;
    }

    protected void TriggerOrderVersionChanged()
    {
        orderVersion.Value++;
    }
}
