using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private UpgradePanelManager upgradePanelManager;
    [SerializeField] public List<GameObject> plugInList;
    public int index;

    public void Set(UpgradeData upgradeData)
    {
        icon.sprite = upgradeData.icon;
        index = upgradeData.imageIndex;
    }

    public void Clean()
    {
        icon.sprite = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnMouseEnter");
        if (index == upgradePanelManager.lastIndex) return;

        //켜져있던 이미지 Off, 마우스 올려놓은 이미지 On
        plugInList[upgradePanelManager.lastIndex].SetActive(false);
        upgradePanelManager.lastIndex = index;
        plugInList[index].SetActive(true);
    }
}