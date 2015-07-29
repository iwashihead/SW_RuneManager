using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SW;

// ルーン追加ウインドウ関連処理
public partial class RuneManager : SingletonObject<RuneManager> {
	public Canvas addRuneCanvas;
	public Image addRuneImage;
	public Text addRuneNameText;
	public InputField addRuneFreeCommentInput;
	public InputField addRuneKindInput;
	public InputField addRuneNoInput;
	public InputField addRuneRarityInput;
	public InputField addRuneLevelInput;
	public InputField addRuneMainOPInput;
	public Text addRuneMainOPText;
	public InputField addRuneSubOPInput;
	public Text addRuneSubOPText;
	public InputField addRuneBonus1OPInput;
	public Text addRuneBonus1OPText;
	public InputField addRuneBonus2OPInput;
	public Text addRuneBonus2OPText;
	public InputField addRuneBonus3OPInput;
	public Text addRuneBonus3OPText;
	public InputField addRuneBonus4OPInput;
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

		string ret = string.Format("{0}+{1}", option.param.ToJapaneseString(), option.value);

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
		addRuneKindInput.onEndEdit.AddListener(val=>{ OnKindChanged(val); });
		addRuneNoInput.onEndEdit.AddListener(val=>{ OnNoChanged(val); });
		addRuneRarityInput.onEndEdit.AddListener(val=>{ OnRarityChanged(val); });
		addRuneLevelInput.onEndEdit.AddListener(val=>{ OnLevelChanged(val); });
		addRuneMainOPInput.onEndEdit.AddListener(val=>{ OnOptionMainChanged(val); });
		addRuneSubOPInput.onEndEdit.AddListener(val=>{ OnOptionSubChanged(val); });
		addRuneBonus1OPInput.onEndEdit.AddListener(val=>{ OnBonus1OPChanged(val); });
		addRuneBonus2OPInput.onEndEdit.AddListener(val=>{ OnBonus2OPChanged(val); });
		addRuneBonus3OPInput.onEndEdit.AddListener(val=>{ OnBonus3OPChanged(val); });
		addRuneBonus4OPInput.onEndEdit.AddListener(val=>{ OnBonus4OPChanged(val); });
		addRuneFreeCommentInput.onEndEdit.AddListener(val=>{ OnFreeCommentChanged(val); });
	}

	public string GetRuneNameString(RuneData data)
	{
		return string.Format("{0}番 ★{1} {2}のルーン+{3}", data.no, data.rank, data.type.ToJapaneseString(), data.level);
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
			addRuneKindInput.text = "";

			// 番号
			addRuneNoInput.text = "";

			// レアリティ
			addRuneRarityInput.text = "";

			// 強化レベル
			addRuneLevelInput.text = "";

			// メインオプション
			addRuneMainOPInput.text = addRuneMainOPText.text = "";

			// サブオプション
			addRuneSubOPInput.text = addRuneSubOPText.text = "";

			// ボーナスオプション1
			addRuneBonus1OPInput.text = addRuneBonus1OPText.text = "";

			// ボーナスオプション2
			addRuneBonus2OPInput.text = addRuneBonus2OPText.text = "";

			// ボーナスオプション3
			addRuneBonus3OPInput.text = addRuneBonus3OPText.text = "";

			// ボーナスオプション4
			addRuneBonus4OPInput.text = addRuneBonus4OPText.text = "";

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
			addRuneKindInput.text = data.type.ToJapaneseString();

			// 番号
			addRuneNoInput.text = data.no.ToString();

			// レアリティ
			addRuneRarityInput.text = data.rank.ToString();

			// 強化レベル
			addRuneLevelInput.text = data.level.ToString();

			// メインオプション
			addRuneMainOPInput.text = addRuneMainOPText.text = GetOptionString(data.mainOption);

			// サブオプション
			addRuneSubOPInput.text = addRuneSubOPText.text = GetOptionString(data.subOption);

			// ボーナスオプション1
			addRuneBonus1OPInput.text = addRuneBonus1OPText.text = GetOptionString(data.bonusOption[0]);

			// ボーナスオプション2
			addRuneBonus2OPInput.text = addRuneBonus2OPText.text = GetOptionString(data.bonusOption[1]);

			// ボーナスオプション3
			addRuneBonus3OPInput.text = addRuneBonus3OPText.text = GetOptionString(data.bonusOption[2]);

			// ボーナスオプション4
			addRuneBonus4OPInput.text = addRuneBonus4OPText.text = GetOptionString(data.bonusOption[3]);

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
		// 同一ルーンがないかチェック



		if (isAddRune)
		{
			// 新規追加
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
				if (runeListCanvas.enabled) Refresh();
			});
		}
	}

	public RuneOption ValidateRuneOption(string value)
	{
		RuneOption op = new RuneOption();

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
			op.value = System.Int32.Parse(numValue);
		}
		catch (System.Exception e){
			Debug.LogError(e.ToString());
		}
		Debug.Log("number : "+ op.value);

		// %付き?
		bool hasPercent = value.Contains("%");

		// パラメタ種類を取り出す
		if (value.Contains("攻撃") || value.Contains("ATK") || value.Contains("atk")|| value.Contains("kougeki"))
			op.param = hasPercent ? RuneParam.Atk_Percent : RuneParam.Atk_Flat;
		else if (value.Contains("防御") || value.Contains("DEF") || value.Contains("def")|| value.Contains("bougyo"))
			op.param = hasPercent ? RuneParam.Def_Percent : RuneParam.Def_Flat;
		else if (value.Contains("体力") || value.Contains("HP") || value.Contains("hp")|| value.Contains("tairyoku"))
			op.param = hasPercent ? RuneParam.HP_Percent : RuneParam.HP_Flat;
		else if (value.Contains("クリ率") || value.Contains("CRI Rate") || value.Contains("cri rate")|| value.Contains("kuriritu"))
			op.param = RuneParam.Cri;
		else if (value.Contains("クリダメ") || value.Contains("CRI Dmg") || value.Contains("cri dmg")|| value.Contains("kuridame"))
			op.param = RuneParam.CriDmg;
		else if (value.Contains("的中") || value.Contains("ACC") || value.Contains("acc")|| value.Contains("tekityuu"))
			op.param = RuneParam.Acc;
		else if (value.Contains("抵抗") || value.Contains("RES") || value.Contains("res")|| value.Contains("teikou"))
			op.param = RuneParam.Reg;
		else if (value.Contains("速度") || value.Contains("SPD") || value.Contains("spd")|| value.Contains("sokudo"))
			op.param = RuneParam.Spd;

		return op;
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
			addRuneKindInput.text = addRuneData.type.ToJapaneseString();
		}
		// ルーン名更新
		string runeName = string.Format("{0}番 ★{1} {2}のルーン+{3}", addRuneData.no, addRuneData.rank, addRuneData.type.ToJapaneseString(), addRuneData.level);
		addRuneNameText.text = runeName;
		// ルーン画像更新
		addRuneImage.sprite = GetRuneSprite(addRuneData.type);
	}
		
	public int GetFlatValue(int rarity, int level, int no)
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
		return 0;
	}

	public void UpdateMainFlat()
	{
		// 1, 3, 5番の場合はメインOP固定
		if (addRuneData.no == 1)
		{
			int val = GetFlatValue(addRuneData.rank, addRuneData.level, addRuneData.no);
			addRuneMainOPInput.text = addRuneMainOPText.text = "攻撃+" + val;
			addRuneData.mainOption.param = RuneParam.Atk_Flat;
			addRuneData.mainOption.value = val;
		}
		else if (addRuneData.no == 3)
		{
			int val = GetFlatValue(addRuneData.rank, addRuneData.level, addRuneData.no);
			addRuneMainOPInput.text = addRuneMainOPText.text = "防御+" + val;
			addRuneData.mainOption.param = RuneParam.Def_Flat;
			addRuneData.mainOption.value = val;
		}
		else if (addRuneData.no == 5)
		{
			int val = GetFlatValue(addRuneData.rank, addRuneData.level, addRuneData.no);
			addRuneMainOPInput.text = addRuneMainOPText.text = "体力+" + val;
			addRuneData.mainOption.param = RuneParam.HP_Flat;
			addRuneData.mainOption.value = val;
		}
	}

	public void OnNoChanged(string value)
	{
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

		// 数字を先に取り出す
		try {
			Regex re = new Regex(@"[^0-9]");
			string numValue = re.Replace(value, "");
			int num = System.Int32.Parse(numValue);
			if (num >= 1 && num <= 6) {
				addRuneData.no = num;
				UpdateMainFlat();
			} else {
				addRuneNoInput.text = addRuneData.no.ToString();
			}
		}
		catch (System.Exception e){
			Debug.LogError(e.ToString());
			addRuneNoInput.text = addRuneData.no.ToString();
		}

		// ルーン名更新
		string runeName = string.Format("{0}番 ★{1} {2}のルーン+{3}", addRuneData.no, addRuneData.rank, addRuneData.type.ToJapaneseString(), addRuneData.level);
		addRuneNameText.text = runeName;
	}

	public void OnRarityChanged(string value)
	{
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

		// 数字を先に取り出す
		try {
			Regex re = new Regex(@"[^0-9]");
			string numValue = re.Replace(value, "");
			int num = System.Int32.Parse(numValue);

			if (num >= 1 && num <= 6) {
				addRuneData.rank = num;
				UpdateMainFlat();
			} else {
				addRuneRarityInput.text = addRuneData.rank.ToString();
			}

			UpdateMainFlat();
		}
		catch (System.Exception e){
			Debug.LogError(e.ToString());
			addRuneRarityInput.text = addRuneData.rank.ToString();
		}

		// ルーン名更新
		string runeName = string.Format("{0}番 ★{1} {2}のルーン+{3}", addRuneData.no, addRuneData.rank, addRuneData.type.ToJapaneseString(), addRuneData.level);
		addRuneNameText.text = runeName;
	}

	public void OnLevelChanged(string value)
	{
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

		// 数字を先に取り出す
		try {
			Regex re = new Regex(@"[^0-9]");
			string numValue = re.Replace(value, "");
			int num = System.Int32.Parse(numValue);

			if (num >= 1 && num <= 15) {
				addRuneData.level = num;
				UpdateMainFlat();
			} else {
				addRuneLevelInput.text = addRuneData.level.ToString();
			}
			UpdateMainFlat();
		}
		catch (System.Exception e){
			Debug.LogError(e.ToString());
			addRuneLevelInput.text = addRuneData.level.ToString();
		}

		// ルーン名更新
		string runeName = string.Format("{0}番 ★{1} {2}のルーン+{3}", addRuneData.no, addRuneData.rank, addRuneData.type.ToJapaneseString(), addRuneData.level);
		addRuneNameText.text = runeName;
	}

	public void OnOptionMainChanged(string value)
	{
		Text textField = addRuneMainOPText;
		if (value == null) {
			textField.text = "";
			return;
		}
		RuneOption op = ValidateRuneOption(value);
		// 値の更新
		addRuneData.mainOption = op;
		textField.text = GetOptionString(op);
	}

	public void OnOptionSubChanged(string value)
	{
		Text textField = addRuneSubOPText;
		if (value == null) {
			textField.text = "";
			return;
		}
		RuneOption op = ValidateRuneOption(value);
		// 値の更新
		addRuneData.subOption = op;
		textField.text = GetOptionString(op);
	}

	public void OnBonus1OPChanged(string value)
	{
		Text textField = addRuneBonus1OPText;
		if (value == null) {
			textField.text = "";
			return;
		}
		RuneOption op = ValidateRuneOption(value);
		// 値の更新
		addRuneData.bonusOption[0] = op;
		textField.text = GetOptionString(op);
	}

	public void OnBonus2OPChanged(string value)
	{
		Text textField = addRuneBonus2OPText;
		if (value == null) {
			textField.text = "";
			return;
		}
		RuneOption op = ValidateRuneOption(value);
		// 値の更新
		addRuneData.bonusOption[1] = op;
		textField.text = GetOptionString(op);
	}

	public void OnBonus3OPChanged(string value)
	{
		Text textField = addRuneBonus3OPText;
		if (value == null) {
			textField.text = "";
			return;
		}
		RuneOption op = ValidateRuneOption(value);
		// 値の更新
		addRuneData.bonusOption[2] = op;
		textField.text = GetOptionString(op);
	}

	public void OnBonus4OPChanged(string value)
	{
		Text textField = addRuneBonus4OPText;
		if (value == null) {
			textField.text = "";
			return;
		}
		RuneOption op = ValidateRuneOption(value);
		// 値の更新
		addRuneData.bonusOption[3] = op;
		textField.text = GetOptionString(op);
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
