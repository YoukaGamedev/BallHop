using System.Collections;
using UnityEngine;

public class MenuTween : MonoBehaviour
{
    [SerializeField] protected TweenUI[] _objectToAnimate;

    // disable this game object after animate all child
    public void SetDisableGameobject()
    {
        float tweenDuration = 0f;
        foreach (var obj in _objectToAnimate)
        {
            obj.HandleOnDisable();

            // get the longest onDisable tween duration
            float duration = obj.GetTweenDuraionOnDisable();
            if (tweenDuration < duration)
                tweenDuration = duration;
        }

        if (gameObject.activeSelf)
            StartCoroutine(DisableGameobjectRoutine(gameObject, tweenDuration));
    }

    protected IEnumerator DisableGameobjectRoutine(GameObject go, float duration)
    {
        yield return new WaitForSeconds(duration);
        go.SetActive(false);
    }
}
