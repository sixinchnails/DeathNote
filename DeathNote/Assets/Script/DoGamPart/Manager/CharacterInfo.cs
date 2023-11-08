using UnityEngine;

[System.Serializable]
public class CharacterInfo
{
    public int id;
    public string soulName;
    public bool state;
    public string passive;
    public string active1;
    public string active2;
    public string active3;
    public string nickname;
    public string skillDescription;
}

[System.Serializable]
public class SoulsList
{
    public CharacterInfo[] souls; 
}
