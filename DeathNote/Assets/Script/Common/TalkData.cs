using System.Collections;
using System.Collections.Generic;

public class TalkData
{
    //id 0이면 설명문, 1이면 유저 대사, 2이면 사신 대사
    public int id;
    public string content;

    public TalkData(int id, string content)
    {
        this.id = id; 
        this.content = content;
    }
}