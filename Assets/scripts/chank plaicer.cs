using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chankPlaicer : MonoBehaviour
{
    public Transform Player;
    public chank[] ChankPrefabs;
    public chank FirstChunk;
    private List<chank> spawnsChunk = new List<chank>();
    public GameObject carPrefab;
    private System.Random rand = new System.Random();
    void Start()
    {
        spawnsChunk.Add(FirstChunk);
    }


    void Update()
    {
        float side = rand.Next(1, 3) == 1 ? -1f : 1f; 
        var last = spawnsChunk[spawnsChunk.Count - 1];

        if (Player.position.z > spawnsChunk[spawnsChunk.Count - 1].end.position.z-200)
        {
            SpawnChank();
            var car = Instantiate(carPrefab, new Vector3(last.transform.position.x-50 , last.transform.position.y-59 , last.transform.position.z  * side), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            car.transform.SetParent(gameObject.transform);
        }
    
    }

    void SpawnChank()
    {
        chank newChunk = Instantiate(ChankPrefabs[Random.Range(0,ChankPrefabs.Length)]);
        newChunk.transform.position= spawnsChunk[spawnsChunk.Count-1].end.position-newChunk.begin.localPosition;
        spawnsChunk.Add(newChunk);
        if(spawnsChunk.Count >= 10) 
        {
            Destroy(spawnsChunk[0].gameObject);
            spawnsChunk.RemoveAt(0);
        }
    }
}
