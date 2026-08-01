using UnityEngine;

public enum DifficultyLevel
{
    Easy = 0,
    Medium = 1,
    Hard = 2
}
public static class GameDifficulty
{
    //public static int difficultyLevel = 0; // 0 = Easy, 1 = Medium, 2 = Hard
    public static DifficultyLevel difficulty;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic()
    {
        //difficultyLevel = 1;
        difficulty = DifficultyLevel.Medium;
    }
}
