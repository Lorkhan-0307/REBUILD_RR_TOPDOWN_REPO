using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  1. 팝업이 열려야 한다.
        - 화면에 클릭 가능한 UI를 업데이트 한다.
    2. 팝업이 클로즈 된다.
        - 화면에 클릭 가능한 UI를 업데이트 한다.

    3. 팝업이 여러개가 떴을때 제일 마지막에 열린 팝업이 제일 위로 보여져야 한다.
        3-1. 3번째 팝업 방금 열렸을때  30
        3-2. 3번째 팝업이 방금 닫혔다. 20
        3-3. 다시 3번째 팝업이 열렸다. 30

    4. 팝업이 뜨면 백그라운드는 블러가 들어가면 좋겠다. (optional) 
        - 가우시안 블러, ...스터디

    5. 팝업이 뜨면 팝업 뒤에 있는 UI들은 눌리면 안된다.
        - 투명이미지를 팝업에 백그라운드 이미지로 엄청 크게 넣는다. 
            (-> 팝업 뒤에는 안눌린다)
        - 
 */


public class PopUpManager : MonoSingleton<PopUpManager>
{
    [SerializeField] Transform popupCanvas;
    [SerializeField] Transform invisibleImage;
    Stack<Popup> popupStack = new Stack<Popup>();
    
    
    public static Popup Open(string popupld)
    {
        string prefabName = "Popup " + popupld;
        Debug.Log(prefabName);
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        

        var popup = GameObject.Instantiate(prefab) as GameObject;



        popup.name = prefabName;

        var popupComponent = popup.GetComponent<Popup>();
        popupComponent.Open();

        return popupComponent;
    }

    public void Open(Popup popup)
    {
        popup.transform.SetParent(popupCanvas);
        popup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        popupStack.Push(popup.GetComponent<Popup>());

        InvisibleImageReplace();

        
    }

    public void InvisibleImageReplace()
    {
        int stackCount = popupStack.Count;
        if(stackCount == 0)
        {
            invisibleImage.gameObject.SetActive(false);
        }
        else if (stackCount == 1)
        {
            invisibleImage.gameObject.SetActive(true);
            invisibleImage.SetSiblingIndex(0);
        }
        else
        {
            invisibleImage.SetSiblingIndex(stackCount-1);
        }
    }

    public void Close(Popup popup)
    {
        popupStack.Pop();
        InvisibleImageReplace();
    }

    public void Close()
    {
        popupStack.Peek().Close();
        Debug.Log("POPUPCLOSE form POPUPMG");
    }
}
