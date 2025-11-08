using System.Collections.Generic;
using UnityEngine;

enum CardID
{
    None = 0,
    Asteroid = 1
}

public class LevelGenerator : MonoBehaviour
{
    private float _currentLevelCursorX = 35.5f;
    private static readonly float[] _yOffsets = { -7.5f, -2.5f, 2.5f, 7.5f };

    private CellDataSO[] _currentColumn = new CellDataSO[4];
    private CellDataSO[] _lastColumn = new CellDataSO[4];

    [SerializeField] private List<CellDataSO> _cards;

    private void Start()
    {
        for (int i = 0; i < 5; i++) GenerateColumn();
    }

    private void GenerateColumn()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2 pos = new Vector2(_currentLevelCursorX, _yOffsets[i]);
            CellDataSO horizontalNeighbor = _lastColumn[i];
            CellDataSO verticalNeighbor = (i > 0) ? _currentColumn[i - 1] : null;
            List<bool> bordering = new List<bool>();
            for (int j = 0; j < _cards.Count; j++) bordering.Add(horizontalNeighbor.Id == _cards[j]?.Id || verticalNeighbor.Id == _cards[j]?.Id);
            _currentColumn[i] = GenerateLevelCell(pos, bordering);
        }
        _lastColumn = _currentColumn;
        for (int i = 0; i < 4; i++) _currentColumn = null;
        _currentLevelCursorX += 5;
    }

    private CellDataSO GenerateLevelCell(Vector2 pos, List<bool> bordering)
    {
        float totalWeight = CalculateTotalWeight(bordering);
        float selector = Random.Range(0f, totalWeight);
        float previousWeight = 0f;
        for (int i = 0; i < _cards.Count; i++)
        {
            if (selector > previousWeight && selector <= previousWeight + _cards[i].Weight(bordering[i]))
            {
                GameObject obj = GameObject.Instantiate(_cards[i].GetPrefab());
                obj.transform.position = pos;
                return _cards[i];
            }
            previousWeight += _cards[i].Weight(bordering[i]);
        }
        return null;
    }

    private float CalculateTotalWeight(List<bool> bordering)
    {
        float result = 0;
        for (int i = 0; i < _cards.Count; i++)
        {
            result += _cards[i].Weight(bordering[i]);
        }
        return result;
    }
}
