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

        public RunnerManagerBase CurrentManager { get; set; }
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
            CurrentManager = manager;
            currentMaxScore = saveSystem.GetScore(CurrentManager.LaneCount);
            currentScore = 0;
            CurrentManager.OnGameStart();
            scoreBeatingInCurrentRun = false;
        }

        public void RenderMain()
        {
            onRenderMain.Invoke();
        }

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
            saveSystem.FinishedGameScore(currentScore, CurrentManager.LaneCount);
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
