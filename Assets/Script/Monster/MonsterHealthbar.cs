using UnityEngine;
using System.Collections;

public class MonsterHealthbar : MonoBehaviour {
	
	private bool _isHealthbarEnabled = false;
	private Vector3 _healthBarPosition;
	private MonsterProfile _MonsterProfile;
	private float _ratio;
	private TextureManager _TextureManager;
	
	public bool IsHealthbarEnabled
	{
		get {return _isHealthbarEnabled; }
		set {_isHealthbarEnabled = value; }
	}
	
	// Use this for initialization
	void Start () {
		_MonsterProfile = GetComponent<MonsterProfile>();
		_TextureManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>();
	}
	
	void OnGUI()
	{
		//Vector3 tmpPos = transform.position;
		float offsetX = -0.1f;
		float offsetY = 0.1f;
		if(_isHealthbarEnabled)
		{
			_ratio = (float)((float)_MonsterProfile.CurHp/(float)_MonsterProfile.MaxHp);
			//Debug.Log (_ratio);
			_healthBarPosition = Camera.main.WorldToViewportPoint (transform.position);	
			if(_healthBarPosition.z >0.0f)
			{
				GUI.DrawTexture(new Rect((_healthBarPosition.x+offsetX)*Screen.width,  Screen.height-((_healthBarPosition.y+offsetY)*Screen.height) , 150, 25),_TextureManager.Texture_ProgBackground);
				GUI.DrawTexture(new Rect((_healthBarPosition.x+offsetX)*Screen.width,  Screen.height-((_healthBarPosition.y+offsetY)*Screen.height) , 150*_ratio, 25),_TextureManager.Texture_ProgHealtBar);	
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		
	}
}
