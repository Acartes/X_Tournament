using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>Gère les effets de chaque cases.</summary>
public class EffectManager : NetworkBehaviour
{
	// *************** //
	// ** Variables ** // Toutes les variables sans distinctions
	// *************** //

	public static EffectManager Instance;

	bool isPushing = false;
	bool oneTime = true;

	// ******************** //
	// ** Initialisation ** // Fonctions de départ, non réutilisable
	// ******************** //

	public override void OnStartClient()
	{
		if (Instance == null)
			Instance = this;
		StartCoroutine(waitForInit());
	}

	IEnumerator waitForInit()
	{
		while (!LoadingManager.Instance.isGameReady())
			yield return new WaitForEndOfFrame();

	}

	// *************** //
	// ** Fonctions ** // Fonctions réutilisables ailleurs
	// *************** //

	/// <summary>Un push multiple. Ne marche pas avec la tornade.</summary>
	public void MultiplePush(GameObject objAfflicted, CaseData caseAfflicted, int pushValue, PushType pushType, Direction pushDirection = Direction.Front)
	{
		StartCoroutine(MultiplePushDelayed(objAfflicted, caseAfflicted, pushValue, pushType, pushDirection));
	}

	IEnumerator MultiplePushDelayed(GameObject objAfflicted, CaseData caseAfflicted, int pushValue, PushType pushType, Direction pushDirection = Direction.Front)
	{
    if (objAfflicted.GetComponent<PersoData>())
			yield return new WaitForSeconds(0.05f);

      if (objAfflicted.GetComponent<BallonData>() && objAfflicted.GetComponent<BallonData>().isPushed == false)
		{
			yield return new WaitForSeconds(0.12f);
		}
		if (objAfflicted.GetComponent<BallonData>() && (objAfflicted.GetComponent<BallonData>().isPushed == true || objAfflicted.GetComponent<BallonData>().isMoving))
		{
			yield return new WaitForSeconds(0.02f);
		}

    if (objAfflicted.GetComponent<BallonData>() && objAfflicted.GetComponent<BallonData>().isMoving)
    {
      objAfflicted.GetComponent<BallonData>().StopMove();
      objAfflicted.GetComponent<BallonData>().StopAllCoroutines();
    }


    if (GameManager.Instance.actualAction == PersoAction.isMoving)
      {
      MoveBehaviour.Instance.StopAllCoroutines();
      }

    yield return new WaitForSeconds(0.1f);

    PushBehaviour.Instance.PushCheck(objAfflicted, pushValue, caseAfflicted, pushType, pushDirection);
		PushBehaviour.Instance.MultiplePushStart();
	}


	/// <summary>Augmente ou diminue le nombre de PA de la cible.</summary>
	public void ChangePA(int number)
	{
		GameManager.Instance.manaGlobalActual += number;
		if (number < 0)
			GameManager.Instance.manaGlobalActual = Mathf.Clamp(GameManager.Instance.manaGlobalActual, 0, GameManager.Instance.manaGlobalMax); // on peut pas dépasser le max
	}

	public void ChangePr(PersoData persoAfflicted, int number)
	{
		persoAfflicted.actualPointResistance += number;
		persoAfflicted.actualPointResistance = Mathf.Clamp(persoAfflicted.actualPointResistance, 0, persoAfflicted.maxPointResistance); // on peut pas dépasser le max
		InfoPerso.Instance.stats.updatePr(persoAfflicted.actualPointResistance);
	}

	public void ChangePm(PersoData persoAfflicted, int number)
	{
		persoAfflicted.actualPointMovement += number;
		persoAfflicted.actualPointMovement = Mathf.Clamp(persoAfflicted.actualPointMovement, 0, persoAfflicted.maxPointMovement); // on peut pas dépasser le max
		InfoPerso.Instance.stats.updatePm(persoAfflicted.actualPointMovement);
	}

	public void ChangePADebuff(int number)
	{
		GameManager.Instance.paDebuff += number;
	}

	public void ChangePmDebuff(PersoData persoAfflicted, int number)
	{
		persoAfflicted.pmDebuff += number;
	}

	public void ChangePr(SummonData summonAfflicted, int number)
	{
		summonAfflicted.actualPointResistance += number;
	}

	public void Rotate(GameObject objAfflicted, Direction newDirection)
	{
		Debug.Log(objAfflicted.GetComponent<SummonData>());
		Debug.Log(newDirection);
		if (objAfflicted.GetComponent<SummonData>())
			objAfflicted.GetComponent<SummonData>().pushDirection = newDirection;
	}

	public void doExplosion(CaseData caseAfflicted)
	{
		StartCoroutine(doExplosionCoroutine(caseAfflicted));
	}

	public IEnumerator doExplosionCoroutine(CaseData caseAfflicted)
	{
		damageAndPush(caseAfflicted.GetBottomLeftCase(), Direction.SudOuest);
		yield return new WaitForEndOfFrame();
		damageAndPush(caseAfflicted.GetBottomRightCase(), Direction.SudEst);
		yield return new WaitForEndOfFrame();
		damageAndPush(caseAfflicted.GetTopRightCase(), Direction.NordEst);
		yield return new WaitForEndOfFrame();
		damageAndPush(caseAfflicted.GetTopLeftCase(), Direction.NordOuest);
	}

	private void damageAndPush(CaseData tempCase, Direction direction)
	{
		if (tempCase == null)
			return;

		if (tempCase.personnageData != null)
		{
			if (tempCase.personnageData.timeStunned > 0)
			{
				return;
			}
			EffectManager.Instance.MultiplePush(tempCase.personnageData.gameObject, tempCase, 1, PushType.FromTerrain, direction);
		}
		if (tempCase.ballon != null)
		{
			EffectManager.Instance.MultiplePush(tempCase.ballon.gameObject, tempCase, 1, PushType.FromTerrain, direction);
		}
	}

}
