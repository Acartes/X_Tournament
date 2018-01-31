using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpTo : MonoBehaviour
{

    public GetCase targetPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CaseData coll = collision.transform.GetComponent<CaseData>();
        PersoData perso = collision.transform.GetComponent<PersoData>();

        if (coll != null)
        {
            if(coll.caseElement == Element.Feu)
            {
                AirScript.Instance.airEffect.SetActive(false);
                AirScript.Instance.fireAirEffect.transform.position = coll.transform.position;
                AirScript.Instance.fireAirEffect.SetActive(true);
           //     coll.changeElement(Element.Aucun);
                transform.parent.gameObject.SetActive(false);
            }
        }
        else if (perso != null)
        {
            if (targetPosition.node == null) // pas de target = pas de tp
                return;
                perso.transform.position = targetPosition.transform.position + new Vector3(0, GraphManager.Instance.getCaseOffset(perso.gameObject), 0);
                AirScript.Instance.airEffect.SetActive(false);
        }

    }
}
