using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    // Where to save the save game file
    public static string path = Path.Combine(Application.persistentDataPath, "rockfall.save");
    
    public static void SaveGame(int highScore) {
        
        BinaryFormatter formatter = new BinaryFormatter();
        Debug.Log(path);

        using (FileStream stream = new FileStream(path, FileMode.Create)) {
            SaveGameData data = new SaveGameData(highScore);
            formatter.Serialize(stream, data);
        }
    }

    public static SaveGameData LoadGame() {
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open)) {
                SaveGameData data = (SaveGameData)formatter.Deserialize(stream);
                return data;
            }
        } else {
            Debug.Log("No Save Game file!");
            return null;
        }
    }
}
