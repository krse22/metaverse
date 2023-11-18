using UnityEngine;

namespace Prototyping.Games
{
    public class InfinityRunnerManager : RunnerManagerBase, IGamePortal
    {

        public override void OnGameEnd()
        {
            isPlaying = false;
            onGameEndEvent.Invoke();
        }

        // Called directly from Buttons in UI
        public void Play() {
            ObjectCleanup();
            InitSystems();
            InitController();

            isPlaying = true;
        }

        void InitController()
        {
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
            controller = player.GetComponent<PlayerRunnerController>();
            PlayerCoreCamera.SetCameraOwner(controller);
            int[] lanes = InfinityRunnerUtils.GenerateLanes(laneCount);
            controller.Play(lanes, sideDashDistance, this);
        }

        public void ExitGame()
        {
            GameCore.Instance.ExitPortal();
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
        }   

    }
}

