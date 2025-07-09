using Sans.UI.Menu;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScheduler : Singleton<MenuScheduler>
{
    [SerializeField] MenuController _controller;

    protected override void Awake()
    {
        base.Awake();
    }

    private void HandleOnSceneLoaded(Scene scene, LoadSceneMode lsm)
    {
        if (scene.buildIndex == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleOnSceneLoaded;
        GameManager.OnStartGame += HandleOnStartGame;
        GameManager.OnEndGame += HandleOnEndGame;
        Admob.OnRewardedAdWatched += HandleOnRewardedWatched;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleOnSceneLoaded;
        GameManager.OnStartGame -= HandleOnStartGame;
        GameManager.OnEndGame -= HandleOnEndGame;
        Admob.OnRewardedAdWatched -= HandleOnRewardedWatched;
    }

    private void HandleOnRewardedWatched()
    {
        _controller.SwitchMenu(MenuType.Gameplay);
    }

    private void HandleOnEndGame(bool canRevive)
    {
        if (canRevive) _controller.SwitchMenu(MenuType.Revive);
        else _controller.SwitchMenu(MenuType.GameOver);

    }

    private void Start()
    {
        _controller.OpenMenu(MenuType.Main);
    }

    void HandleOnStartGame()
    {
        if (_controller.GetCurrentMenu == MenuType.Gameplay) return;

        _controller.SwitchMenu(MenuType.Gameplay);
    }
}
