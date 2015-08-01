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

		public MonsterData Clone()
		{
			MonsterData clone = new MonsterData();
			clone.j_name = j_name;
			clone.id = id;
			clone.name = name;
			clone.race = race;
			clone.rarity = rarity;
			clone.element = element;
			clone.level = level;
			clone.b_hp = b_hp;
			clone.b_atk = b_atk;
			clone.b_def = b_def;
			clone.b_spd = b_spd;
			clone.b_crate = b_crate;
			clone.b_cdmg = b_cdmg;
			clone.b_res = b_res;
			clone.b_acc = b_acc;

			return clone;
		}
	}
}