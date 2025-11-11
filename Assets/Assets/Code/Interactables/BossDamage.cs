using System.Collections;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public GameObject boss;
    public UnityEventOnTimer timer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Retreat();
        }
    }

    public void Retreat()
    {
        timer.Pause();
        boss.GetComponent<Animator>().SetTrigger("Retreat");
        StartCoroutine(reenableLasers());
    }

    IEnumerator reenableLasers()
    {
        yield return new WaitForSeconds(7f);
        timer.Unpause();
    }
}
