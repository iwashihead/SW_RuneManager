using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using SW;

public class MonsterDataEditor : EditorWindow {
	public static MonsterMaster _data;
	public static int selected = -1;
	public static Vector2 scrollPos;
	public static bool isEditMode;

	private MonsterData copyInstance = null;

	// ウインドウの表示
	[MenuItem("Window/MonsterDataEditor")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(MonsterDataEditor), false, "MonsterDataEditor");
	}

	string DataPath
	{
		get { return "Assets/Resources/" + typeof(MonsterMaster).FullName + ".asset"; }
	}

	void OnGUI () {
		if (_data == null) {
			// ScriptableObjectの読み込み
			_data = AssetDatabase.LoadAssetAtPath(DataPath, typeof(MonsterMaster)) as MonsterMaster;
			if(_data == null)
			{
				_data = ScriptableObject.CreateInstance<MonsterMaster>() as MonsterMaster;
				AssetDatabase.CreateAsset(_data, DataPath);
				AssetDatabase.Refresh();
			}
		}

		EditorGUILayout.BeginHorizontal();

		#region 要素の追加処理 ================================================================================
		Color mainColor = GUI.color;
		GUI.color = Color.green;
		if (GUILayout.Button("+", GUILayout.Width(20))) {
			MonsterData addData = new MonsterData();
			MonsterData[] copy = new MonsterData[_data.mons.Count];
			Array.Copy(_data.mons.ToArray(), copy, _data.mons.Count);
			Array.Sort(copy, (MonsterData a, MonsterData b)=>{ return a.id - b.id; });
			int newId = (copy!=null && copy.Length>0) ? copy[0].id : 0;
			// 最小のモンスターIDを探す
			for (int i=0; i<copy.Length; i++)
			{
				if (newId < copy[i].id) {
					break;
				}
				else {
					newId++;
				}
			}
			addData.id = newId;

			// データの追加
			_data.mons.Insert(newId, addData);
		}
		GUI.color = mainColor;
		#endregion


		#region コピー処理 ================================================================================
		GUI.color = (copyInstance==null) ? Color.white : Color.green;
		if (GUILayout.Button("copy", GUILayout.Width(50))) {
			if (selected != -1)
			{
				copyInstance = (MonsterData)_data.mons[selected].Clone();
				Debug.Log("copy : " + copyInstance.id + "  " +  copyInstance.name);
			}
		}
		GUI.color = mainColor;
		#endregion


		#region ペースト処理 ================================================================================
		GUI.color = (copyInstance==null ? Color.white : Color.green);
		if (GUILayout.Button("paste", GUILayout.Width(50))) {
			if (selected != -1)
			{
				int pasteId = _data.mons[selected].id;
				copyInstance.id = pasteId;
				_data.mons[selected] = copyInstance;
				Debug.Log("paste : " + copyInstance.id + "  " +  copyInstance.name);
			}
		}
		GUI.color = mainColor;
		#endregion



		EditorGUILayout.EndHorizontal();


		#region 各要素の編集 ================================================================================
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		Color orgCol = GUI.backgroundColor;

		// 配置オブジェクトのリストを表示
		if (_data != null && _data.mons != null && _data.mons.Count > 0)
		{
			for(int i = 0; i < _data.mons.Count; i++) {
				orgCol = GUI.backgroundColor;

				GUILayout.BeginHorizontal();
				bool prev = i == selected;
				if (prev) GUI.backgroundColor = Color.magenta;
				bool flag = GUILayout.Toggle((i == selected), _data.mons[i].id.ToString() + " : " + _data.mons[i].j_name, "BoldLabel");

				GUI.backgroundColor = orgCol;
				if (GUILayout.Button("Edit",GUILayout.Width(60))) {
					isEditMode = !isEditMode;
					selected = i;// 選択中にする
				}
				GUI.backgroundColor = Color.red;
				if (GUILayout.Button("Remove", GUILayout.Width(60))) {
					// 削除確認のダイアログ
					bool result = EditorUtility.DisplayDialog(
						"Remove",
						"Remove this object.",
						"OK",
						"Cancel");

					if(result){
						_data.mons.RemoveAt(i);
						GUI.FocusControl("");
					}
				}
				GUI.backgroundColor = orgCol;
				EditorGUILayout.EndHorizontal();
				if (flag){
					selected = i;

					// 編集モード
					if (isEditMode){
						// TODO : 変数代入のエディタスクリプトを記述
						int id_new				= EditorGUILayout.IntField("\tid", _data.mons[i].id);
						if (id_new != _data.mons[i].id)
						{
							// すでにIDが使われていないかチェック
							if (_data.mons.Find(a=>a.id==id_new) == null)
								_data.mons[i].id = id_new;
						}

						_data.mons[i].name		= EditorGUILayout.TextField("\tname", _data.mons[i].name);
						_data.mons[i].j_name		= EditorGUILayout.TextField("\tj_name", _data.mons[i].j_name);
						_data.mons[i].race		= EditorGUILayout.TextField("\trace", _data.mons[i].race);
						_data.mons[i].rarity = EditorGUILayout.IntField("\trarity", _data.mons[i].rarity);
						_data.mons[i].element		= (Element)Enum.ToObject(typeof(Element), EditorGUILayout.EnumPopup("\telement", _data.mons[i].element));

						_data.mons[i].level = EditorGUILayout.IntField("\tlevel", _data.mons[i].level);
						_data.mons[i].b_hp = EditorGUILayout.IntField("\tb_hp", _data.mons[i].b_hp);
						_data.mons[i].b_atk = EditorGUILayout.IntField("\tb_atk", _data.mons[i].b_atk);
						_data.mons[i].b_def = EditorGUILayout.IntField("\tb_def", _data.mons[i].b_def);
						_data.mons[i].b_spd = EditorGUILayout.IntField("\tb_spd", _data.mons[i].b_spd);
						_data.mons[i].b_crate = EditorGUILayout.IntField("\tb_crate", _data.mons[i].b_crate);
						_data.mons[i].b_cdmg = EditorGUILayout.IntField("\tb_cdmg", _data.mons[i].b_cdmg);
						_data.mons[i].b_res = EditorGUILayout.IntField("\tb_res", _data.mons[i].b_res);
						_data.mons[i].b_acc = EditorGUILayout.IntField("\tb_acc", _data.mons[i].b_acc);
					}
				}
				else{
					// トグルを二度押ししたら非選択状態にする
					if (prev){
						selected = -1;
						isEditMode = false;
					}
				}
				GUI.color = orgCol;
			}
		}
		#endregion

		// スクロールバー終了
		GUILayout.EndScrollView();
		// Apply Data
		ScriptableObject scriptable = AssetDatabase.LoadAssetAtPath(DataPath, typeof(ScriptableObject)) as ScriptableObject;
		EditorUtility.SetDirty(scriptable);
	}
}