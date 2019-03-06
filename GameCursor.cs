using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class GameCursor : MonoBehaviour {

	[SerializeField] private Image cursor;
	private enum Mode {Project3D, Project2D};
	[SerializeField] private Mode mode;
	[SerializeField] private Cursors[] option;
	[SerializeField] private Sprite defaultCursor;
	private static bool visible = true;
	private RectTransform rect;

	[System.Serializable] struct Cursors
	{
		public Sprite sprite;
		public LayerMask layerMask;
	}

	public static void Visible(bool value)
	{
		visible = value;
	}

	void Awake()
	{
		rect = cursor.GetComponent<RectTransform>();
		cursor.raycastTarget = false;
		Cursor.visible = false;
	}

	void LateUpdate()
	{
		if(visible)
		{
			Raycast();

			rect.pivot = new Vector2(cursor.sprite.pivot.x / cursor.sprite.texture.width, cursor.sprite.pivot.y / cursor.sprite.texture.height);
			rect.position = Input.mousePosition;
		}

		if(!visible && cursor.gameObject.activeSelf) cursor.gameObject.SetActive(false);
		else if(visible && !cursor.gameObject.activeSelf) cursor.gameObject.SetActive(true);
	}

	Sprite SetCursor(int layer)
	{
		for(int i = 0; i < option.Length; i++)
		{
			if(((1 << layer) & option[i].layerMask) != 0)
			{
				return option[i].sprite;
			}
		}

		return defaultCursor;
	}

	void Raycast()
	{
		if(mode == Mode.Project3D)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out hit))
			{
				cursor.sprite = SetCursor(hit.collider.gameObject.layer);
			}
			else
			{
				cursor.sprite = SetCursor(-1);
			}
		}
		else
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if(hit.transform != null)
			{
				cursor.sprite = SetCursor(hit.collider.gameObject.layer);
			}
			else
			{
				cursor.sprite = SetCursor(-1);
			}
		}
	}
}
