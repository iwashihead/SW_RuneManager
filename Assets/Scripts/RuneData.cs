using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SW
{
	public struct Status
	{
		public static Status One = new Status(1,1,1,1,1,1,1,1);
		public int hp;
		public int atk;
		public int def;
		public int spd;
		public int cri;
		public int cdmg;
		public int res;
		public int acc;

		public Status (int hp, int atk, int def, int spd, int cri, int cdmg, int res, int acc )
		{
			this.hp = hp;
			this.atk = atk;
			this.def = def;
			this.cri = cri;
			this.spd = spd;
			this.cdmg = cdmg;
			this.res = res;
			this.acc = acc;
		}
	}

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
		public int key;
		public string owner;
		public RuneData[] runes;

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
			runes = new RuneData[6];
		}

		/// <summary>
		/// 攻撃指数を取得する
		/// </summary>
		public int GetAttackPoint(int baseAtk, int baseCri, int baseCriDmg)
		{
			int atk = GetTotalATK(baseAtk);
			int cri = GetTotalCRI(baseCri);
			int cdmg = GetTotalCRIDMG(baseCriDmg);

			float nonCriDmg = atk * (1f - (float)cri/100);
			float criDmg = atk * (1f+cdmg/100) * (float)(cri)/100;

			return Mathf.CeilToInt((nonCriDmg + criDmg)*10f);
		}

		public int GetDefencePoint(int baseHp, int baseDef)
		{
			return Mathf.CeilToInt( GetTotalHP(baseHp) * GetTotalDEF(baseDef) * 0.0008f );
		}

		public int GetTotalValuePoint(Status status, Status weight)
		{
			int atk = GetAttackPoint(status.atk, status.cri, status.cdmg);
			atk += Mathf.CeilToInt(GetTotalATK(status.atk) * (weight.atk-1) * 0.6f);
			atk += GetTotalCRI(status.cri) * (weight.cri-1) * 60;
			atk += GetTotalCRIDMG(status.cdmg) * (weight.cdmg-1) * 30;
			int def = GetDefencePoint(status.hp, status.def) * weight.hp * weight.def;
			int spd = GetTotalSPD(status.spd) * 35 * weight.spd;
			int res = GetTotalRES(status.res) * 50 * weight.res;
			int acc = GetTotalACC(status.acc) * 50 * weight.acc;

			int total = atk+def+spd+res+acc;

			return total;
		}

		public int GetTotalATK(int baseVal)
		{
			int percent = 0;
			int flat = 0;
			foreach (RuneData rune in runes)
			{
				if (rune==null) continue;
				percent += rune.ATK_Percent;
				flat += rune.ATK_Flat;
			}
			return Mathf.CeilToInt(baseVal * (1f + (float)percent / 100) + flat);
		}

		public int GetTotalHP(int baseVal)
		{
			int percent = 0;
			int flat = 0;
			foreach (RuneData rune in runes)
			{
				if (rune==null) continue;
				percent += rune.HP_Percent;
				flat += rune.HP_Flat;
			}
			return Mathf.CeilToInt(baseVal * (1f + (float)percent / 100) + flat);
		}

		public int GetTotalDEF(int baseVal)
		{
			int percent = 0;
			int flat = 0;
			foreach (RuneData rune in runes)
			{
				if (rune==null) continue;
				percent += rune.DEF_Percent;
				flat += rune.DEF_Flat;
			}
			return Mathf.CeilToInt(baseVal * (1f + (float)percent / 100) + flat);
		}

		public int GetTotalSPD(int baseVal)
		{
			int ret = baseVal;
			foreach (RuneData rune in runes)
			{
				if (rune==null) continue;
				ret += rune.SPD;
			}
			return ret;
		}

		public int GetTotalCRI(int baseVal)
		{
			int ret = baseVal;
			foreach (RuneData rune in runes)
			{
				if (rune==null) continue;
				ret += rune.CRI;
			}
			return Mathf.Clamp(ret, 0, 100);
		}

		public int GetTotalCRIDMG(int baseVal)
		{
			int ret = baseVal;
			foreach (RuneData rune in runes)
			{
				if (rune==null) continue;
				ret += rune.CRIDMG;
			}
			return ret;
		}

		public int GetTotalRES(int baseVal)
		{
			int ret = baseVal;
			foreach (RuneData rune in runes)
			{
				if (rune==null) continue;
				ret += rune.RES;
			}
			return Mathf.Clamp(ret, 0, 100);
		}

		public int GetTotalACC(int baseVal)
		{
			int ret = baseVal;
			foreach (RuneData rune in runes)
			{
				if (rune==null) continue;
				ret += rune.ACC;
			}
			return Mathf.Clamp(ret, 0, 100);
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
		/// 一意になるように設定されたキー
		/// </summary>
		public int key;

		/// <summary>
		/// TODO ルーンの価値を数値化して算出する
		/// </summary>
		public int GetValue() {
			return GetValue(Status.One);
		}

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

		/// <summary>
		/// 各種ステータスに任意の価値の重み付けをして数値化した価値データを返す
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="hp_w">Hp w.</param>
		/// <param name="atk_w">Atk w.</param>
		/// <param name="def_w">Def w.</param>
		/// <param name="spd_w">Spd w.</param>
		/// <param name="cri_w">Cri w.</param>
		/// <param name="dmg_w">Dmg w.</param>
		/// <param name="res_w">Res w.</param>
		/// <param name="acc_w">Acc w.</param>
		public int GetValue(Status weight)
		{
			float val = 0f;
			val += ATK_Percent * ATK_PERCENT_VAL * weight.atk;
			val += ATK_Flat * ATK_FLAT_VAL * weight.atk;
			val += HP_Percent * HP_PERCENT_VAL * weight.hp;
			val += HP_Flat * HP_FLAT_VAL * weight.hp;
			val += DEF_Percent * DEF_PERCENT_VAL * weight.def;
			val += DEF_Flat * DEF_FLAT_VAL * weight.def;
			val += SPD * SPD_VAL * weight.spd;
			val += ACC * ACC_VAL * weight.acc;
			val += RES * RES_VAL * weight.res;
			val += CRI * CRI_VAL * weight.cri;
			val += CRIDMG * CRIDMG_VAL * weight.cdmg;

			return (int)val;
		}

		public RuneData()
		{
			key = -1;
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
		/// 空のルーン
		/// </summary>
		public static readonly RuneData BlankRune = new RuneData();

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