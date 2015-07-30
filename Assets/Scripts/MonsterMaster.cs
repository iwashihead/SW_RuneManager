using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using SW;

public class MonData {
	public List<MonsterData> mons;
}

public class MonsterMaster : ScriptableObject {
	public List<MonsterData> mons;
	string filePath = "MonsterData";

	public MonsterMaster()
	{
		if (mons == null) { mons = new List<MonsterData>(); }

		// 読み込みが必要になったら呼び出す
//		Load();
	}

	[ContextMenu("LoadData")]
	public void Load()
	{
		if (filePath != null) {
			TextAsset data = Resources.Load<TextAsset>(filePath);
			if (data != null) {
				Debug.Log("data :" + data.text + "  bytes");
				try{ 
					MonData master = JsonFx.Json.JsonReader.Deserialize<MonData>(data.text);
					if (master != null) {
						mons = master.mons;
					}
				}catch (System.Exception e)
				{
					Debug.LogError(e.ToString());
				}
			}else {
				Debug.Log("data not found");
			}
		}
	}

	void OnGUI()
	{
		// 編集可能に
		hideFlags = HideFlags.None;
	}
}
