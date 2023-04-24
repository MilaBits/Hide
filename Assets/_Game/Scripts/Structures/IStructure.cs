using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tier
{
    Crude = 0,
    Basic = 1,
    Stable = 2,
    Reinforced = 3
}


public interface IStructure
{
    public int GetHealth();
    public Tier GetTier();
    public void Init(GameObject player = null);
}

public static class TierExtensions
{
    public static Color ToColor(this Tier tier) => tier switch
    {
        Tier.Crude      => new Color(.26f, .7f, .26f),
        Tier.Basic      => new Color(.42f, .61f, .96f),
        Tier.Stable     => new Color(1f, .89f, .6f),
        Tier.Reinforced => new Color(.9f, .56f, .22f),
        _ => throw new System.NotImplementedException(),
    };
}
