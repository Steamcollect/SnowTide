using System.Diagnostics;

namespace BT.Save
{
    [System.Serializable]
    public class ContentSaved
    {
        public int[] highscores = new int[3];

        public void AddScore(int score)
        {
            for (int i = 0; i < highscores.Length; i++)
            {
                if (highscores[i] < score)
                {
                    for (int j = highscores.Length-1; j > i; j--)
                    {
                        highscores[j] = highscores[j - 1];
                    }
                    highscores[i] = score;
                    return;
                }
            }
        }

        // Score
        public int totalScore;

        // Distance
        public int totalDistanceReach;
        public int maxDistanceReach;
        public int totalDriftReach;
        public int maxDriftReach;

        // People
        public int totalPeopleSaved;
        public int maxPeopleSaved;

        // Combo
        public int totalDriftCombo;
        public int maxDriftCombo;

        // Obstacle
        public int totalObstacleTouch;
        public int maxObstacleTouch;

        // Other
        public int gamePlayed;
        public float gameTime;
    }
}

