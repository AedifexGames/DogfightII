using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
   public void Spawn(GameObject obj)
    {
        GameObject inst = Instantiate(obj, null);
        inst.transform.position = transform.position;
    }
}
