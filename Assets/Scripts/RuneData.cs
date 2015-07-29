using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SW
{
	/// <summary>
	/// ルーンのパラメーターの種類
	/// </summary>
	public enum RuneParam
	{
		/// <summary>
		/// 体力+%
		/// </summary>
		HP_Percent,
		/// <summary>
		/// 攻撃+%
		/// </summary>
		Atk_Percent,
		/// <summary>
		/// 防御+%
		/// </summary>
		Def_Percent,
		/// <summary>
		/// 速度+数値
		/// </summary>
		Spd,
		/// <summary>
		/// クリ率+%
		/// </summary>
		Cri,
		/// <summary>
		/// クリダメ+%
		/// </summary>
		CriDmg,
		/// <summary>
		/// 効果抵抗
		/// </summary>
		Reg,
		/// <summary>
		/// 効果的中
		/// </summary>
		Acc,
		/// <summary>
		/// 体力+数値
		/// </summary>
		HP_Flat,
		/// <summary>
		/// 攻撃+数値
		/// </summary>
		Atk_Flat,
		/// <summary>
		/// 防御+数値
		/// </summary>
		Def_Flat,
		/// <summary>
		/// 空 サブオプションなど、オプションが存在しないことも有る
		/// </summary>
		None,
	}

	/// <summary>
	/// ルーンの種類
	/// </summary>
	public enum RuneType
	{
		/// <summary>元気</summary>
		Energy,
		/// <summary>猛攻</summary>
		Fatal,
		/// <summary>刃</summary>
		Blade,
		/// <summary>激怒</summary>
		Rage,
		/// <summary>迅速</summary>
		Swift,
		/// <summary>集中</summary>
		Focus,
		/// <summary>絶望</summary>
		Despair,
		/// <summary>守護</summary>
		Guard,
		/// <summary>忍耐</summary>
		Endure,
		/// <summary>暴走</summary>
		Violent,
		/// <summary>意思</summary>
		Will,
		/// <summary>果報</summary>
		Nemesis,
		/// <summary>保護</summary>
		Shield,
		/// <summary> 反撃</summary>
		Revenge,
		/// <summary>吸血</summary>
		Vampire,
	}


	/// <summary>
	/// ルーンのオプション一箇所分のデータ
	/// </summary>
	public class RuneOption
	{
		public RuneParam param;
		public int value;

		public void SetParam(RuneParam p) { this.param = p; }
		public void SetValue(int p) { this.value = p; }

		public RuneOption(){ }
		public RuneOption(RuneParam param, int value) {
			this.param = param;
			this.value = value;
		}
	}

	/// <summary>
	/// ルーンのセット構成
	/// 所有者
	/// </summary>
	public class RuneSet
	{
		public string owner;
		public int[] index;

		public RuneSet()
		{
			Reset();
		}

		/// <summary>
		/// ルーンセットを初期化する
		/// </summary>
		public void Reset()
		{
			owner = "";
			index = new int[6];
			// -1で初期化
			for (int i=0; i<6; i++)
			{
				index[i] = -1;
			}
		}

		/// <summary>
		/// ルーンをセットする
		/// 正常にセットできた場合はfalse,セットできなかった場合はtrueを返す
		/// </summary>
		public bool Set(int runeIndex)
		{
			RuneData rune = Data.GetRune(runeIndex);
			if (rune == null) { return true; }// 失敗

			int no = rune.no;

			if (no == 1)
				index[0] = runeIndex;
			else if (no == 2)
				index[1] = runeIndex;
			else if (no == 3)
				index[2] = runeIndex;
			else if (no == 4)
				index[3] = runeIndex;
			else if (no == 5)
				index[4] = runeIndex;
			else if (no == 6)
				index[5] = runeIndex;
			else
				return true;// 異常

			return false;// 正常
		}
	}

	/// <summary>
	/// 所持ルーン一覧
	/// セーブデータはこのクラスをJsonで保存する
	/// </summary>
	public class RuneInventory
	{
		public List<RuneData> runes = new List<RuneData>();
		public List<RuneSet> runeSets = new List<RuneSet>();
		public bool dontCheckDelete;
	}

	/// <summary>
	/// ルーン情報
	/// </summary>
	public class RuneData {

		// ルーンの各種パラメタの評価数値
		public static readonly int HP_PERCENT_VAL			= 962;
		public static readonly int HP_FLAT_VAL				= 9;
		public static readonly int ATK_PERCENT_VAL			= 962;
		public static readonly int ATK_FLAT_VAL				= 127;
		public static readonly int DEF_PERCENT_VAL			= 962;
		public static readonly int DEF_FLAT_VAL				= 127;
		public static readonly int SPD_VAL					= 1443;
		public static readonly int ACC_VAL					= 892;
		public static readonly int RES_VAL					= 806;
		public static readonly int CRI_VAL					= 1047;
		public static readonly int CRIDMG_VAL				= 786;

		/// <summary>
		/// ルーンの種類
		/// </summary>
		public RuneType type;

		/// <summary>
		/// 何番目のルーンか(1~6)
		/// </summary>
		public int no;

		/// <summary>
		/// ルーンのランク（星の数 1~6）
		/// </summary>
		public int rank;

		/// <summary>
		/// ルーンの強化値(0~15)
		/// </summary>
		public int level;

		/// <summary>
		/// メインオプション
		/// </summary>
		public RuneOption mainOption;

		/// <summary>
		/// サブオプション
		/// </summary>
		public RuneOption subOption;

		/// <summary>
		/// ボーナスオプション(最大4つ)
		/// </summary>
		public List<RuneOption> bonusOption;

		/// <summary>
		/// 所持キャラ名 （自由に設定可能）
		/// </summary>
		public string owner;


		/// <summary>
		/// TODO ルーンの価値を数値化して算出する
		/// </summary>
		public int GetValue() { return 0; }

		/// <summary>
		/// TODO ルーン情報に矛盾がないかチェックする
		/// オプションの種類が重複していないか
		/// 1番に防御オプションがついていないか
		/// 3番に攻撃オプションがついていないか
		/// ...など
		/// </summary>
		public bool Validate() { return false; }


		/// <summary>
		/// HP％の数値
		/// </summary>
		public int HP_Percent {
			get {
				int ret = 0;
				RuneParam para = RuneParam.HP_Percent;

				if (mainOption.param==para) ret+=mainOption.value;
				if (subOption.param==para) ret+=subOption.value;
				for (int i=0; i<4; i++)
					if (bonusOption[i].param==para) ret+=bonusOption[i].value;
				return ret;
			}
		}

		/// <summary>
		/// HP(実数)の数値
		/// </summary>
		public int HP_Flat {
			get {
				int ret = 0;
				RuneParam para = RuneParam.HP_Flat;

				if (mainOption.param==para) ret+=mainOption.value;
				if (subOption.param==para) ret+=subOption.value;
				for (int i=0; i<4; i++)
					if (bonusOption[i].param==para) ret+=bonusOption[i].value;
				return ret;
			}
		}

		/// <summary>
		/// 攻撃％の数値
		/// </summary>
		public int ATK_Percent {
			get {
				int ret = 0;
				RuneParam para = RuneParam.Atk_Percent;

				if (mainOption.param==para) ret+=mainOption.value;
				if (subOption.param==para) ret+=subOption.value;
				for (int i=0; i<4; i++)
					if (bonusOption[i].param==para) ret+=bonusOption[i].value;
				return ret;
			}
		}

		/// <summary>
		/// 攻撃（実数）の数値
		/// </summary>
		public int ATK_Flat {
			get {
				int ret = 0;
				RuneParam para = RuneParam.Atk_Flat;

				if (mainOption.param==para) ret+=mainOption.value;
				if (subOption.param==para) ret+=subOption.value;
				for (int i=0; i<4; i++)
					if (bonusOption[i].param==para) ret+=bonusOption[i].value;
				return ret;
			}
		}

		/// <summary>
		/// 防御％の数値
		/// </summary>
		public int DEF_Percent {
			get {
				int ret = 0;
				RuneParam para = RuneParam.Def_Percent;

				if (mainOption.param==para) ret+=mainOption.value;
				if (subOption.param==para) ret+=subOption.value;
				for (int i=0; i<4; i++)
					if (bonusOption[i].param==para) ret+=bonusOption[i].value;
				return ret;
			}
		}

		/// <summary>
		/// 防御（実数）の数値
		/// </summary>
		public int DEF_Flat {
			get {
				int ret = 0;
				RuneParam para = RuneParam.Def_Flat;

				if (mainOption.param==para) ret+=mainOption.value;
				if (subOption.param==para) ret+=subOption.value;
				for (int i=0; i<4; i++)
					if (bonusOption[i].param==para) ret+=bonusOption[i].value;
				return ret;
			}
		}

		/// <summary>
		/// 速度の数値
		/// </summary>
		public int SPD {
			get {
				int ret = 0;
				RuneParam para = RuneParam.Spd;

				if (mainOption.param==para) ret+=mainOption.value;
				if (subOption.param==para) ret+=subOption.value;
				for (int i=0; i<4; i++)
					if (bonusOption[i].param==para) ret+=bonusOption[i].value;
				return ret;
			}
		}

		/// <summary>
		/// 的中の数値
		/// </summary>
		public int ACC {
			get {
				int ret = 0;
				RuneParam para = RuneParam.Acc;

				if (mainOption.param==para) ret+=mainOption.value;
				if (subOption.param==para) ret+=subOption.value;
				for (int i=0; i<4; i++)
					if (bonusOption[i].param==para) ret+=bonusOption[i].value;
				return ret;
			}
		}

		/// <summary>
		/// 抵抗の数値
		/// </summary>
		public int RES {
			get {
				int ret = 0;
				RuneParam para = RuneParam.Reg;

				if (mainOption.param==para) ret+=mainOption.value;
				if (subOption.param==para) ret+=subOption.value;
				for (int i=0; i<4; i++)
					if (bonusOption[i].param==para) ret+=bonusOption[i].value;
				return ret;
			}
		}

		public int CRI {
			get {
				int ret = 0;
				RuneParam para = RuneParam.Cri;

				if (mainOption.param==para) ret+=mainOption.value;
				if (subOption.param==para) ret+=subOption.value;
				for (int i=0; i<4; i++)
					if (bonusOption[i].param==para) ret+=bonusOption[i].value;
				return ret;
			}
		}

		public int CRIDMG {
			get {
				int ret = 0;
				RuneParam para = RuneParam.CriDmg;

				if (mainOption.param==para) ret+=mainOption.value;
				if (subOption.param==para) ret+=subOption.value;
				for (int i=0; i<4; i++)
					if (bonusOption[i].param==para) ret+=bonusOption[i].value;
				return ret;
			}
		}

		/// <summary>
		/// ルーンの強さを総合評価した数値
		/// </summary>
		public int TotalValue {
			get {
				int val = 0;
				val += ATK_Percent * ATK_PERCENT_VAL;
				val += ATK_Flat * ATK_FLAT_VAL;
				val += HP_Percent * HP_PERCENT_VAL;
				val += HP_Flat * HP_FLAT_VAL;
				val += DEF_Percent * DEF_PERCENT_VAL;
				val += DEF_Flat * DEF_FLAT_VAL;
				val += SPD * SPD_VAL;
				val += ACC * ACC_VAL;
				val += RES * RES_VAL;
				val += CRI * CRI_VAL;
				val += CRIDMG * CRIDMG_VAL;

				return val;
			}
		}

		public RuneData()
		{
			type = RuneType.Energy;
			no = 1;
			rank = 1;
			level = 0;
			mainOption = new RuneOption(RuneParam.None, 0);
			subOption =  new RuneOption(RuneParam.None, 0);
			bonusOption = new List<RuneOption>();
			bonusOption.Add(new RuneOption(RuneParam.None, 0));
			bonusOption.Add(new RuneOption(RuneParam.None, 0));
			bonusOption.Add(new RuneOption(RuneParam.None, 0));
			bonusOption.Add(new RuneOption(RuneParam.None, 0));
		}

		/// <summary>
		/// ルーンの情報を画像から解析する
		/// 解析失敗時はnullを返す
		/// </summary>
		public static RuneData ParseFronTexture(Texture2D tex)
		{
			RuneData ret = new RuneData();

			// TODO テンプレートマッチングする

			return ret;
		}

		public override bool Equals (object obj)
		{
			RuneData rune = (RuneData)obj;
			if (rune == null) return false;

			// 検証
			if (this.owner != rune.owner) return false;
			if (this.type != rune.type) return false;
			if (this.rank != rune.rank) return false;
			if (this.no != rune.no) return false;
			if (this.level != rune.level) return false;
			if (!this.mainOption.Equals(rune.mainOption)) return false;
			if (!this.subOption.Equals(rune.subOption)) return false;
			if (this.bonusOption.Count != rune.bonusOption.Count) return false;
			if (this.bonusOption.Count >=1 && !this.bonusOption[0].Equals(rune.bonusOption[0])) return false;
			if (this.bonusOption.Count >=2 && !this.bonusOption[1].Equals(rune.bonusOption[1])) return false;
			if (this.bonusOption.Count >=3 && !this.bonusOption[2].Equals(rune.bonusOption[2])) return false;
			if (this.bonusOption.Count >=4 && !this.bonusOption[3].Equals(rune.bonusOption[3])) return false;

			return true;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}

		public RuneData CopyTo()
		{
			RuneData ret = new RuneData();
			ret.type = this.type;
			ret.owner = this.owner;
			ret.no = this.no;
			ret.level = this.level;
			ret.rank = this.rank;
			ret.mainOption = this.mainOption;
			ret.subOption = this.subOption;
			ret.bonusOption = this.bonusOption;
			return ret;
		}
	}
}