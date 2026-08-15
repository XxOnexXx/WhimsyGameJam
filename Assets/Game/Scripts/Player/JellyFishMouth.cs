using System;
using UnityEngine;

public class JellyFishMouth : MonoBehaviour
{   

    public event Action<int> OnFoodEaten;
      void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
 
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IEdible>(out var edible))
        {
            int xp = edible.XPvalue;
            edible.OnEaten(gameObject);
            OnFoodEaten?.Invoke(xp);
        }
    }
}
