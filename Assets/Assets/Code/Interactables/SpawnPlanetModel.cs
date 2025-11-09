using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SpawnPlanetModel : MonoBehaviour
{
    public List<GameObject> models = new List<GameObject>();

    private void Start()
    {
        int r = Random.Range(0, models.Count);
        GameObject obj = Instantiate(models[r]);
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
    }
}
