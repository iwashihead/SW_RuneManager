using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogCanvas : MonoBehaviour {

	public Text message;
	public System.Action onOK;
	public bool isClose;

	/// <summary>
	/// ダイアログ表示
	/// メッセージカラーは通常黒(Color.Black)
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="color">Color.</param>
	/// <param name="onOK">On O.</param>
	public static DialogCanvas Create(string message, Color color, System.Action onOK)
	{
		GameObject go = Instantiate(Resources.Load<GameObject>("Prefub/DialogCanvas")) as GameObject;
		DialogCanvas dialog = go.GetComponent<DialogCanvas>();

		if (dialog != null)
		{
			dialog.transform.localScale = Vector3.one;
			dialog.transform.localPosition = Vector3.zero;
			dialog.message.color = color;
			dialog.message.text = message;
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
		if (onOK != null) { onOK(); }
		Close();
	}
}
