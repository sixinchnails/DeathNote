using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldOnOffController : MonoBehaviour
{
    KooksUserManager kooksUserManager;
    KooksUserManager.UserData userData;
    private int progressNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        //!!!!!!!!!!!!!@@@@@@@@@@@@@@@@@@@@@@@로컬스토리지에서 해당 정보 가져오기 미리 만든 부분 !!!!!!!!!!!!!@@@@@@@@@@@@@@@@@@@@@@@
        //progressNum = PlayerPrefs.GetInt("progress");
        //!!!!!!!!!!!!!@@@@@@@@@@@@@@@@@@@@@@@로컬스토리지에서 해당 정보 가져오기 미리 만든 부분 !!!!!!!!!!!!!@@@@@@@@@@@@@@@@@@@@@@@

        kooksUserManager = FindObjectOfType<KooksUserManager>();
        if (kooksUserManager != null)
        {
            userData = kooksUserManager.GetUserData(); // KooksUserManager 스크립트에서 UserData를 가져옵니다.
            progressNum = userData.progress;
            if(progressNum == 0)
            {
                // 라현 flow 물어보기
            }else if((progressNum / 100) == 1) // 1부터 6까지는 해당하는 범위까지 MoveToStage??까지 fly 스크립트 켜주고 LockedStage?? 꺼준다.
            {
                GameObject beUnlockGameObjects = GameObject.Find("LockedStageOne");
                beUnlockGameObjects.SetActive(false);
                GameObject beFlyGameObject = GameObject.Find("MoveToStageOne");
                beFlyGameObject.GetComponent<fly> ().enabled = true;
            }
            else if ((progressNum / 100) == 2)
            {
                //2까지 열려있어야한다.
                GameObject[] beUnlockGameObjects = { GameObject.Find("LockedStageOne"), GameObject.Find("LockedStageTwo") };
                for (int i = 0; i < beUnlockGameObjects.Length; i++)
                {
                    beUnlockGameObjects[i].SetActive(false);
                }

                GameObject[] beFlyGameObjects = { GameObject.Find("MoveToStageOne"), GameObject.Find("MoveToStageTwo") };
                for (int i = 0; i < beFlyGameObjects.Length; i++)
                {
                    beFlyGameObjects[i].GetComponent<fly>().enabled = true;
                }

            }
            else if ((progressNum / 100) == 3)
            {
                //3까지 열려있어야한다.
                GameObject[] otherGameObjects = { GameObject.Find("LockedStageOne"), GameObject.Find("LockedStageTwo"), GameObject.Find("LockedStageThree") };
                for (int i = 0; i < otherGameObjects.Length; i++)
                {
                    otherGameObjects[i].SetActive(false);
                }

                GameObject[] beFlyGameObjects = { GameObject.Find("MoveToStageOne"), GameObject.Find("MoveToStageTwo"), GameObject.Find("MoveToStageThree") };
                for (int i = 0; i < beFlyGameObjects.Length; i++)
                {
                    beFlyGameObjects[i].GetComponent<fly>().enabled = true;
                }
            }
            else if ((progressNum / 100) == 4)
            {
                //4까지 열려있어야한다.
                GameObject[] otherGameObjects = { GameObject.Find("LockedStageOne"), GameObject.Find("LockedStageTwo"), GameObject.Find("LockedStageThree"), GameObject.Find("LockedStageFour") };
                for (int i = 0; i < otherGameObjects.Length; i++)
                {
                    otherGameObjects[i].SetActive(false);
                }

                GameObject[] beFlyGameObjects = { GameObject.Find("MoveToStageOne"), GameObject.Find("MoveToStageTwo"), GameObject.Find("MoveToStageThree"), GameObject.Find("MoveToStageFour") };
                for (int i = 0; i < beFlyGameObjects.Length; i++)
                {
                    beFlyGameObjects[i].GetComponent<fly>().enabled = true;
                }
            }
            else if ((progressNum / 100) == 5)
            {
                //5까지 열려있어야한다.
                GameObject[] otherGameObjects = { GameObject.Find("LockedStageOne"), GameObject.Find("LockedStageTwo"), GameObject.Find("LockedStageThree"), GameObject.Find("LockedStageFour"), GameObject.Find("LockedStageFive") };
                for (int i = 0; i < otherGameObjects.Length; i++)
                {
                    otherGameObjects[i].SetActive(false);
                }

                GameObject[] beFlyGameObjects = { GameObject.Find("MoveToStageOne"), GameObject.Find("MoveToStageTwo"), GameObject.Find("MoveToStageThree"), GameObject.Find("MoveToStageFour"), GameObject.Find("MoveToStageFive") };
                for (int i = 0; i < beFlyGameObjects.Length; i++)
                {
                    beFlyGameObjects[i].GetComponent<fly>().enabled = true;
                }
            }
            else if ((progressNum / 100) == 6)
            {
                //6까지 열려있어야한다.
                GameObject[] otherGameObjects = { GameObject.Find("LockedStageOne"), GameObject.Find("LockedStageTwo"), GameObject.Find("LockedStageThree"), GameObject.Find("LockedStageFour"), GameObject.Find("LockedStageFive"), GameObject.Find("LockedStageSix") };
                for (int i = 0; i < otherGameObjects.Length; i++)
                {
                    otherGameObjects[i].SetActive(false);
                }

                GameObject[] beFlyGameObjects = { GameObject.Find("MoveToStageOne"), GameObject.Find("MoveToStageTwo"), GameObject.Find("MoveToStageThree"), GameObject.Find("MoveToStageFour"), GameObject.Find("MoveToStageFive"), GameObject.Find("MoveToStageSix") };
                for (int i = 0; i < beFlyGameObjects.Length; i++)
                {
                    beFlyGameObjects[i].GetComponent<fly>().enabled = true;
                }
            }
            else if ((progressNum / 100) == 7)
            {
                //6까지 열려있어야한다.
                GameObject[] otherGameObjects = { GameObject.Find("LockedStageOne"), GameObject.Find("LockedStageTwo"), GameObject.Find("LockedStageThree"), GameObject.Find("LockedStageFour"), GameObject.Find("LockedStageFive"), GameObject.Find("LockedStageSix") };
                for (int i = 0; i < otherGameObjects.Length; i++)
                {
                    otherGameObjects[i].SetActive(false);
                }

                GameObject[] beFlyGameObjects = { GameObject.Find("MoveToStageOne"), GameObject.Find("MoveToStageTwo"), GameObject.Find("MoveToStageThree"), GameObject.Find("MoveToStageFour"), GameObject.Find("MoveToStageFive"), GameObject.Find("MoveToStageSix") };
                for (int i = 0; i < beFlyGameObjects.Length; i++)
                {
                    beFlyGameObjects[i].GetComponent<fly>().enabled = true;
                }
            }
            else if ((progressNum / 100) == 8)
            {
                //6까지 열려있어야한다.
                GameObject [] otherGameObjects = { GameObject.Find("LockedStageOne"), GameObject.Find("LockedStageTwo"), GameObject.Find("LockedStageThree"), GameObject.Find("LockedStageFour"), GameObject.Find("LockedStageFive"), GameObject.Find("LockedStageSix") };
                for(int i = 0; i< otherGameObjects.Length; i++)
                {
                    otherGameObjects[i].SetActive(false);
                }

                GameObject[] beFlyGameObjects = { GameObject.Find("MoveToStageOne"), GameObject.Find("MoveToStageTwo"), GameObject.Find("MoveToStageThree"), GameObject.Find("MoveToStageFour"), GameObject.Find("MoveToStageFive"), GameObject.Find("MoveToStageSix") };
                for (int i = 0; i < beFlyGameObjects.Length; i++)
                {
                    beFlyGameObjects[i].GetComponent<fly>().enabled = true;
                }
            }
        }
        else
        {
            Debug.LogError("KooksUserManager not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
