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
    private float _deletionOffsetFromCursor = -100f;
    private static readonly float[] _yOffsets = { -7.5f, -2.5f, 2.5f, 7.5f };

    private List<int> _currentColumn;
    private List<int> _lastColumn;
    private List<Transform> _createdColumns;

    [SerializeField] private List<CellDataSO> _cards = new List<CellDataSO>();

    private void Start()
    {
        _lastColumn = new List<int> { 0, 0, 0, 0 };
        _currentColumn = new List<int> { 0, 0, 0, 0 };
        _createdColumns = new List<Transform>();
        for (int i = 0; i < 5; i++) GenerateColumn();
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < _createdColumns.Count; i++)
        {
            if (_createdColumns[i].position.x < _currentLevelCursorX + _deletionOffsetFromCursor)
            {
                GameObject toDestroy = _createdColumns[i].gameObject;
                _createdColumns.RemoveAt(i);
                Destroy(toDestroy);
                i--;
            }
        }
    }
    public void GenerateColumn()
    {
        GameObject parentObj = new GameObject("Parent at: " + _currentLevelCursorX);
        Transform parent = parentObj.transform;
        _createdColumns.Add(parent);
        parent.transform.position = Vector3.right * _currentLevelCursorX;
        for (int i = 0; i < 4; i++)
        {
            Vector2 pos = new Vector2(0, _yOffsets[i]);
            int horizontalNeighbor = _lastColumn[i];
            int verticalNeighbor = (i > 0) ? _currentColumn[i - 1] : 0;
            bool[] bordering = new bool[_cards.Count];
            for (int j = 0; j < _cards.Count; j++) { bordering[j] = (horizontalNeighbor == _cards[j].Id || verticalNeighbor == _cards[j].Id); }
            _currentColumn[i] = GenerateLevelCell(pos, bordering, parent);
        }
        _lastColumn = _currentColumn;
        for (int i = 0; i < 4; i++) _currentColumn[i] = 0;
        _currentLevelCursorX += 5;
    }

    private int GenerateLevelCell(Vector2 pos, bool[] bordering, Transform parent)
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
                GameObject obj = GameObject.Instantiate(prefab, parent);
                if (obj == null) return _cards[i].Id;
                obj.transform.localPosition = pos;
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
