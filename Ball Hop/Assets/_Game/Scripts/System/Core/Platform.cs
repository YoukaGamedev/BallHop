using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] int maxPosX = 5;
    [SerializeField] MeshRenderer _renderer;
    [SerializeField] GameData _data;
    [SerializeField] ParticleSystem _splatFx;

    Vector3 childPos;

    public static event Action OnCollideWithPlayer;

    private void Start()
    {
        _renderer.material = _data.GetRandomMaterial;

        childPos = transform.GetChild(0).transform.localPosition;
        childPos.x = UnityEngine.Random.Range(-maxPosX, maxPosX+1);
        transform.GetChild(0).transform.localPosition = childPos;

        LeanTween.moveLocalY(_renderer.transform.gameObject, -.5f, .5f).setEase(LeanTweenType.easeOutQuad);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.GetContact(0).normal.y != -1f) return;

        if (collision.collider.CompareTag("Player"))
        {
            OnCollideWithPlayer?.Invoke();
            _splatFx.transform.position = collision.GetContact(0).point + (Vector3.up * .01f);
            _splatFx.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}