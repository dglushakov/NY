using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{ 
    // Start is called before the first frame update
    public GameObject[] Cars;

    void Start()
    {
        Invoke("SpawnCar", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCar()
    {
        float carSpawnInterval = Random.Range(3, 10.0f);
        int carNumber = Random.Range(0, Cars.Length);

        int carDirection = Random.Range(0, 2);
        if (carDirection == 0) {
            Instantiate(Cars[carNumber], new Vector3(2.5f, -1.0f, -60.0f), transform.rotation);
        } else if (carDirection == 1)
        {
            Instantiate(Cars[carNumber], new Vector3(-2.5f, -1.0f, 19.0f), Quaternion.Euler(0, 180, 0));
        }
        
        
        Invoke("SpawnCar", carSpawnInterval);
    }

}
