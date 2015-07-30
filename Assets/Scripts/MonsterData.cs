using UnityEngine;
using System.Collections;

namespace SW {

	public enum Element
	{
		NONE,
		FIRE,
		WATER,
		WIND,
		LIGHT,
		DARK,
	}

	/// <summary>
	/// モンスターのデータ
	/// </summary>
	[System.Serializable]
	public class MonsterData {
		public string j_name;// 日本語名
		public int id;
		public string name;

		public string race;
		public int rarity;
		public Element element;
		public int level;
		public int b_hp;
		public int b_atk;
		public int b_def;
		public int b_spd;
		public int b_crate;
		public int b_cdmg;
		public int b_res;
		public int b_acc;
	}
}