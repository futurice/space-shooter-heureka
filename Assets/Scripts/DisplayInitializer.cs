using UnityEngine;
using System.Collections;


public enum ScreenResolution {
	Res1920x1080,
	Res3840x1080,
	Res1920x2160,
	ResDefault
}

public class DisplayInitializer : MonoBehaviour {

	[SerializeField]
	ScreenResolution _resolution = ScreenResolution.ResDefault;

	Vector2 getScreenRes(ScreenResolution res) {
		switch (res) {
		case ScreenResolution.Res1920x1080: return new Vector2(1920,1080);
		case ScreenResolution.Res3840x1080: return new Vector2(3840,1080);
		case ScreenResolution.Res1920x2160: return new Vector2(1920,2160);
		default: return getScreenRes(ScreenResolution.Res1920x2160);
		}
	}

	// Use this for initialization
	void Start () {
		Vector2 res = getScreenRes(_resolution);
		Debug.Log(string.Format("Setting screen resolution to {0}, {1}", res.x, res.y));
		Screen.SetResolution((int)res.x, (int)res.y, false);
		#if !UNITY_EDITOR
		this.gameObject.GetComponent<Funnel.Funnel>().enabled=true;
		#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
