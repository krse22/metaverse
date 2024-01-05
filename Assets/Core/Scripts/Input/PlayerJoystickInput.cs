using UnityEngine;

public class PlayerJoystickInput : MonoBehaviour
{
    public static string joystickName = "Player";

    public static float Horizontal { get => UltimateJoystick.GetHorizontalAxis(joystickName); }
    public static float Vertical { get => UltimateJoystick.GetVerticalAxis(joystickName); }

    public static float HorizontalRaw { get => UltimateJoystick.GetHorizontalAxisRaw(joystickName); }
    public static float VerticalRaw { get => UltimateJoystick.GetVerticalAxisRaw(joystickName); }
}
