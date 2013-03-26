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
		currentLevelData = null;
		currentRoomData = null;
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
	
	#region file management
	
	const string fileDirectory = "SaveData/";
	const string fileName = "SaveFile";    // Edit this for different save files
	const string fileTag = ".sav";
	int currentFileNum = -1;
	
	private string getSavePath() {
		return fileDirectory + fileName + currentFileNum.ToString() + fileTag;
	}
 
	public bool DataExists(int saveFile) {
		bool exists = hasSaved() && 
			   File.Exists(fileDirectory + fileName + saveFile.ToString() + fileTag);
		return exists;
	}
	
	// Write data
	public void Save ()
	{
		if(currentFileNum >= 0) {
			string filePath = getSavePath();
			if(!hasSaved()) Directory.CreateDirectory(fileDirectory);
			Stream stream = File.Open(filePath, FileMode.Create);
			BinaryFormatter bformatter = new BinaryFormatter();
			bformatter.Binder = new VersionDeserializationBinder(); 
			bformatter.Serialize(stream, gameData);
			stream.Close();
		}
	}
 
	public void Load(int fileNum) {
		currentFileNum = fileNum;
		gameData = Load();
	}
	// Load from a file
	private GameData Load() 
	{
		GameData data = new GameData ();
		string filePath = getSavePath();
		if(DataExists(currentFileNum)) {
			Stream stream = File.Open(filePath, FileMode.Open);
			BinaryFormatter bformatter = new BinaryFormatter();
			bformatter.Binder = new VersionDeserializationBinder(); 
			data = (GameData)bformatter.Deserialize(stream);
			stream.Close();
		}
		if(data.levels == null) data = null;
		return data;
	}
	
	public void StartNewGame(int fileNum) {
		currentFileNum = fileNum;
		string filePath = getSavePath();
		if(DataExists(currentFileNum)) File.Delete(filePath);
		Load(fileNum);
	}
	
	//Returns whether player has save data
	public bool hasSaved() {
		return Directory.Exists(fileDirectory);
	}
	
	#endregion
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