using Unity.VisualScripting;
using UnityEngine;

namespace Prototyping.Games
{
    public class InfinityRunnerInputSystem : MonoBehaviour
    {

        private static PlayerRunnerController controller;

        public static void RegisterController(PlayerRunnerController controllerInstance)
        {
            controller = controllerInstance;
        }

        private void Update()
        {
            Inputs();
        }

        void Inputs()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Left();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Right();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Up();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Slide();
            }
        }

        public void Left()
        {
            if (controller != null)
            {
                controller.SlideLeft();
            }
        }

        public void Right()
        {
            if (controller != null)
            {
                controller.SlideRight();
            }
        }

        public void Up()
        { 
            if(controller != null)
            {
                controller.Jump();
            }
        }

        public void Slide()
        {
            if (controller != null)
            {
                controller.Slide();
            }
        }

    }
}
