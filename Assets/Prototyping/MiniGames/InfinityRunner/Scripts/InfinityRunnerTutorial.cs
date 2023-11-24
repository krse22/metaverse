using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Prototyping.Games
{
    public class InfinityRunnerTutorial : RunnerManagerBase
    {

        [Serializable]
        public class TutorialObject
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
        private string currentTutorialSide = "";
        private float initialMovemendSpeed = 0f;

        [SerializeField] private Image[] buttonImages;

        public void StartTutorial()
        {
            current.CurrentManager = this;
            ObjectCleanup();
            InitSystems();
            tutorialTraps.ToList().ForEach(t => t.Init());
            isPlaying = true;
            InitController();
            initialMovemendSpeed = movementSpeed;
            buttonImages.ToList().ForEach((btn) =>
            {
                btn.raycastTarget = false;
                btn.color = new Color(1f, 1f, 1f, 0.5f);
            });
        }


        void InitController()
        {
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
            controller = player.GetComponent<PlayerRunnerController>();
            PlayerCoreCamera.SetCameraOwner(controller);
            int[] lanes = InfinityRunnerUtils.GenerateLanes(laneCount);
            controller.Play(lanes, sideDashDistance, this);
        }

        public void ExitTutorial()
        {
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
            buttonImages.ToList().ForEach((btn) =>
            {
                btn.raycastTarget = true;
                btn.color = Color.white;
                btn.gameObject.GetComponent<Animation>().Play("IdleButton");
            });
        }

        public override void OnGameEnd()
        {
            isPlaying = false;
            onGameEndEvent.Invoke();
            movementSpeed = initialMovemendSpeed;
        }

        public void OnTutorialHit(string side)
        {
            movementSpeed = 2f;
            currentTutorialSide = side;
            buttonImages.ToList().ForEach((btn) =>
            {
                if (btn.GetComponent<InfinityRunnerTutorialActivator>().side != side)
                {
                    btn.raycastTarget = false;
                    btn.color = new Color(1f, 1f, 1f, 0.5f);
                } else
                {
                    btn.raycastTarget = true;
                    btn.color = Color.white;
                    btn.gameObject.GetComponent<Animation>().Play("TutorialButtonAnimation");
                }
            });
        }

        public void ButtonClicked(string side)
        {
            if (side == currentTutorialSide && isPlaying)
            {
                movementSpeed = initialMovemendSpeed;
                buttonImages.ToList().ForEach((btn) =>
                {
                    if (btn.GetComponent<InfinityRunnerTutorialActivator>().side == side)
                    {
                        Animation anim = btn.gameObject.GetComponent<Animation>();
                        anim.Play("IdleButton");
                        btn.raycastTarget = false;
                        btn.color = new Color(1f, 1f, 1f, 0.5f);
                    }
    
                });
            }
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
