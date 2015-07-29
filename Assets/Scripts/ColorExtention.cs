using UnityEngine;
using System.Collections;
using System;

public static class ColorExtention {
	/// <summary>
	/// 16進カラーコードを色データに変換して返す
	/// 例) "FF0000" ならColor.redを返す
	/// </summary>
	/// <returns>色データ.</returns>
	/// <param name="colorCode">カラーコード文字列.</param>
	public static UnityEngine.Color toColor(this string colorCode)
	{
		// #は除外する
		colorCode = colorCode.Replace("#", "");
		string r = new string( new char[]{ colorCode[0], colorCode[1] } );
		string g = new string( new char[]{ colorCode[2], colorCode[3] } );
		string b = new string( new char[]{ colorCode[4], colorCode[5] } );

		int _r, _g, _b;
		try {
			// 16進数文字列を変換
			_r = Int32.Parse(r, System.Globalization.NumberStyles.HexNumber);
			_g = Int32.Parse(g, System.Globalization.NumberStyles.HexNumber);
			_b = Int32.Parse(b, System.Globalization.NumberStyles.HexNumber);
		} catch (Exception e)
		{
			Debug.LogError(e.ToString());
			return Color.white;
		}

		return new Color((float)_r/255, (float)_g/255, (float)_b/255);
	}

	/// <summary>
	/// 色データを16進カラーコード文字列に変換して返す
	/// </summary>
	/// <returns>The color code.</returns>
	/// <param name="color">Color.</param>
	public static string toColorCode(this UnityEngine.Color color)
	{
		int r = (int)( color.r * 255 );
		int g = (int)( color.g * 255 );
		int b = (int)( color.b * 255 );

		return Convert.ToString(r, 16) + Convert.ToString(g, 16) + Convert.ToString(b, 16);
	}
}
