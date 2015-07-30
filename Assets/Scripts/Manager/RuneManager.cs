using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// ルーン管理メインクラス
/// </summary>
public partial class RuneManager : SingletonObject<RuneManager> {
	
	void Awake()
	{
		AddRuneAwake();
		RuneListAwake();
	}

	void Start()
	{
		AddRuneDisable();
		RuneListInitialize();
	}

	void LateUpdate()
	{
		if (refreshFlag) Refresh();
	}
}
