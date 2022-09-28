using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private PlayerMove player;
    [SerializeField] private UI_PlugInTree uI_PlugInTree;

    private void Start()
    {
        uI_PlugInTree.SetPlayerPlugIn(player.GetPlayerPlugIn());
    }
}
