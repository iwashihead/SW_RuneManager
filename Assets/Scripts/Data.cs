using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SW;

/// <summary>
/// データの管理クラス
/// セーブとロードを行う、データの保存先はPlayerPrefsを使用している
/// 形式はJson、プラグインとしてJsonFxを使用
/// </summary>
public class Data : SingletonObject<Data> {
	/// <summary>
	/// 所持ルーン一覧
	/// </summary>
	public RuneInventory inv;
	public MonsterMaster mon;

	public static string DATA_KEY = "runeData";

	public void Awake()
	{
		// データ読み込み
		Load();
	}

	/// <summary>
	/// ルーンの取得、ない場合はnullが返る
	/// </summary>
	public static RuneData GetRune(int index)
	{
		// インデックスが範囲外
		if (Data.Instance.inv == null
			|| Data.Instance.inv.runes == null
			|| index < 0
			|| index >= Data.Instance.inv.runes.Count)
		{
			return null;
		}

		return Data.Instance.inv.runes[index];
	}

	/// <summary>
	/// 所持者(タグ)をキーにルーンセットを取得する、ない場合はnullが返る
	/// </summary>
	public static RuneSet GetRuneSet(string owner)
	{
		if (Data.Instance.inv == null
			|| Data.Instance.inv.runeSets == null
			|| string.IsNullOrEmpty(owner))
		{
			return null;
		}

		for (int i=0; i<Data.Instance.inv.runeSets.Count; i++)
		{
			if (owner == Data.Instance.inv.runeSets[i].owner)
			{
				return Data.Instance.inv.runeSets[i];
			}
		}

		return null;
	}

	/// <summary>
	/// データを保存、失敗した場合はtrue, 正常の場合はfalseを返す
	/// </summary>
	public static bool Save()
	{
		// データセーブ
		try
		{
			string json = JsonFx.Json.JsonWriter.Serialize(Data.Instance.inv);
			Debug.Log("Data Save : \n" + json);
			PlayerPrefs.SetString(DATA_KEY, json);
		}
		catch (Exception e)
		{
			Debug.LogError(e.ToString());
			return true;// 異常あり
		}

		return false;
	}

	/// <summary>
	/// データの読み込み、失敗した場合はtrue, 正常の場合はfalseを返す
	/// </summary>
	public static bool Load()
	{
		// データの初期化
		string json = PlayerPrefs.GetString(DATA_KEY);

		return Load(json);
	}

	/// <summary>
	/// データの読み込み、失敗した場合はtrue, 正常の場合はfalseを返す
	/// </summary>
	public static bool Load(string json)
	{
		// データの初期化
		if (string.IsNullOrEmpty(json) == false) {
			Debug.Log("### data json ###\n" + json);
		}

		// データの復元
		try {
			Data.Instance.inv = string.IsNullOrEmpty(json) ? null : JsonFx.Json.JsonReader.Deserialize<RuneInventory>(json);
		} catch (Exception e)
		{
			Debug.LogError(e.ToString());

			if (Instance.inv == null) Instance.inv = new RuneInventory();
			if (Instance.inv.runes == null) Instance.inv.runes = new List<RuneData>();
			if (Instance.inv.runeSets == null) Instance.inv.runeSets = new List<RuneSet>();

			return true;
		}

		if (Instance.inv == null) Instance.inv = new RuneInventory();
		if (Instance.inv.runes == null) Instance.inv.runes = new List<RuneData>();
		if (Instance.inv.runeSets == null) Instance.inv.runeSets = new List<RuneSet>();

		// キーの割り当て
		AssignKey();

		return false;
	}

	public static int GetRuneKey()
	{
		int key = UnityEngine.Random.Range(0, int.MaxValue);
		if (Instance.inv.runes != null)
		{
			while ( Instance.inv.runes.Find(a=>a.key==key) != null )
			{
				key = UnityEngine.Random.Range(0, int.MaxValue);
			}
		}
		return key;
	}

	public static int GetRuneSetKey()
	{
		int key = UnityEngine.Random.Range(0, int.MaxValue);
		if (Instance.inv.runeSets != null)
		{
			while ( Instance.inv.runeSets.Find(a=>a.key==key) != null )
			{
				key = UnityEngine.Random.Range(0, int.MaxValue);
			}
		}
		return key;
	}

	// keyを割り当てる
	public static void AssignKey()
	{
		bool hasChange = false;
		Data.Instance.inv.runes.ForEach(a=>{
			if(a.key == -1) {
				hasChange=true;
				a.key = GetRuneKey();
			}
		});
		Data.Instance.inv.runeSets.ForEach(a=>{
			if(a.key == -1) {
				hasChange=true;
				a.key = GetRuneSetKey();
			}
		});

		if (hasChange)
		{
			// 値が変わっていたらセーブする
			Data.Save();
		}
	}
}
