using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SW;

public class Data : SingletonObject<Data> {
	/// <summary>
	/// 所持ルーン一覧
	/// </summary>
	public RuneInventory inv;

	public static string DATA_KEY = "runeData";

	public void Awake()
	{
		// データ読み込み
		Load();

		// 読み込みに失敗しても動くようにする
		if (inv == null) {
			Debug.Log("Data Null");
			inv = new RuneInventory();
		}
		if (inv.runes == null) {
			Debug.Log("Runes Null");
			inv.runes = new List<RuneData>();
		}
		if (inv.runeSets == null) {
			Debug.Log("RuneSets Null");
			inv.runeSets = new List<RuneSet>();
		}
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
	/// 所持者をキーにルーンセットを取得する、ない場合はnullが返る
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

			return true;
		}

		return false;
	}
}
