using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    int score;
    int maxCombo;
    int peopleAmount;
    int bestScore;

    [Header("RSO")]
    [SerializeField] RSO_IntValue rsoScore;
    [SerializeField] RSO_IntValue rsoMaxCombo;
    [SerializeField] RSO_IntValue rsoPeopleAmount;
    [SerializeField] RSO_IntValue rsoBestScore;

    void GetValues()
    {
        score = rsoScore.Value;
        maxCombo = rsoMaxCombo.Value;
        peopleAmount = rsoPeopleAmount.Value;
        bestScore = rsoBestScore.Value;
    }
}