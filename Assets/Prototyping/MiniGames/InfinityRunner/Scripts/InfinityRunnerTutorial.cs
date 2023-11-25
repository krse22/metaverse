using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Prototyping.Games
{

    public static class ButtonStateExtension
    {
        public static void SetButtonState(this Image button, bool enabled, string clip = "", bool playClip = false)
        {
            button.raycastTarget = enabled;
            button.color = enabled ? UnityEngine.Color.white : new UnityEngine.Color(1f, 1f, 1f, 0.5f);
            if (playClip)
            {
                button.gameObject.GetComponent<Animation>().Play(clip);
            }
        }
    }

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
                return;
            }
            go.transform.position = startPosition;
        }
    }

    public class InfinityRunnerTutorial : RunnerManagerBase
    {
        [SerializeField] private TutorialObject[] tutorialTraps;
        [SerializeField] private Image[] buttonImages;
        private string currentTutorialSide = "";
        private float initialMovemendSpeed;

        public override void OnGameStart()
        {
            base.OnGameStart();
            initialMovemendSpeed = movementSpeed;
            tutorialTraps.ToList().ForEach(t => t.Init());
            buttonImages.ToList().ForEach((btn) =>  btn.SetButtonState(false));
        }

        public void ExitTutorial()
        {
            isPlaying = false;
            movementSpeed = initialMovemendSpeed;
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
            buttonImages.ToList().ForEach((btn) => btn.SetButtonState(true, "IdleButton", true));
        }

        public override void OnGameEnd()
        {
            isPlaying = false;
            movementSpeed = initialMovemendSpeed;
            current.OnTutorialFailed();
        }

        public void OnTutorialHit(string side)
        {
            movementSpeed = 2f;
            currentTutorialSide = side;
            buttonImages.ToList().ForEach((btn) =>
            {
                bool correctSide = btn.GetComponent<InfinityRunnerTutorialActivator>().side == side;
                btn.SetButtonState(correctSide, "TutorialButtonAnimation", correctSide);
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
                        btn.SetButtonState(false, "IdleButton", true);
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
