using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static KooksUserManager;

public class MusicUnlockController : MonoBehaviour
{
    KooksUserManager kooksUserManager;
    KooksUserManager.UserData userData;
    private int progressNum = 0;
    public int WorldNum;    // 지금 몇번째 월드인지 !
    void Start()
    {
        Debug.Log(WorldNum);
        //!!!!!!!!!!!!!@@@@@@@@@@@@@@@@@@@@@@@로컬스토리지에서 해당 정보 가져오기 미리 만든 부분 !!!!!!!!!!!!!@@@@@@@@@@@@@@@@@@@@@@@
        //progressNum = PlayerPrefs.GetInt("progress");
        //!!!!!!!!!!!!!@@@@@@@@@@@@@@@@@@@@@@@로컬스토리지에서 해당 정보 가져오기 미리 만든 부분 !!!!!!!!!!!!!@@@@@@@@@@@@@@@@@@@@@@@

        kooksUserManager = FindObjectOfType<KooksUserManager>();
        if (kooksUserManager != null)
        {
            userData = kooksUserManager.GetUserData(); // KooksUserManager 스크립트에서 UserData를 가져옵니다.
            progressNum = userData.progress;
            GameObject[] Lockers = { GameObject.Find("LockedMusic1"), GameObject.Find("LockedMusic2"), GameObject.Find("LockedMusic3"), GameObject.Find("LockedMusic4") };
            GameObject[] Musics = { GameObject.Find("MusicInform1"), GameObject.Find("MusicInform2"), GameObject.Find("MusicInform3"), GameObject.Find("MusicInform4") };
            if ((progressNum / 100) > WorldNum) 
            {
                //무조건 다 열려야겠지?
                for (int i = 0; i < Lockers.Length; i++)
                {
                    Lockers[i].SetActive(false);
                }
            }
            else if ((progressNum / 100) == WorldNum)
            {
                // 1의자리수 ( 몇번쨰 까지 클리어했는지 ) 확인하고 열어줘야겠지?
                if((progressNum % 10) == 0){
                    for (int i = 0; i < 1; i++)
                    {
                        Lockers[i].SetActive(false);
                    }
                    for (int i = 1; i < 4; i++)
                    {
                        Musics[i].SetActive(false);
                    }
                }
                else if ((progressNum % 10) == 1)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Lockers[i].SetActive(false);
                    }
                    for (int i = 2; i < 4; i++)
                    {
                        Musics[i].SetActive(false);
                    }
                }
                else if ((progressNum % 10) == 2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Lockers[i].SetActive(false);
                    }
                    for (int i = 3; i < 4; i++)
                    {
                        Musics[i].SetActive(false);
                    }
                }
                else if ((progressNum % 10) == 3)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Lockers[i].SetActive(false);
                    }
                }
                else if ((progressNum % 10) == 4) // 얘가 정령들 해금시키는 아마그곳?
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Lockers[i].SetActive(false);
                    }
                }
            }
        }
    }
}
