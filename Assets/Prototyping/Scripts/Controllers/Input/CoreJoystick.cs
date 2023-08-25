using UnityEngine;

namespace Prototyping
{
    public class CoreJoystick : MonoBehaviour
    {
        public static float Horizontal { get => UltimateJoystick.GetHorizontalAxis("CoreJoystick"); }
        public static float Vertical { get => UltimateJoystick.GetVerticalAxis("CoreJoystick"); }

        public static float HorizontalRaw { get => UltimateJoystick.GetHorizontalAxisRaw("CoreJoystick"); }
        public static float VerticalRaw { get => UltimateJoystick.GetVerticalAxisRaw("CoreJoystick"); }

    }
}

