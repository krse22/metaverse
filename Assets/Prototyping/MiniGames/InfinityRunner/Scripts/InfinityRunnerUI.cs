using Prototyping.Games;
using UnityEngine;

public class InfinityRunnerUI : MonoBehaviour
{

    [SerializeField] private PlayerRunnerManager threeLanemanager;
    [SerializeField] private PlayerRunnerManager fiveLaneManager;

    [SerializeField] private Transform endGameUI;

    public void RestartGame()
    {
        threeLanemanager.Play();
    }

    public void ExitGame()
    {
        fiveLaneManager.ExitGame();
        endGameUI.gameObject.SetActive(false);
    }

}
