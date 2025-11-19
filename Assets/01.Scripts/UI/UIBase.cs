using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour, IUIBase
{
    [SerializeField] protected UISortintOrder sortintOrder = UISortintOrder.Default;
    [SerializeField] protected GameObject view;

    public abstract void Init();
    public virtual void Open()
    {
        view.gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        view.gameObject.SetActive(false);
    }
}
