using UnityEngine;

namespace Prototyping.Games
{
    public class InfinityRunnerManager : RunnerManagerBase
    {

        private float currentScore = 0;

        public override void OnGameEnd()
        {
            isPlaying = false;
            current.GameEnd();
        }

        public override void OnGameStart() {
            base.OnGameStart();
            currentScore = 0;
        }

        void Update()
        {
            if (isPlaying)
            {
                currentScore = currentScore + (movementSpeed * Time.deltaTime);
                current.UpdateScore(Mathf.CeilToInt(currentScore));
            }
        }

    }
}

