using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OKCancelDialog : MonoBehaviour {

	public Text message;
	public Toggle toggle;
	public System.Action<bool> onOK;
	public System.Action<bool> onCancel;
	public bool isClose;

	/// <summary>
	/// ダイアログ表示
	/// メッセージカラーは通常黒(Color.Black)
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="color">Color.</param>
	/// <param name="onOK">On O.</param>
	public static OKCancelDialog Create(string message, Color color, System.Action<bool> onOK, System.Action<bool> onCancel)
	{
		GameObject go = Instantiate(Resources.Load<GameObject>("Prefub/OKCancelDialog")) as GameObject;
		OKCancelDialog dialog = go.GetComponent<OKCancelDialog>();

		if (dialog != null)
		{
			dialog.transform.localScale = Vector3.one;
			dialog.transform.localPosition = Vector3.zero;
			dialog.message.color = color;
			dialog.message.text = message;
			dialog.onOK = onOK;
			dialog.onCancel = onCancel;
			dialog.toggle.gameObject.SetActive(false);// トグルは表示しない
		}

		return dialog;
	}

	public static OKCancelDialog CreateWithToggle(string message, Color color, System.Action<bool> onOK, System.Action<bool> onCancel)
	{
		GameObject go = Instantiate(Resources.Load<GameObject>("Prefub/OKCancelDialog")) as GameObject;
		OKCancelDialog dialog = go.GetComponent<OKCancelDialog>();

		if (dialog != null)
		{
			dialog.transform.localScale = Vector3.one;
			dialog.transform.localPosition = Vector3.zero;
			dialog.message.color = color;
			dialog.message.text = message;
			dialog.onOK = onOK;
			dialog.onCancel = onCancel;
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
		if (onOK != null) { onOK(toggle.isOn); }
		Close();
	}

	public void OnCancelButton()
	{
		if (onCancel != null) { onCancel(toggle.isOn); }
		Close();
	}
}