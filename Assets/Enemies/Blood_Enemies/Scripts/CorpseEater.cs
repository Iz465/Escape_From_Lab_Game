using System.Collections;
using UnityEngine;

public class CorpseEater : navmeshtestscript
{
    [SerializeField]
    private LayerMask corpseLayer;
    private Collider[] corpseCount;
    protected override void Attack()
    {
        
        ConsumeCorpse();
    }

    private void ConsumeCorpse()
    {
        corpseCount = Physics.OverlapSphere(transform.position, 15f, corpseLayer);
        if (corpseCount.Length > 0)
            StartCoroutine(CorpseTravel(5f));
        else
            Debug.Log("No Corpse To Consume");
 
    }

    private IEnumerator CorpseTravel(float time)
    {
        float timer = 0f;
        Vector3 startPos = corpseCount[0].gameObject.transform.position;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = transform.localScale;
        endScale.x *= 2f;
        endScale.y *= 2f;
        endScale.z *= 2f;
        controller.enabled = false;
      

        while (timer < time)
        {
            corpseCount[0].gameObject.transform.position = Vector3.Lerp(startPos, transform.position, timer / time);
            transform.localScale = Vector3.Lerp(startScale, endScale, timer / time);

            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(corpseCount[0].gameObject);
        controller.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 15f);
    }


}
