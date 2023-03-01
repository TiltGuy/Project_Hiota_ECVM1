using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class TextFX : MonoBehaviour
{
   static public void Create(Vector3 position, string text, float duration = 1f)
    {
        var textPrefab = Resources.Load<GameObject>("TextFX");
        var textFX = Instantiate(textPrefab, position, Quaternion.identity).GetComponent<TextFX>();
        textFX.textMesh.text = text;
        textFX.duration = duration;
    }

    public TextMesh textMesh => GetComponent<TextMesh>();
    public float duration = 1f;
    public float upSpeed = 1f;
    public bool lookAtMainCamera = true;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    void Update()
    {
        transform.Translate(Vector3.up * upSpeed * Time.deltaTime);

        if ( lookAtMainCamera )
        {
            var dir = (transform.position - Camera.main.transform.position).normalized;
            dir.y = 0;

            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
