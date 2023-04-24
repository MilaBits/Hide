using FishNet.Object;

internal class Player : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner) return;

        ViewManager.Instance.Initialize();
    }

    private void Update()
    {
        if (!IsOwner) return;

    }
}
