using UnityEngine;

[CreateAssetMenu()]
public class GameData : ScriptableObject
{
    [SerializeField] Material[] _platMats;

    public Material GetRandomMaterial => _platMats[Random.Range(0, _platMats.Length)];
}