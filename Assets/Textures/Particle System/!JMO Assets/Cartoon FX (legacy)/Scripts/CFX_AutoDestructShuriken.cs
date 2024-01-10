using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	public bool OnlyDeactivate;

	// Добавьте счетчик воспроизведений
	private int playCount = 0;

	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}

	IEnumerator CheckIfAlive ()
	{
		ParticleSystem ps = this.GetComponent<ParticleSystem>();

		while(true && ps != null)
		{
			yield return new WaitForSeconds(0.5f);
			if(!ps.IsAlive(true))
			{
				// Увеличьте счетчик воспроизведений
				playCount++;

				// Если система частиц воспроизвелась дважды, остановите ее
				if (playCount >= 2)
				{
					if(OnlyDeactivate)
					{
						#if UNITY_3_5
							this.gameObject.SetActiveRecursively(false);
						#else
							this.gameObject.SetActive(false);
						#endif
					}
					else
						GameObject.Destroy(this.gameObject);
					break;
				}
				else
				{
					// Добавьте задержку перед воспроизведением системы частиц снова
					yield return new WaitForSeconds(1);
					ps.Play();
				}
			}
		}
	}
}