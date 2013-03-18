using UnityEngine;    // For Debug.Log, etc.
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Runtime.Serialization;
using System.Reflection;
 
public class DataManager {
	int levelNum;
	int roomNum;
	GameData gameData;
	LevelData currentLevelData;
	RoomData currentRoomData;
	
	public DataManager() {
		gameData = Load();
	}
	
	#region random access getters and setters
	
	public LevelData GetLevelData(int level) {
		return gameData.levels[level];
	}
	
	public bool isLevelUnlocked(int level) {
		bool isUnlocked = true;
		/*
		 * level 0 = hub
		 * level 1 is automatically unlocked
		 */
		if(level >= gameData.levels.Length || 
			(level > 1 && GetLevelData(level) == null))
			isUnlocked = false;
		return isUnlocked;
	}
	
	#endregion
	
	#region pointer getters and setters 
	
	public GameData GetGameData() {return gameData;}	
	public void SetGameData(GameData newData) {
		gameData = newData;
	}
	
	public LevelData GetLevelData() {return currentLevelData;}
	public void SetLevelData(LevelData newData) {
		currentLevelData = newData;
		gameData.levels[levelNum] = currentLevelData;
	}

	public RoomData GetRoomData() {return currentRoomData;}
	public void SetRoomData(RoomData newData) {
		if(currentRoomData != null) {
			
			//take away old death count
			currentLevelData.deaths -= currentRoomData.deaths;
			gameData.deaths -= currentRoomData.deaths;
		}
		currentRoomData = newData;
		gameData.levels[levelNum].rooms[roomNum] = currentRoomData;
		
		//update death count
		currentLevelData.deaths += currentRoomData.deaths;
		gameData.deaths += currentRoomData.deaths;
	}
	
	#endregion
	
	#region change pointers
	public void ChangeLevel(int newLevel) {
		if(newLevel == 0 && !isLevelUnlocked(levelNum + 1) && 
			levelNum + 1 < gameData.levels.Length) {
			
			gameData.levels[levelNum + 1] = new LevelData();	
		}
		
		levelNum = newLevel;
		currentLevelData = gameData.levels[levelNum];
	}
	
	public void ChangeRoom(int newRoom) {
		roomNum = newRoom;
		currentRoomData = currentLevelData.rooms[roomNum];
		Game.instance.dataManager.Save();
	}
	#endregion
	
	public string currentFilePath = "SaveData.sav";    // Edit this for different save files
 
	// Write data
	public void Save () {Save (currentFilePath);}
	public void Save (string filePath)
	{
		Stream stream = File.Open(filePath, FileMode.Create);
		BinaryFormatter bformatter = new BinaryFormatter();
		bformatter.Binder = new VersionDeserializationBinder(); 
		bformatter.Serialize(stream, gameData);
		stream.Close();
	}
 
	// Load from a file
	public GameData Load()  { return Load(currentFilePath); }
	public GameData Load(string filePath) 
	{
		GameData data = new GameData ();
		if(File.Exists(filePath)) {
			Stream stream = File.Open(filePath, FileMode.Open);
			BinaryFormatter bformatter = new BinaryFormatter();
			bformatter.Binder = new VersionDeserializationBinder(); 
			data = (GameData)bformatter.Deserialize(stream);
			stream.Close();
		}
		if(data.levels == null) data = null;
		return data;
	}
}
 
// === This is required to guarantee a fixed serialization assembly name, which Unity likes to randomize on each compile
// Do not change this
public sealed class VersionDeserializationBinder : SerializationBinder 
{ 
    public override Type BindToType( string assemblyName, string typeName )
    { 
        if ( !string.IsNullOrEmpty( assemblyName ) && !string.IsNullOrEmpty( typeName ) ) 
        { 
            Type typeToDeserialize = null; 
            assemblyName = Assembly.GetExecutingAssembly().FullName; 
 
            // The following line of code returns the type. 
            typeToDeserialize = Type.GetType( String.Format( "{0}, {1}", typeName, assemblyName ) ); 
            return typeToDeserialize; 
        } 
        return null; 
    } 
}