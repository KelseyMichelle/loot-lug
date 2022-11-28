using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillWeapon : MonoBehaviour
{

    public float timeToLive = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (timeToLive != 0)
        {
            StartCoroutine(DestroyAfterTimer());
        }
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    IEnumerator DestroyAfterTimer()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }

    // Destroy object if animation finishes
    public void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded"))
        {
            KillObject();
        }
    }

    private void KillObject()
    {
        Destroy(gameObject);
    }
}
