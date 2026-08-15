using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EdibleItem : MonoBehaviour, IEdible, IPoolable
{   
    public int xpValue = 1;

    public GameObject eatenVFXPrefab;
    public AudioClip eatenSFX;

    ObjectPooling pooling;
    public int XPvalue => xpValue;

    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }



    public void OnEaten(GameObject eater)
    {
       if(eatenVFXPrefab != null)
        {
            GameObject vfx = Instantiate(eatenVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 2f);
        }

       if(eatenSFX != null)
        {
            AudioSource.PlayClipAtPoint(eatenSFX, transform.position);

        } 
       if(pooling != null)
        {
            pooling.Release(gameObject);
        }

       else
        {
            Destroy(gameObject);
        } 
    }

    public void SetPool(ObjectPooling pool)
    {
        pooling = pool;
    }

   
}
