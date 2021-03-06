﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SW;
using System.Linq;

// ルーン一覧表示関連処理
public partial class RuneManager : SingletonObject<RuneManager> {

	public Canvas runeListCanvas;

	// ルーン番号でフィルター
	public Toggle toggleNo1;
	public Toggle toggleNo2;
	public Toggle toggleNo3;
	public Toggle toggleNo4;
	public Toggle toggleNo5;
	public Toggle toggleNo6;

	// ルーン種別でフィルター
	public Toggle toggleEnergy;
	public Toggle toggleFatal;
	public Toggle toggleBlade;
	public Toggle toggleRage;
	public Toggle toggleSwift;
	public Toggle toggleFocus;
	public Toggle toggleGuard;
	public Toggle toggleEndure;
	public Toggle toggleViolent;
	public Toggle toggleWill;
	public Toggle toggleNemesis;
	public Toggle toggleShield;
	public Toggle toggleRevenge;
	public Toggle toggleDespair;
	public Toggle toggleVampire;

	// 全てチェック/クリア
	public Button AllCheckButton;
	public Button CheckClearButton;

	// 能力値でソート
	public Button TotalSortButton;
	public Button HPSortButton;
	public Button ATKSortButton;
	public Button DEFSortButton;
	public Button SPDSortButton;
	public Button CRISortButton;
	public Button DMGSortButton;
	public Button ACCSortButton;
	public Button RESSortButton;

	// タグ検索入力
	public InputField tagFindInput;

	// ページング関連UI
	public Button pageFirstButton;
	public Button pageBackButton;
	public Button page1Button;
	public Button page2Button;
	public Button page3Button;
	public Button page4Button;
	public Button page5Button;
	public Button pageNextButton;
	public Button pageEndButton;

	// 該当件数テキスト
	public Text runeNumText;

	public RectTransform runeListObject;

	public List<RuneItem> runeItems;
	public int currentPage;
	static readonly int PAGE_LIST_NUM = 13;// 1ページに表示するルーン数
	static readonly float LIST_TOP_POS = 124f;
	static readonly float LIST_ITEM_HEIGHT = 30f;
	static readonly float LIST_MARGIN = 0f;

	// ソート用変数
	public RuneParam sortParam;
	/// <summary>
	/// falseなら降順 trueなら昇順
	/// </summary>
	public bool sortOrder;

	private int runeCount;
	public bool refreshFlag;
	private bool _isSelectionMode;
	public bool isSelectionMode
	{
		get { return _isSelectionMode; }
		set {
			_isSelectionMode = value;
			foreach(RuneItem item in runeItems)
			{
				item.Refresh(_isSelectionMode);
			}
		}
	}

	public void RuneListInitialize(bool isSelectionMode = false, int no = -1, params RuneType[] runeType)
	{
		runeListCanvas.enabled = true;
		currentPage = 0;
		runeCount = 0;
		sortParam = RuneParam.None;
		sortOrder = true;

		// フィルターを全てONに
		if (no == -1)
		{
			MarkAllNoFilter();
		}
		else {
			ClearNoFilter();
			switch (no)
			{
			case 1: toggleNo1.isOn = true; break;
			case 2: toggleNo2.isOn = true; break;
			case 3: toggleNo3.isOn = true; break;
			case 4: toggleNo4.isOn = true; break;
			case 5: toggleNo5.isOn = true; break;
			case 6: toggleNo6.isOn = true; break;
			}
		}
		// ルーン種別
		if (runeType==null || runeType.Length==0)
		{
			MarkAllRuneFilter();
		}
		else
		{
			ClearRuneFilter();
			foreach (RuneType t in runeType)
			{
				switch (t)
				{
				case RuneType.Blade: toggleBlade.isOn = true; break;
				case RuneType.Despair: toggleDespair.isOn = true; break;
				case RuneType.Endure: toggleEndure.isOn = true; break;
				case RuneType.Energy: toggleEnergy.isOn = true; break;
				case RuneType.Fatal: toggleFatal.isOn = true; break;
				case RuneType.Focus: toggleFocus.isOn = true; break;
				case RuneType.Guard: toggleGuard.isOn = true; break;
				case RuneType.Nemesis: toggleNemesis.isOn = true; break;
				case RuneType.Rage: toggleRage.isOn = true; break;
				case RuneType.Revenge: toggleRevenge.isOn = true; break;
				case RuneType.Shield: toggleShield.isOn = true; break;
				case RuneType.Swift: toggleSwift.isOn = true; break;
				case RuneType.Vampire: toggleVampire.isOn = true; break;
				case RuneType.Violent: toggleViolent.isOn = true; break;
				case RuneType.Will: toggleWill.isOn = true; break;
				}
			}
		}
		this.isSelectionMode = isSelectionMode;
	}

	public void RuneListClose()
	{
		runeListCanvas.enabled = false;
	}

	public void RuneListAwake()
	{
		runeItems = new List<RuneItem>();
		toggleNo1.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleNo2.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleNo3.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleNo4.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleNo5.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleNo6.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleEnergy.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleFatal.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleBlade.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleRage.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleSwift.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleFocus.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleGuard.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleEndure.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleViolent.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleWill.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleNemesis.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleShield.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleRevenge.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleDespair.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		toggleVampire.onValueChanged.AddListener(val=>{ refreshFlag=true; });
		tagFindInput.onEndEdit.AddListener(val=>{ refreshFlag=true; });
		page1Button.onClick.AddListener(()=>{ currentPage-=2; refreshFlag=true; });
		page2Button.onClick.AddListener(()=>{ currentPage-=1; refreshFlag=true; });
		page3Button.onClick.AddListener(()=>{ refreshFlag=true; });
		page4Button.onClick.AddListener(()=>{ currentPage+=1; refreshFlag=true; });
		page5Button.onClick.AddListener(()=>{ currentPage+=2; refreshFlag=true; });
	}

	/// <summary>
	/// 更新処理　LateUpdateでrefreshFlagが立っていたときに呼び出し
	/// </summary>
	void Refresh()
	{
		// 表示中のリストは削除
		if (runeItems == null) { runeItems = new List<RuneItem>(); }
		for (int i=0; i<runeItems.Count; i++)
		{
			RuneItem item = runeItems[i];
			Destroy(item.gameObject);
		}
		runeItems.Clear();

		if (Data.Instance.inv == null || Data.Instance.inv.runes == null) { return; }
		
		// ディープコピーする
		var runeList = new List<RuneData>( Data.Instance.inv.runes );

		// フィルタリング処理
		// タグが一致するもので検索
		string findTag = tagFindInput.text;
		findTag = findTag.Trim();
		findTag = findTag.Replace("\n", "");
		findTag = findTag.Replace("　", "");
		if (string.IsNullOrEmpty(findTag)==false)
		{
			Debug.Log("FindTag = " + findTag);
			runeList.RemoveAll(a=>a.owner != findTag);
		}else {
			tagFindInput.text = "";
		}

		if (toggleNo1.isOn == false)
			runeList.RemoveAll(a=>a.no==1);
		if (toggleNo2.isOn == false)
			runeList.RemoveAll(a=>a.no==2);
		if (toggleNo3.isOn == false)
			runeList.RemoveAll(a=>a.no==3);
		if (toggleNo4.isOn == false)
			runeList.RemoveAll(a=>a.no==4);
		if (toggleNo5.isOn == false)
			runeList.RemoveAll(a=>a.no==5);
		if (toggleNo6.isOn == false)
			runeList.RemoveAll(a=>a.no==6);

		if (toggleEnergy.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Energy);
		if (toggleFatal.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Fatal);
		if (toggleBlade.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Blade);
		if (toggleRage.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Rage);
		if (toggleSwift.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Swift);
		if (toggleFocus.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Focus);
		if (toggleGuard.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Guard);
		if (toggleEndure.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Endure);
		if (toggleViolent.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Violent);
		if (toggleWill.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Will);
		if (toggleNemesis.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Nemesis);
		if (toggleShield.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Shield);
		if (toggleRevenge.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Revenge);
		if (toggleDespair.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Despair);
		if (toggleVampire.isOn == false)
			runeList.RemoveAll(a=>a.type==RuneType.Vampire);

		// 選択中のソートボタンをON表示に
		Color selectedColor = sortOrder ? "FF8000".toColor() : Color.cyan;// 昇順ならオレンジ、降順ならシアン
		TotalSortButton.image.color	= sortParam== RuneParam.None ? selectedColor : Color.white;
		HPSortButton.image.color	= sortParam== RuneParam.HP_Percent ? selectedColor : Color.white;
		ATKSortButton.image.color	= sortParam== RuneParam.Atk_Percent ? selectedColor : Color.white;
		DEFSortButton.image.color	= sortParam== RuneParam.Def_Percent ? selectedColor : Color.white;
		SPDSortButton.image.color	= sortParam== RuneParam.Spd ? selectedColor : Color.white;
		CRISortButton.image.color	= sortParam== RuneParam.Cri ? selectedColor : Color.white;
		DMGSortButton.image.color	= sortParam== RuneParam.CriDmg ? selectedColor : Color.white;
		ACCSortButton.image.color	= sortParam== RuneParam.Acc ? selectedColor : Color.white;
		RESSortButton.image.color	= sortParam== RuneParam.Reg ? selectedColor : Color.white;

		runeCount = runeList.Count;

		// ソート
		SortRune(ref runeList, sortOrder);

		// nullは削除
		runeList.RemoveAll(a=>a==null);

		// 最終的な件数の表示
		runeNumText.text = "該当 " + runeList.Count + "件";

		// ページング
		Paging(ref runeList);

		// 現在のページを表示
		int start = PAGE_LIST_NUM * currentPage;
		int end = start + PAGE_LIST_NUM;
		for (int i = start; i<end; i++)
		{
			if (i >= runeList.Count) {
				break;
			}

			// ルーンアイテム追加
			GameObject go = Instantiate(Resources.Load<GameObject>("Prefub/Rune")) as GameObject;
			RuneItem item = go.GetComponent<RuneItem>();

			item.transform.SetParent(runeListObject);

			// 表示位置
			int index = i % PAGE_LIST_NUM;
			item.transform.localPosition = new Vector3(0, LIST_TOP_POS - index * LIST_ITEM_HEIGHT - LIST_MARGIN, 0f);
			item.transform.localScale = Vector3.one;

			// セットアップ
			item.Setup( runeList[i], isSelectionMode );

			runeItems.Add(item);
		}

		refreshFlag = false;
	}

	public void SortRune(ref List<RuneData> runeList, bool sortOrder)
	{
		int order = sortOrder ? -1 : 1;
		switch (sortParam)
		{
		case RuneParam.Acc:
			runeList.Sort((a,b)=>(a.ACC-b.ACC)*order); break;
		case RuneParam.Atk_Flat:
		case RuneParam.Atk_Percent:
			runeList.Sort((a,b)=>{
				return ((a.ATK_Percent*7 + a.ATK_Flat) - (b.ATK_Percent*7 + b.ATK_Flat)) * order;
			});
			break;
		case RuneParam.Cri:
			runeList.Sort((a,b)=>(a.CRI-b.CRI)*order); break;
		case RuneParam.CriDmg:
			runeList.Sort((a,b)=>(a.CRIDMG-b.CRIDMG)*order); break;
		case RuneParam.Def_Flat:
		case RuneParam.Def_Percent:
			runeList.Sort((a,b)=>{
				return ((a.DEF_Percent*7 + a.DEF_Flat)- (b.DEF_Percent*7 + b.DEF_Flat)) * order; 
			});
			break;
		case RuneParam.HP_Flat:
		case RuneParam.HP_Percent:
			runeList.Sort((a,b)=>{
				return ((a.HP_Percent*100 + a.HP_Flat)- (b.HP_Percent*100 + b.HP_Flat)) * order; 
			});
			break;
		case RuneParam.Reg:
			runeList.Sort((a,b)=>(a.RES-b.RES)*order); break;
		case RuneParam.Spd:
			runeList.Sort((a,b)=>(a.SPD-b.SPD)*order); break;
		default: 
			runeList.Sort((a,b)=>(a.TotalValue-b.TotalValue)*order); break;
		}
	}

	public void Paging(ref List<RuneData> runeList)
	{
		int maxPage = Mathf.CeilToInt((float)runeCount / PAGE_LIST_NUM) - 1;
		bool hasPrev = currentPage - 1 >= 0;
		bool hasPrev2 = currentPage - 2 >= 0;
		bool hasNext = currentPage + 1 <= maxPage;
		bool hasNext2 = currentPage + 2 <= maxPage;
		pageBackButton.interactable =
			pageFirstButton.interactable = hasPrev;
		pageNextButton.interactable =
			pageEndButton.interactable = hasNext;

		page1Button.GetComponent<Text>().text = hasPrev2 ? (currentPage-1).ToString() : "";
		page1Button.interactable = hasPrev2;

		page2Button.GetComponent<Text>().text = hasPrev ? (currentPage).ToString() : "";
		page2Button.interactable = hasPrev;

		page3Button.GetComponent<Text>().text = ""+(currentPage+1);
		page3Button.GetComponent<Text>().color = Color.yellow;

		page4Button.GetComponent<Text>().text = hasNext ? (currentPage+2).ToString() : "";
		page4Button.interactable = hasNext;

		page5Button.GetComponent<Text>().text = hasNext2 ? (currentPage+3).ToString() : "";
		page5Button.interactable = hasNext2;
	}

	public void MarkAllNoFilter()
	{
		toggleNo1.isOn = true;
		toggleNo2.isOn = true;
		toggleNo3.isOn = true;
		toggleNo4.isOn = true;
		toggleNo5.isOn = true;
		toggleNo6.isOn = true;

		// 更新
		refreshFlag=true;
	}

	public void ClearNoFilter()
	{
		toggleNo1.isOn = false;
		toggleNo2.isOn = false;
		toggleNo3.isOn = false;
		toggleNo4.isOn = false;
		toggleNo5.isOn = false;
		toggleNo6.isOn = false;

		// 更新
		refreshFlag=true;
	}

	public void MarkAllRuneFilter()
	{
		toggleEnergy.isOn = true;
		toggleFatal.isOn = true;
		toggleBlade.isOn = true;
		toggleRage.isOn = true;
		toggleSwift.isOn = true;
		toggleFocus.isOn = true;
		toggleGuard.isOn = true;
		toggleEndure.isOn = true;
		toggleViolent.isOn = true;
		toggleWill.isOn = true;
		toggleNemesis.isOn = true;
		toggleShield.isOn = true;
		toggleRevenge.isOn = true;
		toggleDespair.isOn = true;
		toggleVampire.isOn = true;

		// 更新
		refreshFlag=true;
	}

	public void ClearRuneFilter()
	{
		toggleEnergy.isOn = false;
		toggleFatal.isOn = false;
		toggleBlade.isOn = false;
		toggleRage.isOn = false;
		toggleSwift.isOn = false;
		toggleFocus.isOn = false;
		toggleGuard.isOn = false;
		toggleEndure.isOn = false;
		toggleViolent.isOn = false;
		toggleWill.isOn = false;
		toggleNemesis.isOn = false;
		toggleShield.isOn = false;
		toggleRevenge.isOn = false;
		toggleDespair.isOn = false;
		toggleVampire.isOn = false;

		// 更新
		refreshFlag=true;
	}

	public void OnSortToral()
	{
		if (sortParam != RuneParam.None) {
			sortParam = RuneParam.None;
			sortOrder = true;
		} else {
			sortOrder = !sortOrder;
		}

		refreshFlag=true;
	}

	public void OnSortHP()
	{
		if (sortParam != RuneParam.HP_Percent) {
			sortParam = RuneParam.HP_Percent;
			sortOrder = true;
		} else {
			sortOrder = !sortOrder;
		}

		refreshFlag=true;
	}

	public void OnSortAtk()
	{
		if (sortParam != RuneParam.Atk_Percent) {
			sortParam = RuneParam.Atk_Percent;
			sortOrder = true;
		} else {
			sortOrder = !sortOrder;
		}

		refreshFlag=true;
	}

	public void OnSortDef()
	{
		if (sortParam != RuneParam.Def_Percent) {
			sortParam = RuneParam.Def_Percent;
			sortOrder = true;
		} else {
			sortOrder = !sortOrder;
		}

		refreshFlag=true;
	}

	public void OnSortSpd()
	{
		if (sortParam != RuneParam.Spd) {
			sortParam = RuneParam.Spd;
			sortOrder = true;
		} else {
			sortOrder = !sortOrder;
		}

		refreshFlag=true;
	}

	public void OnSortCri()
	{
		if (sortParam != RuneParam.Cri) {
			sortParam = RuneParam.Cri;
			sortOrder = true;
		} else {
			sortOrder = !sortOrder;
		}

		refreshFlag=true;
	}

	public void OnSortDmg()
	{
		if (sortParam != RuneParam.CriDmg) {
			sortParam = RuneParam.CriDmg;
			sortOrder = true;
		} else {
			sortOrder = !sortOrder;
		}

		refreshFlag=true;
	}

	public void OnSortAcc()
	{
		if (sortParam != RuneParam.Acc) {
			sortParam = RuneParam.Acc;
			sortOrder = true;
		} else {
			sortOrder = !sortOrder;
		}

		refreshFlag=true;
	}

	public void OnSortRes()
	{
		if (sortParam != RuneParam.Reg) {
			sortParam = RuneParam.Reg;
			sortOrder = true;
		} else {
			sortOrder = !sortOrder;
		}

		refreshFlag=true;
	}


	public void OnPageNext()
	{
		currentPage++;
		refreshFlag=true;
	}

	public void OnPageEnd()
	{
		int maxPage = Mathf.CeilToInt((float)runeCount / PAGE_LIST_NUM) - 1;
		currentPage = maxPage;
		refreshFlag=true;
	}

	public void OnPageBack()
	{
		currentPage--;
		refreshFlag=true;
	}

	public void OnPageFirst()
	{
		currentPage = 0;
		refreshFlag=true;
	}
}
