using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    // distance between last spawned platform and target
    const float _targetDistance = 50f;

    [SerializeField] float _gap = 8f;
    [SerializeField] Transform _target;
    [SerializeField] Transform _platformPrefab;
    [SerializeField] int _difficultyIncreaseInterval = 5;

    Vector3 _lastSpawnPosition;
    int _platformCount = 0;
    float _timeScale = 1f;

    bool _isRevived = false;
    float _lastTimeScale = 1f;

    private void Start()
    {
        _lastSpawnPosition = Vector3.forward * _gap;
    }

    private void Update()
    {
        if (_lastSpawnPosition.z - _target.position.z < _targetDistance)
        {
            if (_platformCount >= 10) CalculateDifficulty();
            SpawnPlatform(_platformPrefab, _lastSpawnPosition);
        }
    }

    public void Revive()
    {
        _isRevived = true;
    }

    public void OnGameOver()
    {
        _lastTimeScale = _timeScale;
        _timeScale = 1f;
        Time.timeScale = _timeScale;
    }

    void SpawnPlatform(Transform prefab, Vector3 position)
    {
        Transform newPlatform = Instantiate(prefab, transform);
        newPlatform.position = position;

        _lastSpawnPosition += Vector3.forward * _gap;
        _platformCount++;
    }

    void CalculateDifficulty()
    {
        if (Mathf.Repeat(_platformCount, _difficultyIncreaseInterval) == 0)
        {
            _timeScale += .01f;
            Time.timeScale = Mathf.Clamp(_timeScale, 1f, 2f);
        }

        if (_isRevived && _timeScale < _lastTimeScale)
        {
            _timeScale += .05f;
            if (_timeScale < _lastTimeScale) return;
            _timeScale = _lastTimeScale;
        }
    }
}
