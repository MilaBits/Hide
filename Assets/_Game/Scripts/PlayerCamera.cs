using UnityEngine;
using System.Linq;
using FishNet.Object;
using Cinemachine;

public class PlayerCamera : NetworkBehaviour
{
    //private Transform target;
    //public Vector3 offset;
    //public float smoothTime = .3f;

    //private Vector3 velocity;

    //public void SetTarget(Transform target)
    //{
    //    //this.target = target;

    //    transform.position = target.position + offset;
    //    transform.LookAt(target.position);
    //}

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            Camera c = Camera.main;
            CinemachineVirtualCamera vc = c.GetComponent<CinemachineVirtualCamera>();
            vc.LookAt = transform;
            vc.Follow = transform;
        } 
    }

    //void FixedUpdate()
    //{
    //    if (target == null) return;

    //    transform.position = Vector3.SmoothDamp(transform.position, TargetPos(), ref velocity, smoothTime);
    //}

    //private Vector3 TargetPos() => target.position + offset; 
}
