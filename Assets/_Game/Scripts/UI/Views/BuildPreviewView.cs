
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PreviewState
{
    Valid,
    Invalid,
    Upgrade
}
public struct PreviewInfo
{
    public PreviewInfo(BuildOption option, Vector3 position, PreviewState state)
    {
        this.option = option;
        this.position = position;
        this.state = state;
    }

    public BuildOption option;
    public Vector3 position;
    public PreviewState state;
}

public class BuildPreviewView : View
{
    private Dictionary<PreviewState, Material> stateMaterials;

    [SerializeField] private MeshRenderer mr;
    [SerializeField] private MeshFilter mf;

    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;

    private GameObject previewObject;

    private PreviewState state;

    public override void Initialize()
    {
        base.Initialize();
        stateMaterials = new Dictionary<PreviewState, Material>() { { PreviewState.Valid, validMaterial }, { PreviewState.Invalid, invalidMaterial } };
    }

    public override void Show(object args = null)
    {
        base.Show(args);

        if (args is PreviewInfo info)
        {
            if (state != info.state)
            {
                state = info.state;
                UpdateMaterial();
            }

            SetPreviewObject(info.option);
            transform.position = info.position;
        }
    }

    public override void Hide()
    {
        base.Hide();
        previewObject = null;
        mf.mesh = null;
    }

    public void UpdateMaterial()
    {
        Material[] mats = Enumerable.Repeat(stateMaterials[state], 3).ToArray();
        mr.materials = mats;
    }

    public void SetPreviewObject(BuildOption option)
    {
        if (previewObject == option.structurePrefab) return;

        previewObject = option.structurePrefab;
        mf.transform.localPosition = new Vector3(option.Size.x / 2, 0, option.Size.y / 2);
        mf.mesh = previewObject.GetComponentInChildren<MeshFilter>().sharedMesh;
    }
}
