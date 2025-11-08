using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CellDataSO", menuName = "Scriptable Objects/CellDataSO")]
public class CellDataSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _id;
    [SerializeField] private float _baseWeight;
    [SerializeField] private float _bonusWeight;

    [SerializeField] private List<GameObject> prefabs;

    public float Weight(bool bordering) { return bordering ? _baseWeight + _bonusWeight : _baseWeight; }
    public string Name { get { return _name; } }
    public int Id { get { return _id; } }

    public GameObject GetPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Count)];
    }
}
