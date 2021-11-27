using UnityEngine;

[System.Serializable]
public class SaveGameData
{
    public int highScore;

    public SaveGameData(int highScore) {
        this.highScore = highScore;
    }
}
