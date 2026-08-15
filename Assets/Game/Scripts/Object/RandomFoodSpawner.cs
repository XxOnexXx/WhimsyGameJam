using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



public class RandomFoodSpawner : MonoBehaviour
{
   public ObjectPooling foodPooling;

   public Vector3 spawnPointArea = new Vector3(20f, 10f, 20f);

   public float spawnInterval = 1.5f;

   public int maxActive = 30;

   float timer;
   int activeFoodCount;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnInterval && (maxActive <= 0 || activeFoodCount < maxActive))
        {
            timer = 0f;
            SpawnOneItem();
        }
    }

    void SpawnOneItem()
    {
        Vector3 randomOffset = new Vector3(Random.Range(-spawnPointArea.x/2f, spawnPointArea.x/2f), Random.Range(-spawnPointArea.y/2f, spawnPointArea.y/2f), Random.Range(-spawnPointArea.z/2f, spawnPointArea.z/2f));

         Vector3 spawnPos = transform.position + randomOffset;
         GameObject food = foodPooling.Get(spawnPos, Quaternion.Euler(0f, Random.Range(0f,360f), 0f));
         activeFoodCount++;

         if(foodPooling.TryGetComponent<EdibleItem>(out var edible))
        {
            StartCoroutine(LookforRelease(food));
        }
    }

    IEnumerator LookforRelease(GameObject food)
    {
        while(food!=null && food.activeInHierarchy)
        {
            yield return null;
        }
        activeFoodCount = Mathf.Max(0, activeFoodCount - 1);

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.3f);
        Gizmos.DrawCube(transform.position, spawnPointArea);
    }




}
