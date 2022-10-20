using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  1. �˾��� ������ �Ѵ�.
        - ȭ�鿡 Ŭ�� ������ UI�� ������Ʈ �Ѵ�.
    2. �˾��� Ŭ���� �ȴ�.
        - ȭ�鿡 Ŭ�� ������ UI�� ������Ʈ �Ѵ�.

    3. �˾��� �������� ������ ���� �������� ���� �˾��� ���� ���� �������� �Ѵ�.
        3-1. 3��° �˾� ��� ��������  30
        3-2. 3��° �˾��� ��� ������. 20
        3-3. �ٽ� 3��° �˾��� ���ȴ�. 30

    4. �˾��� �߸� ��׶���� ���� ���� ���ڴ�. (optional) 
        - ����þ� ��, ...���͵�

    5. �˾��� �߸� �˾� �ڿ� �ִ� UI���� ������ �ȵȴ�.
        - �����̹����� �˾��� ��׶��� �̹����� ��û ũ�� �ִ´�. 
            (-> �˾� �ڿ��� �ȴ�����)
        - 
 */


public class PopUpManager : MonoSingleton<PopUpManager>
{
    [SerializeField] Transform popupCanvas;
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
    }

    public void Close(Popup popup)
    {
        popupStack.Pop();
    }

    public void Close()
    {
        popupStack.Peek().Close();
        Debug.Log("POPUPCLOSE form POPUPMG");
    }
}
