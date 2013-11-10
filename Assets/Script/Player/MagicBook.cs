using UnityEngine;
using System.Collections;

static class MagicBook {
	
	static private Spell _ActiveSpell = Character.SpellList[(int)SpellName.IceBolt];
	static PrefabManager _PrefabManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
	static GameObject _Player       = GameObject.FindGameObjectWithTag("Player");
	
	private static float _elapsedTimeLastSpell;
	private static float _lastSpellCast;
	private static string _compToLevelOnSpell = "Damage";
	
	static public Spell ActiveSpell
	{
		get {return _ActiveSpell; }
		set {_ActiveSpell = value; }
	}
	
	static public string CompToLevelOnSpell
	{
		get {return _compToLevelOnSpell; }
		set {_compToLevelOnSpell = value; }
	}
	
	static public void CastSpell(Spell _SpellCast)
	{
		_elapsedTimeLastSpell = Time.time - _lastSpellCast;
		
		if(_elapsedTimeLastSpell >= _SpellCast.Cd && Character.CurMP >= _SpellCast.Mana)
		{
			_lastSpellCast = Time.time;
			_elapsedTimeLastSpell =- _SpellCast.Cd;
			UpdateSpellStats(); //Probably not necessary if well implemented, there for precaution
			CastAnimation(_SpellCast);
			Character.LoseMp(_SpellCast.Mana);
		}
	}
	
	static private void CastAnimation(Spell _SpellToAnimate)
	{
		if(_SpellToAnimate == Character.SpellList[(int)SpellName.IceBolt])
		{
			CastIceBoltAnimation();
		}
		else if(_SpellToAnimate == Character.SpellList[(int)SpellName.FireBat])
		{
			CastFireBatAnimation();
		}
		
	}
	
	static private void CastIceBoltAnimation()
	{
		GameObject _GO_Spell;
		float _distanceFromCamera = 1.5f;
		
		Vector3 SpawnPosition = Camera.mainCamera.transform.position + Camera.mainCamera.transform.forward*_distanceFromCamera;
		Quaternion SpawnQuaternion = Camera.main.transform.rotation;
			
		GameObject.Instantiate (_PrefabManager.Spell_IceBolt, SpawnPosition, SpawnQuaternion);
		
	}
	
	static private void CastFireBatAnimation()
	{
		GameObject _GO_Spell;
		float _distanceFromCamera = 1.5f;
		
		Vector3 SpawnPosition = Camera.mainCamera.transform.position + Camera.mainCamera.transform.forward*_distanceFromCamera;
		Quaternion SpawnQuaternion = Camera.main.transform.rotation;
			
			
		_GO_Spell = GameObject.Instantiate (_PrefabManager.Spell_FireBat, SpawnPosition, SpawnQuaternion) as GameObject;
		
		_GO_Spell.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	static public void UpdateSpellStats()
	{
		for(int i = 0; i < Character.SpellList.Length; i++)
		{
			
			// Update damage according to category
			if(Character.SpellList[i].Category == "Ice")
			{
				Character.SpellList[i].Damage = Character.SpellList[i].DamageBase  + (int)Mathf.Round(Character.SpellList[i].DamageLevel * Character.SpellList[i].DamagePerLevel)     + Character.SkillList[(int)SkillName.IceMage].Level*2;
			}
			else if(Character.SpellList[i].Category == "Fire")
			{
				Character.SpellList[i].Damage = Character.SpellList[i].DamageBase  + (int)Mathf.Round(Character.SpellList[i].DamageLevel * Character.SpellList[i].DamagePerLevel)     + Character.SkillList[(int)SkillName.FireMage].Level*2;
			}
			else
			{
				Character.SpellList[i].Damage = Character.SpellList[i].DamageBase  + (int)Mathf.Round(Character.SpellList[i].DamageLevel * Character.SpellList[i].DamagePerLevel);	
			}
			
			
			Character.SpellList[i].Cd     = Character.SpellList[i].CdBase      +                  Character.SpellList[i].CdLevel     * Character.SpellList[i].CdPerLevel   ;
			Character.SpellList[i].Range  = Character.SpellList[i].RangeBase   +                  Character.SpellList[i].RangeLevel  * Character.SpellList[i].RangePerLevel;
			Character.SpellList[i].Mana   = Character.SpellList[i].ManaBase    + (int)Mathf.Round(Character.SpellList[i].ManaLevel   * Character.SpellList[i].ManaPerLevel);
		}
	}
	
	static public void RewardSpell(Spell _SpellProc, float _Reward)
	{
		if(_SpellProc.Category == "Ice")
		{
			Character.GiveExpToSkill(Character.SkillList[(int)SkillName.IceMage],_Reward/Mathf.Pow (Character.SkillList[(int)SkillName.IceMage].Level,1.50f));
		}
		else if(_SpellProc.Category == "Fire")
		{
			Character.GiveExpToSkill(Character.SkillList[(int)SkillName.FireMage],_Reward/Mathf.Pow (Character.SkillList[(int)SkillName.FireMage].Level,1.50f));
		}
		
		switch(_compToLevelOnSpell)
		{
			case "Damage":	
				_SpellProc.DamageCurExp += _Reward/Mathf.Pow(_SpellProc.DamageLevel + 1 ,2);
				if(_SpellProc.DamageCurExp >= 100.0f) {_SpellProc.DamageCurExp -= 100.0f; _SpellProc.DamageLevel++;}
				break;
			case "Cd":	
				_SpellProc.CdCurExp += _Reward/Mathf.Pow(_SpellProc.CdLevel + 1,2);
				if(_SpellProc.CdCurExp >= 100.0f) {_SpellProc.CdCurExp -= 100.0f; _SpellProc.CdLevel++;}
				break;
			case "Mana":	
				_SpellProc.ManaCurExp += _Reward/Mathf.Pow(_SpellProc.ManaLevel + 1,2);
				if(_SpellProc.ManaCurExp >= 100.0f) {_SpellProc.ManaCurExp -= 100.0f; _SpellProc.ManaLevel++;}
				break;
		}
		UpdateSpellStats();
	}
}
