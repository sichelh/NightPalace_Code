using UnityEngine;

public enum CrosshairType
{
    Item,
    Door,
    LockedDoor,
}

//상호작용 가능한 오브젝트 스크립트에 상속
public interface IInteractable
{
    string PromptUI();
    // 상호작용 가능한 오브젝트는 프롬프트UI를 가져야함 (string.empty 이더라도)

    CrosshairType GetCrosshairType();
    // 상호작용 가능한 오브젝트는 CrosshairType열거형 타입에 따른 서로 다른 상호작용 크로스헤어를 가질 수 있음

    string KeyUI();
    // 상호작용 가능한 오브젝트는 서로 다른 KeyUI 텍스트를 가질 수 있음

    void Interact();
    //상호작용 가능한 오브젝트는 상호작용이 가능해야 함

    //void GetData(ref ItemData itemData);
    ItemData GetData();
    //상호작용 가능한 오브젝트의 데이터를 가져올 수 있음.

    string InteractGetDataText();
    //상호작용을 할 시에 나오는 텍스트 문구
}
