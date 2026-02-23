using UnityEngine;

public class DisplayControlls : MonoBehaviour
{
    public static int PlayerScore1;
    public static int PlayerScore2;

    public GUISkin layout;

    private BallControl ballControl;

    private const int WinScore = 10;
    private static readonly Color ScoreColor = new Color(1f, 0.5f, 0f);

    private void Start()
    {
        GameObject ballObject = GameObject.FindGameObjectWithTag("Ball");
        if (ballObject != null)
        {
            ballControl = ballObject.GetComponent<BallControl>();
        }
    }

    public static void Score(string wallID)
    {
        if (wallID == "RightWall")
        {
            PlayerScore1++;
            return;
        }

        PlayerScore2++;
    }

    private void OnGUI()
    {
        GUI.skin = layout;

        DrawScores();
        DrawRestartButton();
        DrawWinnerMessage();

        GUI.color = Color.white;
    }

    private void DrawScores()
    {
        GUI.color = ScoreColor;
        GUI.Label(new Rect(Screen.width / 2 - 162, 20, 100, 100), PlayerScore1.ToString());
        GUI.Label(new Rect(Screen.width / 2 + 162, 20, 100, 100), PlayerScore2.ToString());
    }

    private void DrawRestartButton()
    {
        GUI.color = Color.white;

        if (!GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53), "REINICIAR"))
        {
            return;
        }

        PlayerScore1 = 0;
        PlayerScore2 = 0;

        if (ballControl != null)
        {
            ballControl.RestartGame();
        }
    }

    private void DrawWinnerMessage()
    {
        GUI.color = ScoreColor;

        if (PlayerScore1 >= WinScore)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER ONE WINS");
            if (ballControl != null)
            {
                ballControl.ResetBall();
            }

            return;
        }

        if (PlayerScore2 >= WinScore)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER TWO WINS");
            if (ballControl != null)
            {
                ballControl.ResetBall();
            }
        }
    }
}
