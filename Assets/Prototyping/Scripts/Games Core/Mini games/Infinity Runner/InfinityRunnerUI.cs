using Prototyping.Games;
using UnityEngine;

public class InfinityRunnerUI : MonoBehaviour
{

    [SerializeField] private PlayerRunnerManager manager;
    [SerializeField] private Transform endGameUI;

    public void RestartGame()
    {
        manager.RestartGame();
    }

    public void ExitGame()
    {
        manager.ExitGame();
        endGameUI.gameObject.SetActive(false);
    }

}
