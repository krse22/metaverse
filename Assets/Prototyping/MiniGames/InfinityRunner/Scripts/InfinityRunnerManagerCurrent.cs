using UnityEngine;
using UnityEngine.Events;

namespace Prototyping.Games
{
    public class InfinityRunnerManagerCurrent : MonoBehaviour
    {

        [SerializeField] private UnityEvent<int> onScoreUpdated;
        [SerializeField] private UnityEvent onHighScoreBeaten;
        [SerializeField] private UnityEvent onGameStartEvent;
        [SerializeField] private UnityEvent onGameEndEvent;
        [SerializeField] private UnityEvent onRenderMain;
        [SerializeField] private UnityEvent onTutorialFailed;

        private RunnerManagerBase currentManager;
        private int currentScore = 0;
        private int currentMaxScore = 0;

        private InfinityRunnerSaveSystem saveSystem;

        private bool scoreBeatingInCurrentRun = false;

        private void Start()
        {
            saveSystem = GetComponent<InfinityRunnerSaveSystem>();
        }

        public void Play(RunnerManagerBase manager)
        {
            onGameStartEvent.Invoke();
            currentManager = manager;
            currentMaxScore = saveSystem.GetScore(currentManager.LaneCount);
            currentScore = 0;
            currentManager.OnGameStart();
            scoreBeatingInCurrentRun = false;
        }

        public void RenderMain()
        {
            onRenderMain.Invoke();
            saveSystem.FinishedGameScore(currentScore, currentManager.LaneCount);
        }

        public void StopCurrent()
        {
            currentManager.Pause();
        }

        public void ContinueCurrent()
        {
            currentManager.Unpause();
        }

        public void ExitCurrent()
        {
            currentManager.OnGameEnd();
            currentManager.ObjectCleanup();
        }

        public void GameEnd()
        {
            saveSystem.FinishedGameScore(currentScore, currentManager.LaneCount);
            onGameEndEvent.Invoke();
        }

        public void OnTutorialFailed()
        {
            onTutorialFailed.Invoke();
        }

        public void UpdateScore(int updatedScore)
        {
            if (updatedScore != currentScore)
            {
                onScoreUpdated.Invoke(updatedScore);
                currentScore = updatedScore;
            }

            if (updatedScore > currentMaxScore && !scoreBeatingInCurrentRun && currentMaxScore != 0)
            {
                scoreBeatingInCurrentRun = true;
                onHighScoreBeaten.Invoke();
            }
        }


    }
}
