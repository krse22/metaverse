using UnityEngine;
using TMPro;
public class InfinityRunnerScoreReceiver : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string prefix;

    public void SetScoreText(int score)
    {
        text.text = prefix + score.ToString();
    }

}
