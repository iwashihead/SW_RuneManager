using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SW;

/// <summary>
/// ルーン一個分の情報を表示するコンテナ
/// これをまとめてリスト表示する
/// </summary>
public class RuneItem : MonoBehaviour {

	public RuneData data;

	public Image tagImage;
	public Image iconImage;
	public Text runeName;
	public Image mainPanel;
	public Text mainText;
	public Image subPanel;
	public Text subText;
	public Image bonus1Panel;
	public Text bonus1Text;
	public Image bonus2Panel;
	public Text bonus2Text;
	public Image bonus3Panel;
	public Text bonus3Text;
	public Image bonus4Panel;
	public Text bonus4Text;
	public Text valueText;

	public Button EditButton;
	public Button DeleteButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnEdit()
	{
		RuneManager.Instance.AddRuneInitialize(data);
	}

	public void OnDelete()
	{
		// ダイアログでチェックする
		if (Data.Instance.inv.dontCheckDelete == false)
		{
			OKCancelDialog.CreateWithToggle("ルーンを削除します。", Color.black, (flag)=>{

				// このメッセージを表示しないフラグの受け取り
				Data.Instance.inv.dontCheckDelete = flag;

				// このデータを削除する
				Data.Instance.inv.runes.Remove(data);
				Data.Save();

				// 実態を削除する
				RuneManager.Instance.runeItems.Remove(this);
				Destroy(this.gameObject);

				// 画面をリフレッシュ
				RuneManager.Instance.refreshFlag=true;
			}, (flag)=>{});
		} else {
			// このデータを削除する
			Data.Instance.inv.runes.Remove(data);
			Data.Save();

			// 実態を削除する
			RuneManager.Instance.runeItems.Remove(this);
			Destroy(this.gameObject);

			// 画面をリフレッシュ
			RuneManager.Instance.refreshFlag=true;
		}
	}

	public void OnDecide()
	{
		
	}

	public void Setup( RuneData srcData )
	{
		this.data = srcData;

		// タグ
		tagImage.enabled = !string.IsNullOrEmpty(data.owner);

		// 画像
		iconImage.sprite = RuneManager.Instance.GetRuneSprite(data.type);

		// ルーンの名前
		runeName.text = RuneManager.Instance.GetRuneNameString(data);

		// メインオプション
		string s = RuneManager.Instance.GetOptionString(data.mainOption);
		mainText.text = (s=="")? "---" : s;
		mainPanel.color = GetParamColor(data.mainOption.param);

		// サブオプション
		s = RuneManager.Instance.GetOptionString(data.subOption);
		subText.text = (s=="")? "---" : s;
		subPanel.color = GetParamColor(data.subOption.param);

		// ボーナスオプションを集めて
		List<RuneOption> bonusOP = new List<RuneOption>(data.bonusOption);
		// 種類でソートする
		bonusOP.Sort( (a,b)=>{
			if (RuneManager.Instance.sortParam != RuneParam.None) {
				if (b.param == RuneManager.Instance.sortParam) return 1;
				if (a.param == RuneManager.Instance.sortParam) return -1;
			}
			return (int)a.param - (int)b.param;
		});

		// ボーナス1
		s = RuneManager.Instance.GetOptionString(bonusOP[0]);
		bonus1Text.text = (s=="")? "---" : s;
		bonus1Panel.color = GetParamColor(bonusOP[0].param);

		// ボーナス2
		s = RuneManager.Instance.GetOptionString(bonusOP[1]);
		bonus2Text.text = (s=="")? "---" : s;
		bonus2Panel.color = GetParamColor(bonusOP[1].param);

		// ボーナス3
		s = RuneManager.Instance.GetOptionString(bonusOP[2]);
		bonus3Text.text = (s=="")? "---" : s;
		bonus3Panel.color = GetParamColor(bonusOP[2].param);

		// ボーナス4
		s = RuneManager.Instance.GetOptionString(bonusOP[3]);
		bonus4Text.text = (s=="")? "---" : s;
		bonus4Panel.color = GetParamColor(bonusOP[3].param);

		valueText.text = "" + (data.TotalValue).ToString() + " Pt";
	}

	public Color GetParamColor(RuneParam param)
	{
		Color col = Color.white;
		switch (param)
		{
		case RuneParam.None: col = Color.white; break;
		case RuneParam.HP_Percent: col = "#FFFF00".toColor(); break;// 黄
		case RuneParam.Atk_Percent: col = "#FF0000".toColor(); break;// 赤
		case RuneParam.Def_Percent: col = "#0080FF".toColor(); break;// 青
		case RuneParam.Spd: col = "#00FF00".toColor(); break;// 緑
		case RuneParam.Cri: col = "#FF8000".toColor(); break;//オレンジ
		case RuneParam.CriDmg: col = "#FF00FF".toColor(); break;//マゼンタ
		case RuneParam.Reg: col = "#00FFFF".toColor(); break;//シアン
		case RuneParam.Acc: col = "#9A2EFE".toColor(); break;//紫
		case RuneParam.HP_Flat: col = "#F3F781".toColor(); break;
		case RuneParam.Atk_Flat: col = "#F78181".toColor(); break;
		case RuneParam.Def_Flat: col = "#A9A9F5".toColor(); break;
		}
		col.a = 0.6f;

		return col;
	}
}
