// NULLcode Studio © 2016
// null-code.ru

using UnityEngine;
using System.Collections;

public class MainSelect : MonoBehaviour {

	public LayerMask layerMask; // слои с которыми требуется взаимодействие
	public GameObject _hover; // указатель, если объект под курсором
	public GameObject _select; // указатель, если сделан клик по объекту

	private static GameObject curObj, hoverObj, selectObj;
	private int obj_id, last_id;
	private LayerMask layerMaskInvert;

	void Awake()
	{
		// дополнительная маска нужна, чтобы рейкаст игнорировал выбранные слои и слой по умолчанию Ignore Raycast
		layerMaskInvert = layerMask.value | 1 << 2; // копируем выбранные слои и добавляем к ним еще один
		layerMaskInvert = ~layerMaskInvert; // инвертируем маску
	}

	public static GameObject current
	{
		get { return curObj; }
	}

	void Select() // выбор объекта
	{
		GameObject obj = GetObject();
		if(obj == null) // сброс
		{
			Destroy(selectObj);
			Destroy(hoverObj);
			obj_id = 0;
			curObj = null;
			selectObj = null;
			hoverObj = null;
			return;
		}
		if(!selectObj) selectObj = Instantiate(_select) as GameObject;
		selectObj.transform.parent = obj.transform;
		selectObj.transform.position = GetPosition(obj.transform.position, layerMaskInvert);
		curObj = obj;
		Destroy(hoverObj);
		hoverObj = null;
	}

	void Hover() // вешаем указатель на объект
	{
		GameObject obj = GetObject();
		if(last_id != obj_id && obj)
		{
			if(!hoverObj) hoverObj = Instantiate(_hover) as GameObject;
			hoverObj.transform.position = GetPosition(obj.transform.position, layerMaskInvert);
			hoverObj.transform.parent = obj.transform;
		}
		last_id = obj_id;
	}

	GameObject GetObject() // получаем объект и его хеш код
	{
		GameObject obj = null;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			obj_id = hit.transform.gameObject.GetHashCode();
			obj = hit.transform.gameObject;
		}
		return obj;
	}

	Vector3 GetPosition(Vector3 position, LayerMask layers) // находим поверхность под объектом
	{
		Vector3 result = position;
		RaycastHit hit;
		Ray ray = new Ray(position, Vector3.down);
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, layers))
		{
			result = hit.point + hit.normal * 0.05f;
		}
		return result;
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Select();
		}
		else
		{
			Hover();
		}
	}
}
