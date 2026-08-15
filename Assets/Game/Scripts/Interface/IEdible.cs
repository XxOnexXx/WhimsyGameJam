

using UnityEngine;

public interface IEdible 
{
    
    int XPvalue {get;}
    
    void OnEaten(GameObject eater);
}
