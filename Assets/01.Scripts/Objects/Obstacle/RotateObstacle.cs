using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacle : ObstacleBase
{
    [SerializeField] private float rotateSpeed = 100f;
    public override void Init()
    {

    }

    public override void Move()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
