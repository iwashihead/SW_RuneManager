using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ValueChangeDialog : MonoBehaviour {

	public Text message;
	public InputField input;
	public System.Action<string> onOK;
	public bool isClose;

	/// <summary>
	/// ダイアログ表示
	/// メッセージカラーは通常黒(Color.Black)
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="color">Color.</param>
	/// <param name="onOK">On O.</param>
	public static ValueChangeDialog Create(string message, Color color, System.Action<string> onOK, System.Action<string> onEndEdit)
	{
		GameObject go = Instantiate(Resources.Load<GameObject>("Prefub/DialogCanvas")) as GameObject;
		ValueChangeDialog dialog = go.GetComponent<ValueChangeDialog>();

		if (dialog != null)
		{
			// 入力値のValidate関数も渡しておく
			if (onEndEdit != null) {
				dialog.input.onEndEdit.AddListener(val=>{ onEndEdit(val); });
			}
			dialog.transform.localScale = Vector3.one;
			dialog.transform.localPosition = Vector3.zero;
			dialog.message.color = color;
			dialog.message.text = message;
			dialog.onOK = onOK;
		}

		return dialog;
	}

	public static ValueChangeDialog CreateBigDialog(string title, string text, System.Action<string> onOK, System.Action<string> onEndEdit)
	{
		GameObject go = Instantiate(Resources.Load<GameObject>("Prefub/ImportDialog")) as GameObject;
		ValueChangeDialog dialog = go.GetComponent<ValueChangeDialog>();

		if (dialog != null)
		{
			// 入力値のValidate関数も渡しておく
			if (onEndEdit != null) {
				dialog.input.onEndEdit.AddListener(val=>{ onEndEdit(val); });
			}
			dialog.message.text = title==null ? "" : title;
			dialog.input.text = text==null ? "" : text;
			dialog.transform.localScale = Vector3.one;
			dialog.transform.localPosition = Vector3.zero;
			dialog.onOK = onOK;
		}

		return dialog;
	}

	public void Close()
	{
		StartCoroutine(CloseCoroutine());
	}

	IEnumerator CloseCoroutine()
	{
		if (isClose == true) yield break;
		isClose = true;
		yield return null;
		//		while (transform.localScale.x > 0.05f) {
		//			transform.localScale = transform.localScale * 0.76f;
		//			yield return null;
		//		}
		DestroyImmediate(this.gameObject);
	}

	public void OnOKButton()
	{
		// inputFieldに入力された値を渡してコールバックを呼び出す
		if (onOK != null) { onOK( input.text ); }
		Close();
	}
}
