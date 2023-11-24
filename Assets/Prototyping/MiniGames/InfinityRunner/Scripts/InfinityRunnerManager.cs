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

        // Called directly from Buttons in UI
        public override void OnGameStart() {
            currentScore = 0;
            ObjectCleanup();
            InitSystems();
            InitController();

            isPlaying = true;
        }

        void Update()
        {
            if (isPlaying)
            {
                currentScore = currentScore + (movementSpeed * Time.deltaTime);
                current.UpdateScore(Mathf.CeilToInt(currentScore));
            }
        }

        void InitController()
        {
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
            controller = player.GetComponent<PlayerRunnerController>();
            PlayerCoreCamera.SetCameraOwner(controller);
            int[] lanes = InfinityRunnerUtils.GenerateLanes(laneCount);
            controller.Play(lanes, sideDashDistance, this);
        }

    }
}

