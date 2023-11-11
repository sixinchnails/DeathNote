using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using : import
using UnityEngine.SceneManagement;

public class BirdJump_1 : MonoBehaviour
    //: MonoBehaviour: 라는 걸 상속 받는다.
{
    Rigidbody2D rb;

    public float jumpPower;
    //public으로 선언하면 유니티에서도 값을 조절할 수 있음

    //Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        //Rigidbody2D라는 컴포넌트를 들고와서 rb에 담겠다.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            //Input.GetMouseButtonDown : 마우스를 눌렀을 때
            //Input.GetMouseButton(0) : 왼쪽 마우스를 눌렀을 때
        {
            GetComponent<AudioSource>().Play();
            // 눌렀을 때 소리가 나오도록 해주는 함수
            rb.velocity = Vector2.up * jumpPower; // (0,1)
            //velocity: 속도
                //Vector2:2d
                //Vector2.up : (0,1)
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
        //OnCollisionEnter2D : 부딪히는 이벤트에 사용되는 메소드
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
