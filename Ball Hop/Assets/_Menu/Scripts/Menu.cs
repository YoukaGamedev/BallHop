using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sans.UI.Menu
{
    public abstract class Menu : MonoBehaviour
    {
        [SerializeField] MenuType _type;
        [SerializeField] bool _useAnimation = false;

        protected MenuController _menuController;

        public MenuType Type => _type;

        public UnityEvent OnSetDisable;

        protected virtual void Awake()
        {
            _menuController = transform.parent.GetComponent<MenuController>();

            if(!_menuController)
            {
                Debug.LogWarning("Can't find Menu Controller!");
            }
        }

        public void SetDisable()
        {
            if (!_useAnimation)
            {
                gameObject.SetActive(false);
                return;
            }

            // gameObject should be disable in this event.
            OnSetDisable?.Invoke();
        }

        protected void OnButtonPressed(Button button, UnityAction buttonListener)
        {
            if (!button)
            {
                Debug.LogWarning($"There is a 'Button' that is not attached to the '{gameObject.name}' script,  but a script is trying to access it.");
                return;
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(buttonListener);
        }
    }
}
