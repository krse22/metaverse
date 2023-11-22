using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototyping.Games
{
    public class InfinityRunnerManagerCurrent : MonoBehaviour
    {
        public RunnerManagerBase currentManager;

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

    }
}
