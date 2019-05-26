using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{

    public GameObject itemObject;

    public int numberToGenerate = 10;
    public float posX = 10;
    public float posY = 0;
    public float posZ = 10;
    
    void Start()
    {
        for(int i = 0; i < numberToGenerate; i++)
        {
            SpawnItem();
        }
    }

  

    void SpawnItem()
    {
        //TODO : les faire spawn autour du joueur
        Vector3 spawnPosition = new Vector3(Random.Range(-posX, posX), Random.Range(0, posY), Random.Range(-posY, posY)) + transform.position;
        GameObject clone = Instantiate(itemObject, spawnPosition, Quaternion.identity);
    }

    
}
