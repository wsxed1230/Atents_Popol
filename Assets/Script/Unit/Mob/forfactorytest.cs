using UnityEngine;

public class forfactorytest : MonoBehaviour
{
    public Monster monster;
    // Start is called before the first frame update
    void Awake()
    {
        MonsterFactory factory = new MonsterFactory();
        GameObject obj = factory.CreateMonster(30001);

        //monster.idleAI = new System.Collections.Generic.List<int>();
        //monster.idleAI.Add(0);
        monster = obj.GetComponent<Monster>();
        monster.CinematicEnd();
        
    }
}
