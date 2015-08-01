using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using SW;
using Kender.uGUI;


public partial class RuneManager : SingletonObject<RuneManager> {

	public Canvas runeSetCanvas;

	// ルーン条件
	public ComboBox runeSetKind1;
	public ComboBox runeSetKind2;
	public ComboBox runeSetKind3;
	public ComboBox runeSetFixOption1;
	public ComboBox runeSetFixOption2;
	public ComboBox runeSetFixOption3;
	public ComboBox runeSetPrimary1;
	public ComboBox runeSetPrimary2;
	public ComboBox runeSetPrimary3;
	public Toggle runeSetAttack;

	// モンスター＆ステータス
	public ComboBox runeSetMonsterRarity;
	public ComboBox runeSetMonsterElement;
	public ComboBox runeSetMonsterRace;
	public InputField runeSetInputHP;
	public Text runeSetTextHP;
	public InputField runeSetInputATK;
	public Text runeSetTextATK;
	public InputField runeSetInputDEF;
	public Text runeSetTextDEF;
	public InputField runeSetInputSPD;
	public Text runeSetTextSPD;
	public InputField runeSetInputCRI;
	public Text runeSetTextCRI;
	public InputField runeSetInputDMG;
	public Text runeSetTextDMG;
	public InputField runeSetInputRES;
	public Text runeSetTextRES;
	public InputField runeSetInputACC;
	public Text runeSetTextACC;
	public Text attackPointText;
	public Text defencePointText;
	public Text totalPointText;

	// ルーンセット情報
	public Button[] runeBaseButton;
	public Image[] runeBaseImage;

	// ルーンプレビュー
	public InputField tagEditInput;
	public Image runePreviewImage;
	public Text runePreviewRuneName;
	public Text[] runePreviewOptionText;
	public Text runePreviewTagText;
	public Text runePreviewValueText;
	public Button runePreviewEditButton;
	public Button runePreviewChangeButton;


	public Button runeSetResetButton;
	public Button runeSetAutoSetButton;


	public RuneSet currentRuneSet;

	/// <summary>
	/// ルーンセット画面から変更中のルーン番号(0..5)
	/// </summary>
	private int currentChangingRune;

	public void RuneSetAwake()
	{
		currentRuneSet = new RuneSet();
		runeSetKind3.Interactable = false;
		runeSetMonsterRace.Interactable = false;
		StatusUpdate();

		// ルーン条件
		runeSetKind1.OnSelectionChanged = OnRuneKind1Changed;
		runeSetKind2.OnSelectionChanged = OnRuneKind2Changed;
		runeSetKind3.OnSelectionChanged = OnRuneKind3Changed;
		runeSetFixOption1.OnSelectionChanged = OnFixOption1Changed;
		runeSetFixOption2.OnSelectionChanged = OnFixOption2Changed;
		runeSetFixOption3.OnSelectionChanged = OnFixOption3Changed;
		runeSetPrimary1.OnSelectionChanged = OnPrimary1Changed;
		runeSetPrimary2.OnSelectionChanged = OnPrimary2Changed;
		runeSetPrimary3.OnSelectionChanged = OnPrimary3Changed;

		// モンスター
		runeSetMonsterRarity.OnSelectionChanged = OnMonsterRarityChanged;
		runeSetMonsterElement.OnSelectionChanged = OnMonsterElementChanged;
		runeSetMonsterRace.OnSelectionChanged = OnMonsterRaceChanged;

		// ステータス
		runeSetInputHP.onEndEdit.AddListener(val=>{ OnMonsterHPChanged(val); });
		runeSetInputATK.onEndEdit.AddListener(val=>{ OnMonsterATKChanged(val); });
		runeSetInputDEF.onEndEdit.AddListener(val=>{ OnMonsterDEFChanged(val); });
		runeSetInputSPD.onEndEdit.AddListener(val=>{ OnMonsterSPDChanged(val); });
		runeSetInputCRI.onEndEdit.AddListener(val=>{ OnMonsterCRIChanged(val); });
		runeSetInputDMG.onEndEdit.AddListener(val=>{ OnMonsterDMGChanged(val); });
		runeSetInputRES.onEndEdit.AddListener(val=>{ OnMonsterRESChanged(val); });
		runeSetInputACC.onEndEdit.AddListener(val=>{ OnMonsterACCChanged(val); });

		// ルーン
		RunePreview(null);
		tagEditInput.onEndEdit.AddListener(val=>{ OnTagEdit(val); });
	}

	public void RuneSetInitialize()
	{
		isSelectionMode = false;
		runeSetCanvas.enabled = true;
		RuneDetail(currentRuneSet.runes[ currentChangingRune ]);
		RunePreview(currentRuneSet);
		StatusUpdate();
	}

	public void RuneSetClose()
	{
		runeSetCanvas.enabled = false;
	}

	// ルーン条件
	public int GetRuneSetCount(RuneType type)
	{
		switch (type)
		{
		case RuneType.Blade:
		case RuneType.Endure:
		case RuneType.Energy:
		case RuneType.Focus:
		case RuneType.Guard:
		case RuneType.Nemesis:
		case RuneType.Revenge:
		case RuneType.Shield:
		case RuneType.Will:
			return 2;// これらは2セットルーン
		case RuneType.Despair:
		case RuneType.Fatal:
		case RuneType.Rage:
		case RuneType.Swift:
		case RuneType.Violent:
		case RuneType.Vampire:
			return 4;//これらは4セットルーン
		}
		return 2;
	}
	public RuneParam GetRuneParamFromString(string value)
	{
		if (value == null) return RuneParam.None;

		value = value.Replace("％", "%");

		if (value.Contains("体力%"))
			return RuneParam.HP_Percent;
		if (value.Contains("体力"))
			return RuneParam.HP_Flat;
		if (value.Contains("攻撃%"))
			return RuneParam.Atk_Percent;
		if (value.Contains("攻撃"))
			return RuneParam.Atk_Flat;
		if (value.Contains("防御%"))
			return RuneParam.Def_Percent;
		if (value.Contains("防御"))
			return RuneParam.Def_Flat;
		if (value.Contains("速度"))
			return RuneParam.Spd;
		if (value.Contains("クリ率"))
			return RuneParam.Cri;
		if (value.Contains("クリダメ"))
			return RuneParam.CriDmg;
		if (value.Contains("抵抗"))
			return RuneParam.Reg;
		if (value.Contains("的中"))
			return RuneParam.Acc;

		return RuneParam.None;
	}
	public RuneType GetRuneTypeFromString(string value)
	{
		if (value.Contains("元気") || value.Contains("Energy") || value.Contains("genki"))
			return RuneType.Energy;
		else if (value.Contains("猛攻") || value.Contains("Fatal")|| value.Contains("moukou"))
			return RuneType.Fatal;
		else if (value.Contains("刃") || value.Contains("Blade")|| value.Contains("yaiba"))
			return RuneType.Blade;
		else if (value.Contains("激怒") || value.Contains("Rage")|| value.Contains("gekido"))
			return RuneType.Rage;
		else if (value.Contains("迅速") || value.Contains("Swift")|| value.Contains("jinsoku")|| value.Contains("jinnsoku"))
			return RuneType.Swift;
		else if (value.Contains("集中") || value.Contains("Focus")|| value.Contains("syuutyuu")|| value.Contains("syutyu"))
			return RuneType.Focus;
		else if (value.Contains("絶望") || value.Contains("Despair")|| value.Contains("zetubou"))
			return RuneType.Despair;
		else if (value.Contains("守護") || value.Contains("Guard")|| value.Contains("syugo"))
			return RuneType.Guard;
		else if (value.Contains("忍耐") || value.Contains("Endure")|| value.Contains("nintai")|| value.Contains("ninntai"))
			return RuneType.Endure;
		else if (value.Contains("暴走") || value.Contains("Violent")|| value.Contains("bousou"))
			return RuneType.Violent;
		else if (value.Contains("意思") || value.Contains("Will")|| value.Contains("ishi"))
			return RuneType.Will;
		else if (value.Contains("果報") || value.Contains("Nemesis")|| value.Contains("kahou"))
			return RuneType.Nemesis;
		else if (value.Contains("保護") || value.Contains("Shield")|| value.Contains("hogo"))
			return RuneType.Shield;
		else if (value.Contains("反撃") || value.Contains("Revenge")|| value.Contains("hangeki"))
			return RuneType.Revenge;
		else if (value.Contains("吸血") || value.Contains("Vampire")|| value.Contains("kyuuketu"))
			return RuneType.Vampire;

		return RuneType.Energy;
	}
	public void OnRuneKind1Changed(int value)
	{
		try {
			int runeSetCount = 0;
			RuneType t1 = GetRuneTypeFromString(runeSetKind1.Items[ runeSetKind1.SelectedIndex ].Caption);
			RuneType t2 = GetRuneTypeFromString(runeSetKind2.Items[ runeSetKind2.SelectedIndex ].Caption);
			runeSetCount = GetRuneSetCount(t1) + GetRuneSetCount(t2);
			if (runeSetKind1.SelectedIndex != 0 && runeSetKind2.SelectedIndex != 0 && runeSetCount <= 4)
			{
				runeSetKind3.Interactable = true;
			}
			else {
				runeSetKind3.SelectedIndex = 0;
				runeSetKind3.Interactable = false;
			}
		}
		catch
		{

		}
	}
	public void OnRuneKind2Changed(int value)
	{
		try {
			int runeSetCount = 0;
			RuneType t1 = GetRuneTypeFromString(runeSetKind1.Items[ runeSetKind1.SelectedIndex ].Caption);
			RuneType t2 = GetRuneTypeFromString(runeSetKind2.Items[ runeSetKind2.SelectedIndex ].Caption);
			runeSetCount = GetRuneSetCount(t1) + GetRuneSetCount(t2);
			if (runeSetKind1.SelectedIndex != 0 && runeSetKind2.SelectedIndex != 0 && runeSetCount <= 4)
			{
				runeSetKind3.Interactable = true;
			}
			else {
				runeSetKind3.SelectedIndex = 0;
				runeSetKind3.Interactable = false;
			}
		}
		catch
		{

		}
	}
	public void OnRuneKind3Changed(int value)
	{

	}
	public void OnFixOption1Changed(int value)
	{
		
	}
	public void OnFixOption2Changed(int value)
	{
		
	}
	public void OnFixOption3Changed(int value)
	{
		
	}
	public void OnPrimary1Changed(int valie)
	{
		
	}
	public void OnPrimary2Changed(int valie)
	{

	}
	public void OnPrimary3Changed(int valie)
	{

	}

	// その他の条件


	// ステータス
	public void OnMonsterRarityChanged(int value)
	{
		if (runeSetMonsterRarity.SelectedIndex != 0 && runeSetMonsterElement.SelectedIndex != 0)
		{
			Debug.Log("add race");
			runeSetMonsterRace.Interactable = true;

			// レアリティと属性から、存在するモンスター種別を全取得
			Element element = (Element)(runeSetMonsterElement.SelectedIndex);
			int rarity = runeSetMonsterRarity.SelectedIndex;
			List<string> allRaces = new List<string>(new string[]{ "" });
			foreach (MonsterData dto in Data.Instance.mon.mons)
			{
				if (dto.element != element) continue;
				if (dto.rarity != rarity) continue;
				if (allRaces.Contains(dto.race)==false) {
					allRaces.Add(dto.race);
				}
			}

			// アイテム追加
			runeSetMonsterRace.ClearItems();
			runeSetMonsterRace.AddItems( allRaces.ToArray() );
		}
		else
		{
			runeSetMonsterRace.ClearItems();
			runeSetMonsterRace.Interactable = false;
		}
	}
	public void OnMonsterElementChanged(int value)
	{
		if (runeSetMonsterRarity.SelectedIndex != 0 && runeSetMonsterElement.SelectedIndex != 0)
		{
			runeSetMonsterRace.Interactable = true;

			// レアリティと属性から、存在するモンスター種別を全取得
			Element element = (Element)(runeSetMonsterElement.SelectedIndex);
			int rarity = runeSetMonsterRarity.SelectedIndex;
			List<string> allRaces = new List<string>(new string[]{ "" });
			foreach (MonsterData dto in Data.Instance.mon.mons)
			{
				if (dto.element != element) continue;
				if (dto.rarity != rarity) continue;
				if (allRaces.Contains(dto.race)==false) {
					allRaces.Add(dto.race);
				}
			}

			// アイテム追加
			runeSetMonsterRace.ClearItems();
			runeSetMonsterRace.AddItems( allRaces.ToArray() );
		}
		else
		{
			runeSetMonsterRace.ClearItems();
			runeSetMonsterRace.Interactable = false;
		}
	}
	public void OnMonsterRaceChanged(int value)
	{
		Element element = (Element)(runeSetMonsterElement.SelectedIndex);
		int rarity = runeSetMonsterRarity.SelectedIndex;
		MonsterData dto = Data.Instance.mon.mons.Find(a=>{
			return (a.rarity==rarity) && (a.element==element) && (a.race==runeSetMonsterRace.Items[value].Caption);
		});

		if (dto != null)
		{
			// 該当モンスターのステータス反映
			runeSetInputHP.text = "" + dto.b_hp;
			runeSetInputATK.text = "" + dto.b_atk;
			runeSetInputDEF.text = "" + dto.b_def;
			runeSetInputSPD.text = "" + dto.b_spd;
			runeSetInputCRI.text = "" + dto.b_crate;
			runeSetInputDMG.text = "" + dto.b_cdmg;
			runeSetInputRES.text = "" + dto.b_res;
			runeSetInputACC.text = "" + dto.b_acc;
		}
		StatusUpdate();
	}

	// ステータス
	public void OnMonsterHPChanged(string value)
	{
		StatusUpdate();
	}
	public void OnMonsterATKChanged(string value)
	{
		StatusUpdate();
	}
	public void OnMonsterDEFChanged(string value)
	{
		StatusUpdate();
	}
	public void OnMonsterSPDChanged(string value)
	{
		StatusUpdate();
	}
	public void OnMonsterCRIChanged(string value)
	{
		StatusUpdate();
	}
	public void OnMonsterDMGChanged(string value)
	{
		StatusUpdate();
	}
	public void OnMonsterRESChanged(string value)
	{
		StatusUpdate();
	}
	public void OnMonsterACCChanged(string value)
	{
		StatusUpdate();
	}
	public void StatusUpdate()
	{
		Status st = new Status();
		Int32.TryParse( runeSetInputHP.text, out st.hp );
		Int32.TryParse( runeSetInputATK.text, out st.atk );
		Int32.TryParse( runeSetInputDEF.text, out st.def );
		Int32.TryParse( runeSetInputSPD.text, out st.spd );
		Int32.TryParse( runeSetInputCRI.text, out st.cri );
		Int32.TryParse( runeSetInputDMG.text, out st.cdmg );
		Int32.TryParse( runeSetInputRES.text, out st.res );
		Int32.TryParse( runeSetInputACC.text, out st.acc );

		int t_hp = currentRuneSet.GetTotalHP(st.hp);
		int t_atk = currentRuneSet.GetTotalATK(st.atk);
		int t_def = currentRuneSet.GetTotalDEF(st.def);
		int t_spd = currentRuneSet.GetTotalSPD(st.spd);
		int t_cri = currentRuneSet.GetTotalCRI(st.cri);
		int t_cdmg = currentRuneSet.GetTotalCRIDMG(st.cdmg);
		int t_res = currentRuneSet.GetTotalRES(st.res);
		int t_acc = currentRuneSet.GetTotalACC(st.acc);

		runeSetTextHP.text = string.Format("{0} (+{1})", t_hp, t_hp-st.hp);
		runeSetTextATK.text = string.Format("{0} (+{1})", t_atk, t_atk-st.atk);
		runeSetTextDEF.text = string.Format("{0} (+{1})", t_def, t_def-st.def);
		runeSetTextSPD.text = string.Format("{0} (+{1})", t_spd, t_spd-st.spd);
		runeSetTextCRI.text = string.Format("{0} (+{1})", t_cri, t_cri-st.cri);
		runeSetTextDMG.text = string.Format("{0} (+{1})", t_cdmg, t_cdmg-st.cdmg);
		runeSetTextRES.text = string.Format("{0} (+{1})", t_res, t_res-st.res);
		runeSetTextACC.text = string.Format("{0} (+{1})", t_acc, t_acc-st.acc);

		// 攻撃指数
		int attackPoint = currentRuneSet.GetAttackPoint(st.atk, st.cri, st.cdmg);
		int defencePoint = currentRuneSet.GetDefencePoint(st.hp, st.def);
		Status weight = Status.One;
		int totalPoint = currentRuneSet.GetTotalValuePoint(st, weight);
		attackPointText.text = string.Format("攻撃指数 : {0}", attackPoint);
		defencePointText.text = string.Format("耐久指数 : {0}", defencePoint);
		totalPointText.text = string.Format("総合戦力 : {0}", totalPoint);
	}


	// ルーンセット
	public void OnTapRune1()
	{
		currentChangingRune = 0;
		RuneDetail(currentRuneSet.runes[ currentChangingRune ]);
	}
	public void OnTapRune2()
	{
		currentChangingRune = 1;
		RuneDetail(currentRuneSet.runes[ currentChangingRune ]);
	}
	public void OnTapRune3()
	{
		currentChangingRune = 2;
		RuneDetail(currentRuneSet.runes[ currentChangingRune ]);
	}
	public void OnTapRune4()
	{
		currentChangingRune = 3;
		RuneDetail(currentRuneSet.runes[ currentChangingRune ]);
	}
	public void OnTapRune5()
	{
		currentChangingRune = 4;
		RuneDetail(currentRuneSet.runes[ currentChangingRune ]);
	}
	public void OnTapRune6()
	{
		currentChangingRune = 5;
		RuneDetail(currentRuneSet.runes[ currentChangingRune ]);
	}


	// ルーンプレビュー
	public void OnTagEdit(string val)
	{
		val = val.Trim();
		if (string.IsNullOrEmpty(val)) { return; }

		bool flag = false;
		foreach (RuneData dto in currentRuneSet.runes)
		{
			if (dto==null) continue;
			dto.owner = val;
			flag = true;
		}
		if (flag)
		{// 更新があったときだけ表示する
			Data.Save();
			DialogCanvas.Create("タグを変更しました。", Color.black, ()=>{
				RuneDetail( currentRuneSet.runes[ currentChangingRune ] );
			});
		}
		tagEditInput.text = "";
	}
	public void RunePreview(RuneSet runeSet)
	{
		if (runeSet==null) {
			for (int i=0; i<6; i++)
			{
				runeBaseImage[i].color = Color.clear;
				runeBaseButton[i].image.color = Color.clear;
			}
			return;
		}

		for (int i=0; i<6; i++)
		{
			if (runeSet.runes[i]==null || runeSet.runes[i]==RuneData.BlankRune)
			{
				runeBaseImage[i].color = Color.clear;
				runeBaseButton[i].image.color = Color.clear;
			}
			else
			{
				runeBaseImage[i].color = Color.white;
				runeBaseButton[i].image.color = Color.white;
				runeBaseButton[i].image.sprite = GetRuneSprite( runeSet.runes[i].type );
			}
		}
	}
	public void RuneDetail(RuneData data)
	{
		if (data==null) { data=RuneData.BlankRune; }

		runePreviewImage.sprite = GetRuneSprite(data.type);

		// ルーンの名前
		runePreviewRuneName.text = RuneManager.Instance.GetRuneNameString(data);

		// メインオプション
		string s = RuneManager.Instance.GetOptionString(data.mainOption);
		runePreviewOptionText[0].text = (s=="")? "---" : s;

		// サブオプション
		s = RuneManager.Instance.GetOptionString(data.subOption);
		runePreviewOptionText[1].text = (s=="")? "---" : s;

		// ボーナスオプションを集めて
		List<RuneOption> bonusOP = new List<RuneOption>(data.bonusOption);

		// ボーナス1
		s = RuneManager.Instance.GetOptionString(bonusOP[0]);
		runePreviewOptionText[2].text = (s=="")? "---" : s;

		// ボーナス2
		s = RuneManager.Instance.GetOptionString(bonusOP[1]);
		runePreviewOptionText[3].text = (s=="")? "---" : s;

		// ボーナス3
		s = RuneManager.Instance.GetOptionString(bonusOP[2]);
		runePreviewOptionText[4].text = (s=="")? "---" : s;

		// ボーナス4
		s = RuneManager.Instance.GetOptionString(bonusOP[3]);
		runePreviewOptionText[5].text = (s=="")? "---" : s;

		// タグ
		runePreviewTagText.text = "" + data.owner;

		runePreviewValueText.text = "評価 : " + (data.TotalValue).ToString() + " Pt";
	}

	public void OnRunePreviewEdit()
	{
		if (currentRuneSet.runes[ currentChangingRune ] != RuneData.BlankRune)
		{
			AddRuneInitialize(currentRuneSet.runes[ currentChangingRune ]);
		}
		else
		{
			DialogCanvas.Create("ルーンがありません", Color.black, ()=>{});
		}
	}
	public void OnRunePreviewChange()
	{
		RuneSetClose();

		// ルーンの種類
		List<RuneType> runeTypes = new List<RuneType>();
		int runeSetCount = 0;
		if (runeSetKind1.SelectedIndex != 0) {
			RuneType t = GetRuneTypeFromString(runeSetKind1.Items[ runeSetKind1.SelectedIndex ].Caption);
			runeTypes.Add(t);
		}
		if (runeSetKind2.SelectedIndex != 0) {
			RuneType t = GetRuneTypeFromString(runeSetKind2.Items[ runeSetKind2.SelectedIndex ].Caption);
			runeTypes.Add(t);
		}
		if (runeSetKind3.SelectedIndex != 0) {
			RuneType t = GetRuneTypeFromString(runeSetKind3.Items[ runeSetKind3.SelectedIndex ].Caption);
			runeTypes.Add(t);
		}

		RuneListInitialize(true, currentChangingRune+1, runeTypes.ToArray());
	}

	public void OnHelpStatus()
	{
		DialogCanvas.Create("「攻撃指数 = 非クリ時の攻撃力 + クリ時の攻撃力」で求められる攻撃能力の指標",
			Color.blue,
			()=>{});
	}

	public void OnRuneSetReset()
	{
		// ルーンセットをクリア
		currentRuneSet = new RuneSet();
		RunePreview(currentRuneSet);
		StatusUpdate();
	}

	/// <summary>
	/// 条件にあったルーンをさがす
	/// </summary>
	public void OnRuneSetSearch()
	{
		// ディープコピー
		List<RuneData> list = new List<RuneData>( Data.Instance.inv.runes );
		
		// ルーンの種類
		List<RuneType> runeTypes = new List<RuneType>();
		int runeSetCount = 0;
		if (runeSetKind1.SelectedIndex != 0) {
			RuneType t = GetRuneTypeFromString(runeSetKind1.Items[ runeSetKind1.SelectedIndex ].Caption);
			int addCount = GetRuneSetCount(t);
			for (int i=0; i<addCount; i++) {
				runeTypes.Add(t);
			}
			runeSetCount+=addCount;
		}
		if (runeSetKind2.SelectedIndex != 0) {
			RuneType t = GetRuneTypeFromString(runeSetKind2.Items[ runeSetKind2.SelectedIndex ].Caption);
			int addCount = GetRuneSetCount(t);
			for (int i=0; i<addCount; i++) {
				runeTypes.Add(t);
			}
			runeSetCount+=addCount;
		}
		if (runeSetKind3.SelectedIndex != 0) {
			RuneType t = GetRuneTypeFromString(runeSetKind3.Items[ runeSetKind3.SelectedIndex ].Caption);
			int addCount = GetRuneSetCount(t);
			for (int i=0; i<addCount; i++) {
				runeTypes.Add(t);
			}
			runeSetCount+=addCount;
		}

		// ルーン
		if (runeSetCount == 6)
		{
			// ルーンセット条件に当てはまらないルーンは除外
			list.RemoveAll( a=>{ return runeTypes.Contains(a.type)==false; } );
		}

		// メインオプション条件
		if (runeSetFixOption1.SelectedIndex != 0) {
			list.RemoveAll( a=>{
				if (a.no == 2) {// 2番ルーンで
					RuneParam param = GetRuneParamFromString( runeSetFixOption1.Items[ runeSetFixOption1.SelectedIndex ].Caption );
					if (a.mainOption.param != param) {// メインオプションが一致していない
						return true;// 除外
					}
				}
				return false;
			});
		}
		if (runeSetFixOption2.SelectedIndex != 0) {
			list.RemoveAll( a=>{
				if (a.no == 4) {// 4番ルーンで
					RuneParam param = GetRuneParamFromString( runeSetFixOption2.Items[ runeSetFixOption2.SelectedIndex ].Caption );
					if (a.mainOption.param != param) {// メインオプションが一致していない
						return true;// 除外
					}
				}
				return false;
			});
		}
		if (runeSetFixOption3.SelectedIndex != 0) {
			list.RemoveAll( a=>{
				if (a.no == 6) {// 2番ルーンで
					RuneParam param = GetRuneParamFromString( runeSetFixOption3.Items[ runeSetFixOption3.SelectedIndex ].Caption );
					if (a.mainOption.param != param) {// メインオプションが一致していない
						return true;// 除外
					}
				}
				return false;
			});
		}

		// 優先ステータス
		// 各ステータスに評価係数を用意、優先順位が高いステータスには係数を高く設定する
		Status wt = new Status();
		wt.hp = wt.atk = wt.def = wt.spd = wt.cri = wt.cdmg = wt.res = wt.acc = 1;

		if (runeSetPrimary3.SelectedIndex != 0)
		{
			int index = runeSetPrimary3.SelectedIndex;
			switch (index)
			{
			case 1: wt.hp =  6; break;
			case 2: wt.atk = 6; break;
			case 3: wt.def = 6; break;
			case 4: wt.spd = 6; break;
			case 5: wt.cri = 6; break;
			case 6: wt.cdmg= 6; break;
			case 7: wt.res = 6; break;
			case 8: wt.acc = 6; break;
			}
		}
		if (runeSetPrimary2.SelectedIndex != 0)
		{
			int index = runeSetPrimary2.SelectedIndex;
			switch (index)
			{
			case 1: wt.hp =  8; break;
			case 2: wt.atk = 8; break;
			case 3: wt.def = 8; break;
			case 4: wt.spd = 8; break;
			case 5: wt.cri = 8; break;
			case 6: wt.cdmg= 8; break;
			case 7: wt.res = 8; break;
			case 8: wt.acc = 8; break;
			}
		}
		if (runeSetPrimary1.SelectedIndex != 0)
		{
			int index = runeSetPrimary1.SelectedIndex;
			switch (index)
			{
			case 1: wt.hp =  10; break;
			case 2: wt.atk = 10; break;
			case 3: wt.def = 10; break;
			case 4: wt.spd = 10; break;
			case 5: wt.cri = 10; break;
			case 6: wt.cdmg= 10; break;
			case 7: wt.res = 10; break;
			case 8: wt.acc = 10; break;
			}
		}

		// 候補の絞り込みは終わり全てのルーン
		List<RuneData> rune1 = new List<RuneData>( list.FindAll(a=>a.no==1) );
		List<RuneData> rune2 = new List<RuneData>( list.FindAll(a=>a.no==2) );
		List<RuneData> rune3 = new List<RuneData>( list.FindAll(a=>a.no==3) );
		List<RuneData> rune4 = new List<RuneData>( list.FindAll(a=>a.no==4) );
		List<RuneData> rune5 = new List<RuneData>( list.FindAll(a=>a.no==5) );
		List<RuneData> rune6 = new List<RuneData>( list.FindAll(a=>a.no==6) );

		// 要素が0個にならないよう空ルーンを追加
		if (rune1.Count==0) rune1.Add(RuneData.BlankRune);
		if (rune2.Count==0) rune2.Add(RuneData.BlankRune);
		if (rune3.Count==0) rune3.Add(RuneData.BlankRune);
		if (rune4.Count==0) rune4.Add(RuneData.BlankRune);
		if (rune5.Count==0) rune5.Add(RuneData.BlankRune);
		if (rune6.Count==0) rune6.Add(RuneData.BlankRune);

		// 優先ステータス順になるようソートする
		rune1.Sort( (a,b)=>{ return b.GetValue(wt) - a.GetValue(wt); } );
		rune2.Sort( (a,b)=>{ return b.GetValue(wt) - a.GetValue(wt); } );
		rune3.Sort( (a,b)=>{ return b.GetValue(wt) - a.GetValue(wt); } );
		rune4.Sort( (a,b)=>{ return b.GetValue(wt) - a.GetValue(wt); } );
		rune5.Sort( (a,b)=>{ return b.GetValue(wt) - a.GetValue(wt); } );
		rune6.Sort( (a,b)=>{ return b.GetValue(wt) - a.GetValue(wt); } );

		// 最も良いルーンの組み合わせ情報を保存する変数
		RuneData[] bestRune = new RuneData[6];
		int value_max = 0;
		int maxCout = 10;// 検索の精度に関わる数、価値計算して上位何個までを汲み取るか
		int max1 = Math.Min(rune1.Count, maxCout);
		int max2 = Math.Min(rune2.Count, maxCout);
		int max3 = Math.Min(rune3.Count, maxCout);
		int max4 = Math.Min(rune4.Count, maxCout);
		int max5 = Math.Min(rune5.Count, maxCout);
		int max6 = Math.Min(rune6.Count, maxCout);

		// ステータスの取り出し
		Status st = new Status();
		Int32.TryParse(runeSetInputHP.text, out st.hp);
		Int32.TryParse(runeSetInputATK.text, out st.atk);
		Int32.TryParse(runeSetInputDEF.text, out st.def);
		Int32.TryParse(runeSetInputSPD.text, out st.spd);
		Int32.TryParse(runeSetInputCRI.text, out st.cri);
		Int32.TryParse(runeSetInputDMG.text, out st.cdmg);
		Int32.TryParse(runeSetInputRES.text, out st.res);
		Int32.TryParse(runeSetInputACC.text, out st.acc);


		int freeRuneSetCount = 0;
		for (int i1=0; i1<max1; i1++)
		{
			List<RuneType> leftType = new List<RuneType>( runeTypes );
			freeRuneSetCount = 6-runeSetCount;
			bool free = false;

			if (leftType.Contains(rune1[i1].type) == false) {
				if (freeRuneSetCount <= 0) { continue; }
				else {
					freeRuneSetCount--;
					free = true;
				}
			}else { free = false; }
			leftType.Remove(rune1[i1].type);

			for (int i2=0; i2<max2; i2++)
			{
				if (leftType.Contains(rune2[i2].type) == false) {
					if (freeRuneSetCount <= 0) { continue; }
					else {
						freeRuneSetCount--;
						free = true;
					}
				}else { free = false; }
				leftType.Remove(rune2[i2].type);

				for (int i3=0; i3<max3; i3++)
				{
					if (leftType.Contains(rune3[i3].type) == false) {
						if (freeRuneSetCount <= 0) { continue; }
						else {
							freeRuneSetCount--;
							free = true;
						}
					}else { free = false; }
					leftType.Remove(rune3[i3].type);

					for (int i4=0; i4<max4; i4++)
					{
						if (leftType.Contains(rune4[i4].type) == false) {
							if (freeRuneSetCount <= 0) { continue; }
							else {
								freeRuneSetCount--;
								free = true;
							}
						}else { free = false; }
						leftType.Remove(rune4[i4].type);

						for (int i5=0; i5<max5; i5++)
						{
							if (leftType.Contains(rune5[i5].type) == false) {
								if (freeRuneSetCount <= 0) { continue; }
								else {
									freeRuneSetCount--;
									free = true;
								}
							}else { free = false; }
							leftType.Remove(rune5[i5].type);

							for (int i6=0; i6<max6; i6++)
							{
								if (leftType.Contains(rune6[i6].type) == false) {
									if (freeRuneSetCount <= 0) { continue; }
								}

								currentRuneSet.runes[0] = rune1[i1];
								currentRuneSet.runes[1] = rune2[i2];
								currentRuneSet.runes[2] = rune3[i3];
								currentRuneSet.runes[3] = rune4[i4];
								currentRuneSet.runes[4] = rune5[i5];
								currentRuneSet.runes[5] = rune6[i6];

								int valuePoint = 0;
								if (runeSetAttack.isOn) {
									// 攻撃指数を優先 
									valuePoint = currentRuneSet.GetAttackPoint(st.atk, st.cri, st.cdmg);
								}
								else {
									// 総合評価
									valuePoint = currentRuneSet.GetTotalValuePoint(st, wt);
								}

								if (value_max < valuePoint) {
									value_max = valuePoint;
									// ルーンのindexを保存
									bestRune[0] = rune1[i1];
									bestRune[1] = rune2[i2];
									bestRune[2] = rune3[i3];
									bestRune[3] = rune4[i4];
									bestRune[4] = rune5[i5];
									bestRune[5] = rune6[i6];
								}
							}
							leftType.Add(rune5[i5].type);
							if (free) freeRuneSetCount++;
						}
						leftType.Add(rune4[i4].type);
						if (free) freeRuneSetCount++;
					}
					leftType.Add(rune3[i3].type);
					if (free) freeRuneSetCount++;
				}
				leftType.Add(rune2[i2].type);
				if (free) freeRuneSetCount++;
			}
			leftType.Add(rune1[i1].type);
			if (free) freeRuneSetCount++;
		}// end search

		// ルーンセット更新
		currentRuneSet.runes = bestRune;
		RuneDetail( currentRuneSet.runes[ currentChangingRune ] );
		RunePreview(currentRuneSet);
		StatusUpdate();
	}
}
