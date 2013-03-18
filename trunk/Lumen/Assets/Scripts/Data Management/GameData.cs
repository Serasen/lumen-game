using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization;


[Serializable ()]
public class GameData : ISerializable {
 
	public LevelData[] levels;
	public int deaths;
	
 
	public GameData () {
		deaths = 0;
	}
	public GameData(int numLevels) : this() {
		levels = new LevelData[numLevels];
	}
 
	// Called automatically by ISerializable
	public GameData (SerializationInfo info, StreamingContext ctxt) : this()
	{
    // Get values from info and assign them to appropriate properties.
    	levels = (LevelData[])info.GetValue("levels", typeof(LevelData[]));
    	deaths = (int)info.GetValue("deaths", typeof(int));
	}
 
	// Called automatically
	public void GetObjectData (SerializationInfo info, StreamingContext ctxt)
	{
    	// Repeat for each var defined in Values section
    	info.AddValue("levels", levels);
    	info.AddValue("deaths", deaths);
	}
}

[Serializable ()]
public class LevelData : ISerializable {
	public RoomData[] rooms;
	public bool unlocked;
	public int deaths;
	
	//Call default constructor to indicate unlocked status
	public LevelData () {
		deaths = 0;
	}
	public LevelData(int numRooms) : this() {
		rooms = new RoomData[numRooms];	
	}
	public LevelData (SerializationInfo info, StreamingContext ctxt)
	{
		rooms = (RoomData[])info.GetValue("rooms", typeof(RoomData[]));
		deaths = (int)info.GetValue("deaths", typeof(int));
	}
	
	public int GetNumReachedRooms() {
		int reachedRooms = 0;
		foreach(RoomData room in rooms) {
			if(room != null) reachedRooms++;
		}
		return reachedRooms;
	}
	
	public int getNumReachedKeyholes() {
		int reachedKeyholes = 0;
		foreach(RoomData room in rooms) {
			if(room != null) {
				reachedKeyholes += room.GetNumReachedKeyholes();
			}
		}
		return reachedKeyholes;
	}
	
	public int getNumKeyholes() {
		int keyholes = 0;
		foreach(RoomData room in rooms) {
			if(room != null) {
				keyholes += room.keyholes.Length;
			}
		}
		return keyholes;
		
	}
 
	public void GetObjectData (SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("rooms", rooms);
		info.AddValue("deaths", deaths);
	}	
}

[Serializable ()]
public class RoomData : ISerializable {
	public bool[] keyholes; //marks whether they have been traversed
	public int deaths;

	public RoomData () {
		deaths = 0;
	}
	public RoomData(int numKeyholes) : this() {
		keyholes = new bool[numKeyholes];
	}
	public RoomData(RoomData toCopy) {
		keyholes = new bool[toCopy.keyholes.Length];
		for(int i = 0; i < toCopy.keyholes.Length; i++) {
			keyholes[i] = toCopy.keyholes[i];	
		}
		deaths = toCopy.deaths;
	}
	public RoomData (SerializationInfo info, StreamingContext ctxt)
	{
		keyholes = (bool[])info.GetValue("keyholes", typeof(bool[]));
		deaths = (int)info.GetValue("deaths", typeof(int));
	}
	
	public int GetNumReachedKeyholes() {
		int reached = 0;
		foreach(bool keyholeReached in keyholes) {
			if(keyholeReached)	reached++;
		}
		return reached;
	}
 
	public void GetObjectData (SerializationInfo info, StreamingContext ctxt)
	{
    	info.AddValue("keyholes", keyholes);
		info.AddValue("deaths", deaths);
	}	
}
