using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    //parent
    public GameObject enemies;
    
    //prefabs to be instantiated
    public GameObject octo;
    public int octoNum = 6;

    private Vector2[] _octoPos = new[]
    {
        new Vector2(-49f, 3.5f), new Vector2(-45f, 3.5f),
        new Vector2(-11f, -8f), new Vector2(-25f, -8f), new Vector2(-26f, -3f),
        new Vector2(-20f, 1f)
    };
    
    
    // Start is called before the first frame update
    void Start()
    {
        //spawn octos
        List<int> availableSpawns = new List<int>();
        int number;
        for (int i = 0; i < octoNum; i++)
        {
            do
            {
                number = Random.Range(0, _octoPos.Length);
            } while (availableSpawns.Contains(number));
            
            availableSpawns.Add(number);
            Instantiate(octo, _octoPos[number], Quaternion.identity, enemies.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
