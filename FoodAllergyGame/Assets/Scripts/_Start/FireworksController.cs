using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireworksController : MonoBehaviour {

	public List<GameObject> fireworksList;
	public Rect spawnRect;
	public float fireworksDuration = 4f;
	public float fireworksIntervalMin = 0.5f;
	public float fireworksIntervalMax = 2f;

	public void StartFireworks() {
		StartCoroutine("FireworksEngineRun");
		Invoke("FinishFireworks", fireworksDuration);
	}

	public void FinishFireworks() {
		StopCoroutine("FireworksEngineRun");
	}

	private IEnumerator FireworksEngineRun() {
		while(true) {
			// Fire off one firework
			GameObject fireworkPrefab = fireworksList[UnityEngine.Random.Range(0, fireworksList.Count)];
			GameObject fireworkObj = GameObjectUtils.AddChild(gameObject, fireworkPrefab);

			// Random position
			Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(spawnRect.xMin, spawnRect.xMax),
												UnityEngine.Random.Range(spawnRect.yMin, spawnRect.yMax), -256f);
			fireworkObj.transform.localPosition = randomPosition;
			AudioManager.Instance.PlayClip("Firework", variations: 3);

			// Wait random interval before spawning another one
			float randomInterval = UnityEngine.Random.Range(fireworksIntervalMin, fireworksIntervalMax);
			yield return new WaitForSeconds(randomInterval);
		}
	}
}
