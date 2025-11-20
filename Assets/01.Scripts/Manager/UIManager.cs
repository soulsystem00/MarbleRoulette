using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UISortintOrder
{
    Background = 0,
    Default = 100,
    Popup = 200,
    TopMost = 300
}

public class UIManager : MonoSingleton<UIManager>
{
    private const string UI_PATH = "Prefabs/UI";

    [SerializeField] Canvas uiCanvas;

    private List<UIBase> uiBaseList = new List<UIBase>();

    protected override void Init()
    {
        if (uiBaseList == null)
        {
            uiBaseList = new List<UIBase>();
        }

        uiBaseList.Clear();

        uiBaseList = new List<UIBase>(uiCanvas.GetComponentsInChildren<UIBase>(true));

        foreach (var uiBase in uiBaseList)
        {
            uiBase.Init();
            uiBase.Close();
        }
    }

    public T GetUI<T>() where T : UIBase
    {
        foreach (var uiBase in uiBaseList)
        {
            if (uiBase is T)
            {
                return uiBase as T;
            }
        }
        return null;
    }

    public T OpenUI<T>() where T : UIBase
    {
        T ui = GetUI<T>();
        if (ui != null)
        {
            ui.Open();
        }
        else
        {
            ui = Instantiate(Resources.Load<T>($"{UI_PATH}/{typeof(T).Name}"), uiCanvas.transform);

            ui.Init();
            ui.Open();

            uiBaseList.Add(ui);
        }

        return ui;
    }

    public void CloseUI<T>() where T : UIBase
    {
        T ui = GetUI<T>();
        if (ui != null)
        {
            ui.Close();
        }
    }

    public bool IsOpenUI<T>() where T : UIBase
    {
        T ui = GetUI<T>();
        if (ui != null)
        {
            return ui.IsOpen;
        }
        return false;
    }

    [ContextMenu("TestOpenUI")]
    private void TestOpenUI()
    {
        OpenUI<UISetting>();
    }

    [ContextMenu("TestCloseUI")]
    private void TestCloseUI()
    {
        CloseUI<UISetting>();
    }
}
