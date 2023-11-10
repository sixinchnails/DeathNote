using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowResult : MonoBehaviour
{
    [SerializeField] Text text;
    // Start is called before the first frame update
    public void duplicateName()
    {
        text.text = "중복된 닉네임입니다";
    }

    public void successName()
    {
        text.text = "닉네임이 설정되었습니다";
    }
}
