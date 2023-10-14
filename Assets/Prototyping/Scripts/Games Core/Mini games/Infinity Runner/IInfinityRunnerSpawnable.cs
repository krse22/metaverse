using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototyping.Games {
    public interface IInfinityRunnerSpawnable
    {
        public float Length { get; }
        public void SetManager(PlayerRunnerManager manager);
    }
}