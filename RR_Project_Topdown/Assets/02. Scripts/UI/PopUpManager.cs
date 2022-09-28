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
    
    
    public void Open(string popupld)
    {
        string prefabName = "Popup " + popupld;
        Debug.Log(prefabName);
        GameObject prefab = Resources.Load<GameObject>(prefabName);

        var popup = GameObject.Instantiate(prefab) as GameObject;
        popup.GetComponent<Popup>().Open();
        
        popup.transform.SetParent(popupCanvas);
        popup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        popupStack.Push(prefab.GetComponent<Popup>());
    }   

    public void Close()
    {
        var popup = popupStack.Pop();
        Debug.Log("POPUPCLOSE form POPUPMG");
        popup.Close();
    }
}
