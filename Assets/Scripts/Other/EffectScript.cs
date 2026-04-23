using UnityEngine;

public class EffectScript : MonoBehaviour
{
    
    //Animator에서 Clip의 마지막 프레임에 호출되는 이벤트 함수
    void Disabled()
    {
        this.gameObject.SetActive(false);
    }
}
