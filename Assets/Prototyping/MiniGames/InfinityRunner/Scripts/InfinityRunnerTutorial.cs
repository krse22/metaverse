using System;
using System.Linq;
using UnityEngine;

namespace Prototyping.Games
{
    public class InfinityRunnerTutorial : RunnerManagerBase
    {

        [Serializable]
        public struct TutorialObject
        {
            public GameObject go;
            private Vector3 startPosition;
            private bool isInit;

            public void Init()
            {
                if (!isInit)
                {
                    startPosition = go.transform.position;
                    isInit = true;
                } else
                {
                    go.transform.position = startPosition;
                }
            }
        }

        [SerializeField] private TutorialObject[] tutorialTraps;

        public void StartTutorial()
        {
            ObjectCleanup();
            InitSystems();
            tutorialTraps.ToList().ForEach(t => t.Init());
            gameStarted = true;
            controller = player.gameObject.GetComponent<PlayerRunnerController>();
            int[] lanes = InfinityRunnerUtils.GenerateLanes(laneCount);
            controller.Play(lanes, sideDashDistance, this);
        }

        public override void OnGameEnd()
        {
            StartTutorial();
        }

        private void Update()
        {
            if (IsPlaying)
            {
                foreach(var t in tutorialTraps)
                {
                    Vector3 vec = t.go.transform.position;
                    t.go.transform.position = new Vector3(vec.x, vec.y, vec.z - movementSpeed * Time.deltaTime);
                }
            }
        }

    }

}
