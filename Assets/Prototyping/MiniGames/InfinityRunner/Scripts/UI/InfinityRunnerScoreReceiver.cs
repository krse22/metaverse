using UnityEngine;
using TMPro;
public class InfinityRunnerScoreReceiver : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;

    public void SetScoreText(int score)
    {
        text.text = score.ToString();
    }

}
