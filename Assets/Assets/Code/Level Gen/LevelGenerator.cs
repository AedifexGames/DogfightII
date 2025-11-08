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

<<<<<<< Updated upstream
<<<<<<< Updated upstream
    private List<int> _currentColumn;
    private List<int> _lastColumn;
=======
    private CellDataSO[] _currentColumn;
    private CellDataSO[] _lastColumn;
>>>>>>> Stashed changes
=======
    private CellDataSO[] _currentColumn;
    private CellDataSO[] _lastColumn;
>>>>>>> Stashed changes

    [SerializeField] private List<CellDataSO> _cards = new List<CellDataSO>();

    private void Start()
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        _lastColumn = new List<int> { 0, 0, 0, 0 };
        _currentColumn = new List<int> { 0, 0, 0, 0 };
=======
        _lastColumn = new CellDataSO[4]{ null, null, null, null };
        _currentColumn = new CellDataSO[4] { null, null, null, null };
>>>>>>> Stashed changes
=======
        _lastColumn = new CellDataSO[4]{ null, null, null, null };
        _currentColumn = new CellDataSO[4] { null, null, null, null };
>>>>>>> Stashed changes
        for (int i = 0; i < 5; i++) GenerateColumn();
    }

    public void GenerateColumn()
    {
        
        for (int i = 0; i < 4; i++)
        {
            Vector2 pos = new Vector2(_currentLevelCursorX, _yOffsets[i]);
<<<<<<< Updated upstream
            int horizontalNeighbor = _lastColumn[i];
            int verticalNeighbor = (i > 0) ? _currentColumn[i - 1] : 0;
            bool[] bordering = new bool[_cards.Count];
            for (int j = 0; j < _cards.Count; j++) { bordering[j] = (horizontalNeighbor == _cards[j].Id || verticalNeighbor == _cards[j].Id); }
=======
            CellDataSO horizontalNeighbor = _lastColumn[i];
            CellDataSO verticalNeighbor = (i > 0) ? _currentColumn[i - 1] : null;
            List<bool> bordering = new List<bool>();
            for (int j = 0; j < _cards.Count; j++) { bordering.Add(horizontalNeighbor?.Id == _cards[j].Id || verticalNeighbor?.Id == _cards[j].Id); }
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
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
            if (_cards == null)
                Debug.LogError("Cards list is null!");
            if (_cards[i] == null)
                Debug.LogError($"Card {i} is null");
            if (bordering == null)
                Debug.LogError("Bordering list is null!");
            else if (bordering.Count <= i)
                Debug.LogError($"Bordering list too short: {bordering.Count} < {_cards.Count}");
            if (selector > previousWeight && selector <= previousWeight + _cards[i].Weight(bordering[i]))
            {
                GameObject prefab = _cards[i].GetPrefab();
<<<<<<< Updated upstream
<<<<<<< Updated upstream
                if (prefab == null) return _cards[i].Id;
                GameObject obj = GameObject.Instantiate(prefab);
                if (obj == null) return _cards[i].Id;
=======
                if (prefab == null) return _cards[i];
                GameObject obj = GameObject.Instantiate(prefab);
                if (obj == null) return _cards[i];
>>>>>>> Stashed changes
=======
                if (prefab == null) return _cards[i];
                GameObject obj = GameObject.Instantiate(prefab);
                if (obj == null) return _cards[i];
>>>>>>> Stashed changes
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
