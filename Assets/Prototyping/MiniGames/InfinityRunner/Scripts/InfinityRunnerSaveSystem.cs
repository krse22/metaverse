using UnityEngine;
using UnityEngine.Events;

public class InfinityRunnerSaveSystem : MonoBehaviour
{

    private int currentHighScore3Lane;
    private int currentHighScore5Lane;

    [SerializeField] private UnityEvent<int> on3LaneScoreLoaded;
    [SerializeField] private UnityEvent<int> on5LaneScoreLoaded;
    [SerializeField] private bool testing;

    private void OnEnable()
    {
        LoadCurrentScores();
    }

    private void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.SetInt("IR_3_Lane", 0);
            PlayerPrefs.SetInt("IR_5_Lane", 0);
        }
    }

    void LoadCurrentScores()
    {
        currentHighScore3Lane = PlayerPrefs.GetInt("IR_3_Lane");
        currentHighScore5Lane = PlayerPrefs.GetInt("IR_5_Lane");

        on3LaneScoreLoaded.Invoke(currentHighScore3Lane);
        on5LaneScoreLoaded.Invoke(currentHighScore5Lane);
    }

    public void FinishedGameScore(int finishedScore, int laneCount)
    {
        if (finishedScore > PlayerPrefs.GetInt($"IR_{laneCount}_Lane"))
        {
            PlayerPrefs.SetInt($"IR_{laneCount}_Lane", finishedScore);
            LoadCurrentScores();
        }
    }

    public int GetScore(int lanes)
    {
        return PlayerPrefs.GetInt($"IR_{lanes}_Lane");
    }

}
