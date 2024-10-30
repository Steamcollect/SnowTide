using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabComponent : MonoBehaviour
{
    public List<TabGroup> tabsButtons;
    private int lastTabIndex = -1;
    
    private void Start()
    {
        tabsButtons ??= new List<TabGroup>();
        for (var index = 0; index < tabsButtons.Count; index++)
        {
            var tabButton = tabsButtons[index];
            if (tabButton == null) continue;
            Subscribe(tabButton);
            tabButton.ImageState(0);
            tabButton.ChangeStateContent(false);
        }

        if (tabsButtons.Count > 0)
        {
            OnTabClick(tabsButtons[0]);
        }
        
    }

    private void Subscribe(TabGroup tabGroup)
    {
        tabGroup.OnActionHandler += ActionHandlerButton;
    }


    private void ActionHandlerButton(PointerHandlerAction typeEvent, TabGroup tabGroup)
    {
        switch (typeEvent)
        {
            case PointerHandlerAction.PointEnter:
                OnTabEnter(tabGroup);
                break;
            case PointerHandlerAction.PointClick:
                OnTabClick(tabGroup);
                break;
            case PointerHandlerAction.PointExit:
                OnTabExit(tabGroup);
                break;
        }
    }
    
    private void OnTabEnter(TabGroup tabGroup)
    {
        ResetTabGroup();
        if (tabGroup == null) return;
        if (lastTabIndex == -1 || tabsButtons[lastTabIndex] != tabGroup)
        {
            tabGroup.ImageState(1);
        }
    }
    
    private void OnTabClick(TabGroup tabGroup)
    {
        if (lastTabIndex != -1) tabsButtons[lastTabIndex].ChangeStateContent(false);
        lastTabIndex = tabsButtons.IndexOf(tabGroup);
        ResetTabGroup();
        if (tabGroup == null) return;
        tabGroup.ImageState(2);
        tabGroup.ChangeStateContent(true);
        
    }

    private void OnTabExit(TabGroup tabGroup)
    {
        ResetTabGroup();
    }

    private void ResetTabGroup()
    {
        for (int i = 0; i < tabsButtons.Count; i++)
        {
            if (tabsButtons[i] == null) continue;
            if (lastTabIndex != -1 && lastTabIndex == i) continue;
            tabsButtons[i].ImageState(0);
        }
    }
    
}
