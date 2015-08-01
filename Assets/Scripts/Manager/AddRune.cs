using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SW;
using Kender.uGUI;

// ルーン追加ウインドウ関連処理
public partial class RuneManager : SingletonObject<RuneManager> {
	public Canvas addRuneCanvas;
	public Image addRuneImage;
	public Text addRuneNameText;
	public InputField addRuneFreeCommentInput;
	public ComboBox addRuneKindSelector;
	public ComboBox addRuneNoSelector;
	public ComboBox addRuneRaritySelector;
	public ComboBox addRuneLevelSelector;
	public InputField addRuneMainOPInput;
	public ComboBox addRuneMainOPSelector;
	public Text addRuneMainOPText;
	public InputField addRuneSubOPInput;
	public ComboBox addRuneSubOPSelector;
	public Text addRuneSubOPText;
	public InputField addRuneBonus1OPInput;
	public ComboBox addRuneBonus1OPSelector;
	public Text addRuneBonus1OPText;
	public InputField addRuneBonus2OPInput;
	public ComboBox addRuneBonus2OPSelector;
	public Text addRuneBonus2OPText;
	public InputField addRuneBonus3OPInput;
	public ComboBox addRuneBonus3OPSelector;
	public Text addRuneBonus3OPText;
	public InputField addRuneBonus4OPInput;
	public ComboBox addRuneBonus4OPSelector;
	public Text addRuneBonus4OPText;
	public Button addRuneOKButton;

	/// <summary>
	/// ルーン追加画面で、追加モードか更新モードか
	/// </summary>
	private bool isAddRune;

	private RuneData addRuneData;

	public List<Sprite> runeSprites;

	public Sprite GetRuneSprite(RuneType type)
	{
		if (runeSprites == null) { return null; }
		return runeSprites.Find(a=>a.name==type.ToString());
	}

	public string GetOptionString(RuneOption option)
	{
		// なしの場合は空文字を返す
		if (option.param == RuneParam.None) { return ""; }

		string ret = string.Format("{0}+{1}", option.param.ToJpnString(), option.value);

		// %を削除
		ret = ret.Replace("%","");

		// 末尾に%がつくものはつける
		if (option.param == RuneParam.Atk_Percent
			|| option.param == RuneParam.Cri
			|| option.param == RuneParam.CriDmg
			|| option.param == RuneParam.Def_Percent
			|| option.param == RuneParam.HP_Percent
			|| option.param == RuneParam.Reg
			|| option.param == RuneParam.Acc)
		{
			ret += "%";
		}

		return ret;
	}

	void AddRuneAwake()
	{
		// イベントリスナー登録
		addRuneKindSelector.OnSelectionChanged = OnKindChanged;
		addRuneNoSelector.OnSelectionChanged = OnNoChanged;
		addRuneRaritySelector.OnSelectionChanged = OnRarityChanged;
		addRuneLevelSelector.OnSelectionChanged = OnLevelChanged;
		addRuneMainOPInput.onEndEdit.AddListener(val=>{ OnOptionMainChanged(val); });
		addRuneMainOPSelector.OnSelectionChanged = OnMainParamChanged;
		addRuneSubOPInput.onEndEdit.AddListener(val=>{ OnOptionSubChanged(val); });
		addRuneSubOPSelector.OnSelectionChanged = OnSubParamChanged;
		addRuneBonus1OPInput.onEndEdit.AddListener(val=>{ OnBonus1OPChanged(val); });
		addRuneBonus1OPSelector.OnSelectionChanged = OnBonus1ParamChanged;
		addRuneBonus2OPInput.onEndEdit.AddListener(val=>{ OnBonus2OPChanged(val); });
		addRuneBonus2OPSelector.OnSelectionChanged = OnBonus2ParamChanged;
		addRuneBonus3OPInput.onEndEdit.AddListener(val=>{ OnBonus3OPChanged(val); });
		addRuneBonus3OPSelector.OnSelectionChanged = OnBonus3ParamChanged;
		addRuneBonus4OPInput.onEndEdit.AddListener(val=>{ OnBonus4OPChanged(val); });
		addRuneBonus4OPSelector.OnSelectionChanged = OnBonus4ParamChanged;
		addRuneFreeCommentInput.onEndEdit.AddListener(val=>{ OnFreeCommentChanged(val); });
	}

	public string GetRuneNameString(RuneData data)
	{
		return string.Format("{0}番 ★{1} {2}のルーン+{3}", data.no, data.rank, data.type.ToJpnString(), data.level);
	}

	/// <summary>
	/// ルーン追加キャンバスの初期化
	/// 新規ルーンを追加する場合はnullをセットしてください
	/// </summary>
	public void AddRuneInitialize(RuneData data)
	{
		addRuneCanvas.enabled = true;
		
		if (data == null) {
			// 新規ルーン
			data = new RuneData();
			isAddRune = true;

			// 名前
			addRuneNameText.text = GetRuneNameString(data);

			// イメージ
			addRuneImage.sprite = GetRuneSprite(data.type);

			// 種類
			addRuneKindSelector.SelectedIndex = (int)RuneType.Energy;

			// 番号
			addRuneNoSelector.SelectedIndex = 0;

			// レアリティ
			addRuneRaritySelector.SelectedIndex = 0;

			// 強化レベル
			addRuneLevelSelector.SelectedIndex = 0;

			// メインオプション
			addRuneMainOPInput.text = addRuneMainOPText.text = "";
			addRuneMainOPSelector.SelectedIndex = 0;

			// サブオプション
			addRuneSubOPInput.text = addRuneSubOPText.text = "";
			addRuneSubOPSelector.SelectedIndex = 0;

			// ボーナスオプション1
			addRuneBonus1OPInput.text = addRuneBonus1OPText.text = "";
			addRuneBonus1OPSelector.SelectedIndex = 0;

			// ボーナスオプション2
			addRuneBonus2OPInput.text = addRuneBonus2OPText.text = "";
			addRuneBonus2OPSelector.SelectedIndex = 0;

			// ボーナスオプション3
			addRuneBonus3OPInput.text = addRuneBonus3OPText.text = "";
			addRuneBonus3OPSelector.SelectedIndex = 0;

			// ボーナスオプション4
			addRuneBonus4OPInput.text = addRuneBonus4OPText.text = "";
			addRuneBonus4OPSelector.SelectedIndex = 0;

			// コメント欄
			addRuneFreeCommentInput.text = "";
		}
		else {
			// 既存ルーンの更新
			isAddRune = false;

			// 名前
			addRuneNameText.text = GetRuneNameString(data);

			// イメージ
			addRuneImage.sprite = GetRuneSprite(data.type);

			// 種類
			addRuneKindSelector.SelectedIndex = (int)data.type;

			// 番号
			addRuneNoSelector.SelectedIndex = data.no-1;

			// レアリティ
			addRuneRaritySelector.SelectedIndex = data.rank-1;

			// 強化レベル
			addRuneLevelSelector.SelectedIndex = data.level-1;

			// メインオプション
			addRuneMainOPText.text = GetOptionString(data.mainOption);
			addRuneMainOPInput.text = "" + data.mainOption.value;
			addRuneMainOPSelector.SelectedIndex = GetRuneParamNo(data.mainOption.param);

			// サブオプション
			addRuneSubOPText.text = GetOptionString(data.subOption);
			addRuneSubOPInput.text = "" + data.subOption.value;
			addRuneSubOPSelector.SelectedIndex = GetRuneParamNo(data.subOption.param);

			// ボーナスオプション1
			addRuneBonus1OPText.text = GetOptionString(data.bonusOption[0]);
			addRuneBonus1OPInput.text = "" + data.bonusOption[0].value;
			addRuneBonus1OPSelector.SelectedIndex = GetRuneParamNo(data.bonusOption[0].param);

			// ボーナスオプション2
			addRuneBonus2OPText.text = GetOptionString(data.bonusOption[1]);
			addRuneBonus2OPInput.text = "" + data.bonusOption[1].value;
			addRuneBonus2OPSelector.SelectedIndex = GetRuneParamNo(data.bonusOption[1].param);

			// ボーナスオプション3
			addRuneBonus3OPText.text = GetOptionString(data.bonusOption[2]);
			addRuneBonus3OPInput.text = "" + data.bonusOption[2].value;
			addRuneBonus3OPSelector.SelectedIndex = GetRuneParamNo(data.bonusOption[2].param);

			// ボーナスオプション4
			addRuneBonus4OPText.text = GetOptionString(data.bonusOption[3]);
			addRuneBonus4OPInput.text = "" + data.bonusOption[3].value;
			addRuneBonus4OPSelector.SelectedIndex = GetRuneParamNo(data.bonusOption[3].param);

			// コメント欄
			addRuneFreeCommentInput.text = data.owner==null ? "" : data.owner;// nullだとエラーになる
		}

		addRuneData = data;
	}

	public void AddRuneDisable()
	{
		addRuneCanvas.enabled = false;
	}

	public void OnAddRuneOK()
	{
		// TODO : 同一ルーンがないかチェック


		if (isAddRune)
		{
			// 新規追加
			if (addRuneData.key == -1)
				addRuneData.key = Data.GetRuneKey();
			Data.Instance.inv.runes.Add(addRuneData);
		}
		else
		{
			// 既存データの更新
		}

		// セーブ
		if (Data.Save())
		{
			string message = "ルーンデータの保存に失敗しました!";
			DialogCanvas.Create(message, Color.red, ()=>{
				// 何もしない
			});
		}
		else
		{
			string message = "";
			if (isAddRune) {
				message = "新規ルーンデータを保存しました!";
			} else {
				message = "ルーンデータを保存しました!";
			}
			DialogCanvas.Create(message, Color.black, ()=>{
				AddRuneInitialize(null);
				if (runeListCanvas.enabled) refreshFlag=true;
				if (runeSetCanvas.enabled) {
					RunePreview(currentRuneSet);
					RuneDetail(addRuneData);
					StatusUpdate();
				}
			});
		}
	}

	public int ValidateRuneOption(string value)
	{
		if (string.IsNullOrEmpty(value)) return 0;

		// スペースとか改行は除外
		value = value.Trim();
		value = value.Replace("\n", "");
		value = value.Replace(" ", "");
		value = value.Replace("　", "");

		// 全角数字を半角数字に置換する
		value = value.Replace("０", "0");
		value = value.Replace("１", "1");
		value = value.Replace("２", "2");
		value = value.Replace("３", "3");
		value = value.Replace("４", "4");
		value = value.Replace("５", "5");
		value = value.Replace("６", "6");
		value = value.Replace("７", "7");
		value = value.Replace("８", "8");
		value = value.Replace("９", "9");

		// %も全角を半角に置換する
		value = value.Replace("％", "%");

		// 数字を先に取り出す
		try {
			Regex re = new Regex(@"[^0-9]");
			string numValue = re.Replace(value, "");
			return System.Int32.Parse(numValue);
		}
		catch (System.Exception e){
			Debug.LogError(e.ToString());
			return 0;
		}
	}

	public void OnKindChanged(int value)
	{
		if (value < 0 || value >= System.Enum.GetValues(typeof(RuneType)).Length) {
			value = (int)RuneType.Energy;
			addRuneKindSelector.SelectedIndex = (int)RuneType.Energy;
		}
		addRuneData.type = (RuneType)value;

		// ルーン名更新
		string runeName = string.Format("{0}番 ★{1} {2}のルーン+{3}", addRuneData.no, addRuneData.rank, addRuneData.type.ToJpnString(), addRuneData.level);
		addRuneNameText.text = runeName;
		// ルーン画像更新
		addRuneImage.sprite = GetRuneSprite(addRuneData.type);
	}

	public void OnKindChanged(string value)
	{
		if (value.Contains("元気") || value.Contains("Energy") || value.Contains("genki"))
			value = "Energy";
		else if (value.Contains("猛攻") || value.Contains("Fatal")|| value.Contains("moukou"))
			value = "Fatal";
		else if (value.Contains("刃") || value.Contains("Blade")|| value.Contains("yaiba"))
			value = "Blade";
		else if (value.Contains("激怒") || value.Contains("Rage")|| value.Contains("gekido"))
			value = "Rage";
		else if (value.Contains("迅速") || value.Contains("Swift")|| value.Contains("jinsoku")|| value.Contains("jinnsoku"))
			value = "Swift";
		else if (value.Contains("集中") || value.Contains("Focus")|| value.Contains("syuutyuu")|| value.Contains("syutyu"))
			value = "Focus";
		else if (value.Contains("絶望") || value.Contains("Despair")|| value.Contains("zetubou"))
			value = "Despair";
		else if (value.Contains("守護") || value.Contains("Guard")|| value.Contains("syugo"))
			value = "Guard";
		else if (value.Contains("忍耐") || value.Contains("Endure")|| value.Contains("nintai")|| value.Contains("ninntai"))
			value = "Endure";
		else if (value.Contains("暴走") || value.Contains("Violent")|| value.Contains("bousou"))
			value = "Violent";
		else if (value.Contains("意思") || value.Contains("Will")|| value.Contains("ishi"))
			value = "Will";
		else if (value.Contains("果報") || value.Contains("Nemesis")|| value.Contains("kahou"))
			value = "Nemesis";
		else if (value.Contains("保護") || value.Contains("Shield")|| value.Contains("hogo"))
			value = "Shield";
		else if (value.Contains("反撃") || value.Contains("Revenge")|| value.Contains("hangeki"))
			value = "Revenge";
		else if (value.Contains("吸血") || value.Contains("Vampire")|| value.Contains("kyuuketu"))
			value = "Vampire";

		try {
			addRuneData.type = (RuneType)System.Enum.Parse(typeof(RuneType), value);
		} catch(System.Exception e) {
			Debug.LogError(e.ToString());
//			addRuneKindInput.text = addRuneData.type.ToJpnString();
		}
		// ルーン名更新
		string runeName = string.Format("{0}番 ★{1} {2}のルーン+{3}", addRuneData.no, addRuneData.rank, addRuneData.type.ToJpnString(), addRuneData.level);
		addRuneNameText.text = runeName;
		// ルーン画像更新
		addRuneImage.sprite = GetRuneSprite(addRuneData.type);
	}
		
	public int GetFlatValue(int rarity, int level, int no, RuneParam param)
	{
		if (no == 1 || no == 3)
		{
			switch (rarity)
			{
			case 1:
				return 0;
			case 2:
				return 0;
			case 3:
				switch (level)
				{
				case 1: return 12;
				case 2: return 17;
				case 3: return 22;
				case 4: return 27;
				case 5: return 32;
				case 6: return 37;
				case 7: return 42;
				case 8: return 47;
				case 9: return 52;
				case 10: return 57;
				case 11: return 62;
				case 12: return 67;
				case 13: return 72;
				case 14: return 77;
				case 15: return 92;
				default: return 12;
				}
			case 4:
				switch (level)
				{
				case 1: return 16;
				case 2: return 22;
				case 3: return 28;
				case 4: return 34;
				case 5: return 40;
				case 6: return 46;
				case 7: return 52;
				case 8: return 58;
				case 9: return 64;
				case 10: return 70;
				case 11: return 76;
				case 12: return 82;
				case 13: return 88;
				case 14: return 94;
				case 15: return 112;
				default: return 16;
				}
			case 5:
				switch (level)
				{
				case 1: return 22;
				case 2: return 29;
				case 3: return 36;
				case 4: return 43;
				case 5: return 50;
				case 6: return 57;
				case 7: return 64;
				case 8: return 71;
				case 9: return 78;
				case 10: return 85;
				case 11: return 92;
				case 12: return 99;
				case 13: return 106;
				case 14: return 113;
				case 15: return 135;
				default: return 22;
				}
			case 6:
				switch (level)
				{
				case 1: return 30;
				case 2: return 38;
				case 3: return 46;
				case 4: return 54;
				case 5: return 62;
				case 6: return 70;
				case 7: return 78;
				case 8: return 86;
				case 9: return 94;
				case 10: return 102;
				case 11: return 110;
				case 12: return 118;
				case 13: return 126;
				case 14: return 134;
				case 15: return 160;
				default: return 30;
				}
			default:
				return 0;
			}
		}
		else if (no == 5)// 体力
		{
			switch (rarity)
			{
			case 1:
				return 0;
			case 2:
				return 0;
			case 3:
				switch (level)
				{
				case 1: return 175;
				case 2: return 250;
				case 3: return 325;
				case 4: return 400;
				case 5: return 475;
				case 6: return 550;
				case 7: return 625;
				case 8: return 700;
				case 9: return 775;
				case 10: return 850;
				case 11: return 925;
				case 12: return 1000;
				case 13: return 1075;
				case 14: return 1150;
				case 15: return 1380;
				default: return 175;
				}
			case 4:
				switch (level)
				{
				case 1: return 250;
				case 2: return 340;
				case 3: return 430;
				case 4: return 520;
				case 5: return 610;
				case 6: return 700;
				case 7: return 790;
				case 8: return 880;
				case 9: return 970;
				case 10: return 1060;
				case 11: return 1150;
				case 12: return 1240;
				case 13: return 1330;
				case 14: return 1420;
				case 15: return 1704;
				default: return 250;
				}
			case 5:
				switch (level)
				{
				case 1: return 375;
				case 2: return 480;
				case 3: return 585;
				case 4: return 690;
				case 5: return 795;
				case 6: return 900;
				case 7: return 1005;
				case 8: return 1110;
				case 9: return 1215;
				case 10: return 1320;
				case 11: return 1425;
				case 12: return 1530;
				case 13: return 1635;
				case 14: return 1740;
				case 15: return 2088;
				default: return 375;
				}
			case 6:
				switch (level)
				{
				case 1: return 480;
				case 2: return 600;
				case 3: return 720;
				case 4: return 840;
				case 5: return 960;
				case 6: return 1080;
				case 7: return 1200;
				case 8: return 1320;
				case 9: return 1440;
				case 10: return 1560;
				case 11: return 1680;
				case 12: return 1800;
				case 13: return 1920;
				case 14: return 2040;
				case 15: return 2448;
				default: return 480;
				}
			default:
				return 0;
			}
		}
		else {
			
		}
		return 0;
	}

	public void UpdateMainFlat()
	{
		// 1, 3, 5番の場合はメインOP固定
		if (addRuneData.no == 1)
		{
			int val = GetFlatValue(addRuneData.rank, addRuneData.level, addRuneData.no, RuneParam.Atk_Flat);
			addRuneMainOPInput.text = "" + val;
			addRuneMainOPSelector.SelectedIndex = GetRuneParamNo(RuneParam.Atk_Flat);
			addRuneData.mainOption.param = RuneParam.Atk_Flat;
			addRuneData.mainOption.value = val;
			addRuneMainOPText.text = GetOptionString(addRuneData.mainOption);
		}
		else if (addRuneData.no == 3)
		{
			int val = GetFlatValue(addRuneData.rank, addRuneData.level, addRuneData.no, RuneParam.Def_Flat);
			addRuneMainOPInput.text = "" + val;
			addRuneMainOPSelector.SelectedIndex = GetRuneParamNo(RuneParam.Def_Flat);
			addRuneData.mainOption.param = RuneParam.Def_Flat;
			addRuneData.mainOption.value = val;
			addRuneMainOPText.text = GetOptionString(addRuneData.mainOption);
		}
		else if (addRuneData.no == 5)
		{
			int val = GetFlatValue(addRuneData.rank, addRuneData.level, addRuneData.no, RuneParam.HP_Flat);
			addRuneMainOPInput.text = "" + val;
			addRuneMainOPSelector.SelectedIndex = GetRuneParamNo(RuneParam.HP_Flat);
			addRuneData.mainOption.param = RuneParam.HP_Flat;
			addRuneData.mainOption.value = val;
			addRuneMainOPText.text = GetOptionString(addRuneData.mainOption);
		}
	}

	public void OnNoChanged(int val)
	{
		addRuneData.no = val+1;
		UpdateMainFlat();

		// ルーン名更新
		string runeName = string.Format("{0}番 ★{1} {2}のルーン+{3}", addRuneData.no, addRuneData.rank, addRuneData.type.ToJpnString(), addRuneData.level);
		addRuneNameText.text = runeName;
	}

	public void OnRarityChanged(int value)
	{
		addRuneData.rank = value+1;
		UpdateMainFlat();

		// ルーン名更新
		string runeName = string.Format("{0}番 ★{1} {2}のルーン+{3}", addRuneData.no, addRuneData.rank, addRuneData.type.ToJpnString(), addRuneData.level);
		addRuneNameText.text = runeName;
	}

	public void OnLevelChanged(int val)
	{
		addRuneData.level = val+1;
		UpdateMainFlat();

		// ルーン名更新
		string runeName = string.Format("{0}番 ★{1} {2}のルーン+{3}", addRuneData.no, addRuneData.rank, addRuneData.type.ToJpnString(), addRuneData.level);
		addRuneNameText.text = runeName;
	}

	public int GetRuneParamNo(RuneParam param)
	{
		switch (param)
		{
		case RuneParam.None: return 0;
		case RuneParam.HP_Percent: return 1;
		case RuneParam.HP_Flat: return 2;
		case RuneParam.Atk_Percent: return 3;
		case RuneParam.Atk_Flat: return 4;
		case RuneParam.Def_Percent: return 5;
		case RuneParam.Def_Flat: return 6;
		case RuneParam.Spd: return 7;
		case RuneParam.Cri: return 8;
		case RuneParam.CriDmg: return 9;
		case RuneParam.Reg: return 10;
		case RuneParam.Acc: return 11;
		}
		return 0;
	}

	public RuneParam GetRuneParam(int val)
	{
		switch (val)
		{
		case 0: return RuneParam.None;
		case 1: return RuneParam.HP_Percent;
		case 2: return RuneParam.HP_Flat;
		case 3: return RuneParam.Atk_Percent;
		case 4: return RuneParam.Atk_Flat;
		case 5: return RuneParam.Def_Percent;
		case 6: return RuneParam.Def_Flat;
		case 7: return RuneParam.Spd;
		case 8: return RuneParam.Cri;
		case 9: return RuneParam.CriDmg;
		case 10: return RuneParam.Reg;
		case 11: return RuneParam.Acc;
		}
		return RuneParam.HP_Percent;
	}

	public void OnMainParamChanged(int value)
	{
		addRuneData.mainOption.param = GetRuneParam(value);
		addRuneMainOPText.text = GetOptionString(addRuneData.mainOption);
	}

	public void OnOptionMainChanged(string value)
	{
		addRuneData.mainOption.value = ValidateRuneOption(value);
		addRuneMainOPText.text = GetOptionString(addRuneData.mainOption);
	}

	public void OnSubParamChanged(int value)
	{
		addRuneData.subOption.param = GetRuneParam(value);
		addRuneSubOPText.text = GetOptionString(addRuneData.subOption);
	}

	public void OnOptionSubChanged(string value)
	{
		addRuneData.subOption.value = ValidateRuneOption(value);
		addRuneSubOPText.text = GetOptionString(addRuneData.subOption);
	}

	public void OnBonus1ParamChanged(int value)
	{
		addRuneData.bonusOption[0].param = GetRuneParam(value);
		addRuneBonus1OPText.text = GetOptionString(addRuneData.bonusOption[0]);
	}

	public void OnBonus1OPChanged(string value)
	{
		addRuneData.bonusOption[0].value = ValidateRuneOption(value);
		addRuneBonus1OPText.text = GetOptionString(addRuneData.bonusOption[0]);
	}

	public void OnBonus2ParamChanged(int value)
	{
		addRuneData.bonusOption[1].param = GetRuneParam(value);
		addRuneBonus2OPText.text = GetOptionString(addRuneData.bonusOption[1]);
	}

	public void OnBonus2OPChanged(string value)
	{
		addRuneData.bonusOption[1].value = ValidateRuneOption(value);
		addRuneBonus2OPText.text = GetOptionString(addRuneData.bonusOption[1]);
	}

	public void OnBonus3ParamChanged(int value)
	{
		addRuneData.bonusOption[2].param = GetRuneParam(value);
		addRuneBonus3OPText.text = GetOptionString(addRuneData.bonusOption[2]);
	}

	public void OnBonus3OPChanged(string value)
	{
		addRuneData.bonusOption[2].value = ValidateRuneOption(value);
		addRuneBonus3OPText.text = GetOptionString(addRuneData.bonusOption[2]);
	}

	public void OnBonus4ParamChanged(int value)
	{
		addRuneData.bonusOption[3].param = GetRuneParam(value);
		addRuneBonus4OPText.text = GetOptionString(addRuneData.bonusOption[3]);
	}

	public void OnBonus4OPChanged(string value)
	{
		addRuneData.bonusOption[3].value = ValidateRuneOption(value);
		addRuneBonus4OPText.text = GetOptionString(addRuneData.bonusOption[3]);
	}

	public void OnFreeCommentChanged(string value)
	{
		value = value.Trim();
		value = value.Replace("\n","");

		addRuneData.owner = value;
	}

	public void OnAddRuneClose()
	{
		AddRuneDisable();
	}
}
