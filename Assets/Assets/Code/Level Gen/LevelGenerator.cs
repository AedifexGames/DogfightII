using System.Collections.Generic;
using UnityEngine;

enum CardID
{
    None = 0,
    Asteroid = 1
}

public class LevelGenerator : SingletonBehaviour<LevelGenerator>
{
    private float _currentLevelCursorX = 35.5f;
    private static readonly float[] _yOffsets = { -7.5f, -2.5f, 2.5f, 7.5f };

    private List<int> _currentColumn;
    private List<int> _lastColumn;

    [SerializeField] private List<CellDataSO> _cards = new List<CellDataSO>();

    private void Start()
    {
        _lastColumn = new List<int> { 0, 0, 0, 0 };
        _currentColumn = new List<int> { 0, 0, 0, 0 };
        for (int i = 0; i < 5; i++) GenerateColumn();
    }

    public void GenerateColumn()
    {
        
        for (int i = 0; i < 4; i++)
        {
            Vector2 pos = new Vector2(_currentLevelCursorX, _yOffsets[i]);
            int horizontalNeighbor = _lastColumn[i];
            int verticalNeighbor = (i > 0) ? _currentColumn[i - 1] : 0;
            bool[] bordering = new bool[_cards.Count];
            for (int j = 0; j < _cards.Count; j++) { bordering[j] = (horizontalNeighbor == _cards[j].Id || verticalNeighbor == _cards[j].Id); }
            _currentColumn[i] = GenerateLevelCell(pos, bordering);
        }
        _lastColumn = _currentColumn;
        for (int i = 0; i < 4; i++) _currentColumn[i] = 0;
        _currentLevelCursorX += 5;
    }

    private int GenerateLevelCell(Vector2 pos, bool[] bordering)
    {
        Debug.Log(bordering);
        float totalWeight = CalculateTotalWeight(bordering);
        float selector = Random.Range(0f, totalWeight);
        float previousWeight = 0f;
        
        for (int i = 0; i < _cards.Count; i++)
        {
            if (selector > previousWeight && selector <= previousWeight + _cards[i].Weight(bordering[i]))
            {
                GameObject prefab = _cards[i].GetPrefab();
                if (prefab == null) return _cards[i].Id;
                GameObject obj = GameObject.Instantiate(prefab);
                if (obj == null) return _cards[i].Id;
                obj.transform.position = pos;
                return _cards[i].Id;
            }
            previousWeight += _cards[i].Weight(bordering[i]);
        }
        return 0;
    }

    private float CalculateTotalWeight(bool[] bordering)
    {
        float result = 0;
        for (int i = 0; i < _cards.Count; i++)
        {
            result += _cards[i].Weight(bordering[i]);
        }
        return result;
    }
}
