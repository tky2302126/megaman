using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("SCORE: " + score);
    }

    public void OnGameEnd(bool isPlayerWin)
    {
        if (isPlayerWin)
        {
            Debug.Log("YOU WIN");
        }
        else
        {
            Debug.Log("YOU LOSE");
        }

        // 今後：結果画面へ遷移、リトライ、タイトルに戻る等
    }
}
