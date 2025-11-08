using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CellDataSO", menuName = "Scriptable Objects/CellDataSO")]
public class CellDataSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _id;
    [SerializeField] private float _baseWeight;
    [SerializeField] private float _bonusWeight;

    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

    public float Weight(bool bordering) { return bordering ? _baseWeight + _bonusWeight : _baseWeight; }
    public string Name { get { return _name; } }
    public int Id { get { return _id; } }

    public GameObject GetPrefab()
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        if (prefabs == null) return null;
        if (prefabs.Count == 0) return null;
        int randomIndex = Random.Range(0, prefabs.Count - 1);
        return prefabs[randomIndex];
=======
        if (prefabs?.Count == 0) return null;
        return prefabs[Random.Range(0, prefabs.Count - 1)];
>>>>>>> Stashed changes
=======
        if (prefabs?.Count == 0) return null;
        return prefabs[Random.Range(0, prefabs.Count - 1)];
>>>>>>> Stashed changes
    }
}
