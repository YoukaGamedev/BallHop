using UnityEngine;
using UnityEngine.Events;

public class Floor : MonoBehaviour
{
    public UnityEvent OnGameOver;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnGameOver?.Invoke();
        }
    }
}
