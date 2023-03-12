using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chankplaicer : MonoBehaviour
{
    public Transform Player;
    public chank[] ChankPrefabs;

    private List<chank> spawnsChunk = new List<chank>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnChank()
    {
        chank newChunk = Instantiate(ChankPrefabs[Random.Range(0,ChankPrefabs.Length)]);
        newChunk.transform.position= spawnsChunk[spawnsChunk.Count-1].end.position-newChunk.begin.localPosition;
        spawnsChunk.Add(newChunk);
    }
}
