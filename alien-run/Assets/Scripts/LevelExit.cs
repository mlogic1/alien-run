using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string NextSceneName;
	public LevelIntroOutroTinter Tint;

	IEnumerator DelayedSceneSwitch()
	{
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene(NextSceneName);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "PlayerFeet")
		{
			// TODO: Disable all controls
			Tint.ShowLevelTint();
			StartCoroutine(DelayedSceneSwitch());
		}
	}
}
