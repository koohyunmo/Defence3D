using System.Collections;
using UnityEngine;

public class UpSprite : MonoBehaviour
{
    float speed = 2f;
    Coroutine coUp = null;
    public void Spawn()
    {
        if(coUp != null)
        {
            StopCoroutine(coUp);
            coUp = null;
        }
        coUp = StartCoroutine(co_Up());
    }

    IEnumerator co_Up()
    {
        float time = 1f;

        while (time >= 0f)
        {
            time -= Time.deltaTime;

            var pos = transform.position;
            pos.y += speed * Time.deltaTime;
            transform.position = pos;

            yield return null;
        }

        Managers.Resource.Destroy(gameObject);
    }
}
