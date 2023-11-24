using UnityEngine;
using UnityEngine.Events;

namespace Prototyping.Games
{
    public class InfinityRunnerManagerCurrent : MonoBehaviour
    {

        [SerializeField] private UnityEvent<int> onScoreUpdated;
        public RunnerManagerBase CurrentManager { get; set; }
        private int currentScore = 0;

        public void StopCurrent()
        {
            CurrentManager.Pause();
        }

        public void ContinueCurrent()
        {
            CurrentManager.Unpause();
        }

        public void ExitCurrent()
        {
            CurrentManager.OnGameEnd();
            CurrentManager.ObjectCleanup();
        }

        public void GameEnd()
        {
            GetComponent<InfinityRunnerSaveSystem>().FinishedGameScore(currentScore, CurrentManager.LaneCount);
        }

        public void UpdateScore(int updatedScore)
        {
            if (updatedScore != currentScore)
            {
                onScoreUpdated.Invoke(updatedScore);
                currentScore = updatedScore;
            }
        }


    }
}
