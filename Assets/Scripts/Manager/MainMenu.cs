using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using JsonFx.Json;
using SW;

public partial class RuneManager : SingletonObject<RuneManager> {
	public Button runeListButton;
	public Button addRuneButton;
	public Button setupButton;

	public void OnRuneList()
	{
		// ルーンリストの表示
		RuneListInitialize();
	}

	public void OnAddRune()
	{
		// 新規追加
		AddRuneInitialize(null);
	}

	public void OnSetup()
	{

	}

	public void OnImport()
	{
		// インポートウインドウを開く
		ValueChangeDialog.CreateBigDialog("データインポート\n（※編集中のデータは失われます エクスポートからバックアップを取ることできます）", null, (value)=>{

			// 空なら何もしない
			if (string.IsNullOrEmpty(value)) return;

			// データを読み込む
			if (Data.Load( value )) {
				// 失敗
				DialogCanvas.Create("データの読み込みに失敗しました!", Color.red, ()=>{  });
			} else {
				// 成功
				DialogCanvas.Create("データの読み込みに成功しました!", Color.black, ()=>{
					if (runeListCanvas.enabled) { refreshFlag=true; }
					Data.Save();
				});
			}
		}, (value) => {
			// Inputフィールドに書き込みをした

			// TODO add validate code
			// いまのところ何もしない
		});
	}

	public void OnExport()
	{
		// エクスポートウインドウを開く
		string exportData = JsonFx.Json.JsonWriter.Serialize(Data.Instance.inv);
		ValueChangeDialog.CreateBigDialog("データエクスポート\n編集中のデータを出力しました", exportData, (value)=>{

			// 何もしない

		}, (value) => {

			// 何もしない

		});
	}
}
