using Prototyping.Games;
using UnityEngine;

public class InfinityRunnerUI : MonoBehaviour
{
    [SerializeField] private Transform endGameUI;

    public void RestartGame()
    {

    }

    public void ExitGame()
    {
        endGameUI.gameObject.SetActive(false);
    }

}
