// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos;

public abstract class StageElement : IHasPrimaryKey
{
    private readonly Bindable<int> orderVersion = new();

    public IBindable<int> OrderVersion => orderVersion;

    /// <summary>
    /// Index of the element.
    /// </summary>
    [JsonProperty]
    public ElementId ID { get; private set; } = ElementId.NewElementId();

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
