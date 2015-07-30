using UnityEngine;
using System.Collections;

namespace SW {
	public static class StringUtil {

		/// <summary>
		/// 日本名を返す
		/// </summary>
		public static string ToJpnString(this RuneType type)
		{
			switch (type)
			{
			case RuneType.Blade: return "刃";
			case RuneType.Despair: return "絶望";
			case RuneType.Endure: return "忍耐";
			case RuneType.Energy: return "元気";
			case RuneType.Fatal: return "猛攻";
			case RuneType.Focus: return "集中";
			case RuneType.Guard: return "守護";
			case RuneType.Nemesis: return "果報";
			case RuneType.Rage: return "激怒";
			case RuneType.Revenge: return "反撃";
			case RuneType.Shield: return "保護";
			case RuneType.Swift: return "迅速";
			case RuneType.Vampire: return "吸血";
			case RuneType.Violent: return "暴走";
			case RuneType.Will: return "意思";
			default: return "元気";
			}
		}

		/// <summary>
		/// 文字列からルーンタイプを解析
		/// </summary>
		public static RuneType ParseToType(this string str)
		{
			if (string.IsNullOrEmpty(str)) return RuneType.Energy;

			string str_ignoreSpace = str.Replace(" ", "");

			switch (str_ignoreSpace)
			{
			case "刃": return RuneType.Blade;
			case "絶望": return RuneType.Despair;
			case "忍耐": return RuneType.Endure;
			case "元気": return RuneType.Energy;
			case "猛攻": return RuneType.Fatal;
			case "集中": return RuneType.Focus;
			case "守護": return RuneType.Guard;
			case "果報": return RuneType.Nemesis;
			case "激怒": return RuneType.Rage;
			case "反撃": return RuneType.Revenge;
			case "保護": return RuneType.Shield;
			case "迅速": return RuneType.Swift;
			case "吸血": return RuneType.Vampire;
			case "暴走": return RuneType.Violent;
			case "意思": return RuneType.Will;
			default: return RuneType.Energy;
			}
		}

		/// <summary>
		/// 日本語を返す
		/// </summary>
		public static string ToJpnString(this RuneParam para)
		{
			switch (para)
			{
			case RuneParam.Acc: return "効果的中";
			case RuneParam.Atk_Flat: return "攻撃";
			case RuneParam.Atk_Percent: return "攻撃%";
			case RuneParam.Cri: return "クリ率";
			case RuneParam.CriDmg: return "クリダメ";
			case RuneParam.Def_Flat: return "防御";
			case RuneParam.Def_Percent: return "防御%";
			case RuneParam.HP_Flat: return "体力";
			case RuneParam.HP_Percent: return "体力%";
			case RuneParam.None: return "なし";
			case RuneParam.Reg: return "効果抵抗";
			case RuneParam.Spd: return "速度";
			default: return "なし";
			}
		}

		/// <summary>
		/// 文字列からパラメータ種類を解析
		/// </summary>
		public static RuneParam ParseToParam(this string str)
		{
			if (string.IsNullOrEmpty(str)) return RuneParam.None;

			string str_ignoreSpace = str.Replace(" ", "");

			switch (str_ignoreSpace)
			{
			case "効果的中": return RuneParam.Acc;
			case "攻撃": return RuneParam.Atk_Flat;
			case "攻撃％": return RuneParam.Atk_Percent;
			case "クリ率": return RuneParam.Cri;
			case "クリダメ": return RuneParam.CriDmg;
			case "防御": return RuneParam.Def_Flat;
			case "防御％": return RuneParam.Def_Percent;
			case "体力": return RuneParam.HP_Flat;
			case "体力％": return RuneParam.HP_Percent;
			case "なし": return RuneParam.None;
			case "効果抵抗": return RuneParam.Reg;
			case "速度": return RuneParam.Spd;
			default: return RuneParam.None;
			}
		}
	}
}