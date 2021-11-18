using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {

    ///<summary>
    /// Saves the specified Grid in binary format, JSON = yucky cringe
    ///</summary>
    ///<param name='grid'> The Grid being saved </param>
    public static void SaveGrid(GridMap grid){
        Debug.Log("Saving Started");
        

        string filePath = Application.persistentDataPath + "/grid.BEVI";
        

        //FileStream stream = new FileStream(filePath, FileMode.Create);

        GridData gridData = new GridData(grid);
        string saveString = gridData.SaveToString();
        
        using (StreamWriter writer = File.CreateText(filePath)){

            writer.WriteLine(saveString);
        }
        if(File.Exists(filePath)){
            Debug.Log("IT WORKED");
        }
        else{
            Debug.Log("F-WORD");
        }
        
        

    }

    public static GD_OVERSEER LoadGrid(){

        string filePath = Application.persistentDataPath + "/grid.BEVI";

        if(File.Exists(filePath)){
            //BinaryFormatter formatter = new BinaryFormatter();
            string s = File.ReadAllText(filePath);

            //FileStream stream = new FileStream(filePath, FileMode.Open);
            //StreamReader reader = new StreamReader(filePath);

            GD_OVERSEER gridData = JsonUtility.FromJson<GD_OVERSEER>(s);
            //stream.Close();
            Debug.Log(gridData.height);
            Debug.Log("Loaded");

            return gridData;

        }
        else{

            Debug.LogError("Save file not found in " + filePath);
            return null;
        }
        
    }
    public static void LoadInFile(string s){
        string[] ohgod = s.Split('}');
        Debug.Log(ohgod[0]);
    }

    
}
