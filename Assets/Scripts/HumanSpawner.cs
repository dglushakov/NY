using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    public GameObject[] Humans;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnHuman", 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnHuman()
    {
        float humanSpawnInterval = Random.Range(8, 19.0f);
        int humanNumber = Random.Range(0, Humans.Length);

        GameObject human = Humans[humanNumber];
        Transform suits = human.transform.Find("human_mesh/cloth/complet_suits");

        for(int i=0; i< suits.childCount; i++ )
            suits.GetChild(i).gameObject.SetActive(false);

        int suitNumber = Random.Range(0, suits.childCount);

        Transform suit = suits.GetChild(suitNumber);
        suit.gameObject.SetActive(true);


        Vector3 startPos = new Vector3(6f, -0.3f, 10.0f);
        Instantiate(human, startPos, Humans[humanNumber].transform.rotation);
        Invoke("SpawnHuman", humanSpawnInterval);
    }
}
