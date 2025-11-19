using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Update()
    {
        Move();
    }

    public abstract void Init();

    public abstract void Move();
}
