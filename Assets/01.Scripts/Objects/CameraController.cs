using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private WinType winType;
    [SerializeField] private float followSpeed = 2f;
    [SerializeField] private bool isFollowing = false;

    public WinType WinType { get => winType; set => winType = value; }
    public bool IsFollowing { get => isFollowing; set => isFollowing = value; }
    public float FollowSpeed { get => followSpeed; set => followSpeed = value; }


    private void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        IsFollowing = false;
    }

    private void Update()
    {
        if (isFollowing == false)
        {
            transform.position = new Vector3(0, 0, transform.position.z);
            return;
        }

        var target = GameManager.Instance.GetTargetPlayer(WinType);

        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.transform.position.x, target.transform.position.y, cam.transform.position.z);
            cam.transform.position = Vector3.Lerp(cam.transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }
}
