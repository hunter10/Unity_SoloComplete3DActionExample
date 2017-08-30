using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NormalTarget은 일반 공격을 할 때 공격 반경에 있는 적들의 리스트를 관리하는 클래스입니다.
public class NormalTarget : MonoBehaviour {

    // 공격 대상에 있는 적들의 리스트입니다.
    public List<Collider> targetList;

    private void Awake()
    {
        targetList = new List<Collider>();    
    }

    // 적 개체가 공격 반경 안에 들어오면 targetList에 해당 개체를 추가합니다.
    private void OnTriggerEnter(Collider other)
    {
        targetList.Add(other);
    }

    // 적 개체가 공격 반경을 벗어나면 targetList에서 해당 개체를 제거합니다.
    private void OnTriggerExit(Collider other)
    {
        targetList.Remove(other);
    }
}
