using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UIFX : MonoBehaviour
{
	[SerializeField]
	private GameObject effectPrefab;
	private GameObject effectGO;
	private RenderTexture renderTexture;
	private Camera rtCamera;
	private RawImage rawImage;

    private static float pos =0;

	void Awake()
	{
		//RawImage可以手动添加，设置no alpha材质，以显示带透明的粒子
		rawImage = gameObject.GetComponent<RawImage>();
		if (rawImage == null)
		{
			rawImage = gameObject.AddComponent<RawImage>();
		}
	}

	public RectTransform rectTransform
	{
		get
		{
			return transform as RectTransform;
		}
	}

	void OnEnable()
	{
		if (effectPrefab != null)
		{
			effectGO = Instantiate(effectPrefab);
		    Transform[] transforms = effectGO.GetComponentsInChildren<Transform>();
		    foreach (Transform tran in transforms)
		    {
		        tran.gameObject.layer = 8;
		    }

//		    rectTransform.sizeDelta = new Vector2(100,100);

			renderTexture = new RenderTexture((int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y, 24);
			renderTexture.antiAliasing = 4;

			GameObject cameraObj = new GameObject("UIEffectCamera");
		    Vector3 posi = cameraObj.transform.position;
		    posi.z = pos;
		    cameraObj.transform.position = posi;
		    rtCamera = cameraObj.AddComponent<Camera>();
			rtCamera.clearFlags = CameraClearFlags.SolidColor;
			rtCamera.backgroundColor = new Color();
			rtCamera.cullingMask = 1 << 8;
		    rtCamera.orthographic = true;
		    rtCamera.nearClipPlane = -1;
		    rtCamera.farClipPlane = 1f;
		    rtCamera.orthographicSize = 5f* rectTransform.sizeDelta.y / 720;
			rtCamera.targetTexture = renderTexture;

			effectGO.transform.SetParent(cameraObj.transform, false);

		    pos -= 2;

			rawImage.enabled = true;
			rawImage.texture = renderTexture;
		}
		else
		{
			rawImage.texture = null;
			Debug.LogError("EffectPrefab can't be null");
		}
	}

	void OnDisable()
	{
		if (effectGO != null)
		{
			Destroy(effectGO);
			effectGO = null;
		}
		if (rtCamera != null)
		{
			Destroy(rtCamera.gameObject);
			rtCamera = null;
		}
		if (renderTexture != null)
		{
			Destroy(renderTexture);
			renderTexture = null;
		}
		rawImage.enabled = false;
	}
}
