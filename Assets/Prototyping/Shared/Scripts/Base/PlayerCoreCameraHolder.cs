using UnityEngine;

namespace Prototyping
{
    public interface ICameraHolder
    {
        (Vector3, Vector3) PositionAndRotation();
    }
}
