using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour, IStructure
{
    public int Health = 500;
    public Tier tier = Tier.Crude;
    public int GetHealth() => Health;

    public Tier GetTier() => tier;

    public void Init(GameObject player = null)
    {
        
    }

    private void Start()
    {
        Material[] mats = GetComponentInChildren<Renderer>().materials;
        mats[1].color = tier.ToColor();

        GetComponentInChildren<Renderer>().materials = mats;
    }
}
