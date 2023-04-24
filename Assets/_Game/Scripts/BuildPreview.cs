using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildPreview : MonoBehaviour
{
    private GameObject preview;
    private BuildOption option;

    public Material mValid;
    public Material mInvalid;

    private MeshRenderer mr;
    private MeshFilter mf;

    PreviewState state = PreviewState.Valid;
    PreviewState lastState = PreviewState.Valid;

    private Dictionary<PreviewState, Material> stateMaterials;

    public PreviewState GetState() => state;

    private void Awake()
    {
        stateMaterials = new Dictionary<PreviewState, Material>() { { PreviewState.Valid, mValid }, { PreviewState.Invalid, mInvalid } };
        mr = GetComponentInChildren<MeshRenderer>();
        mf = GetComponentInChildren<MeshFilter>();
    }

    public void SetPreviewObject(BuildOption option)
    {
        preview = option.structurePrefab;
        mf.transform.localPosition = new Vector3(option.Size.x / 2, 0, option.Size.y / 2);
        mf.mesh = preview.GetComponentInChildren<MeshFilter>().sharedMesh;
    }

    internal void Clear()
    {
        preview = null;
        mf.mesh = null;
    }

    internal void UpdateState(Tile currentTile, PreviewState state)
    {
        if (currentTile == null) transform.position = Vector3.positiveInfinity;
        transform.position = currentTile.transform.position;

        if (state != lastState)
        {
            Material[] mats = Enumerable.Repeat(stateMaterials[state], 3).ToArray();
            mr.materials = mats;
        }

        lastState = state;
    }
}