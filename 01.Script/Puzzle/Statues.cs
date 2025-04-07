using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statues : BaseInteraction
{
    public StatuesManager Manager;
    [HideInInspector] public int n;
    public float angle;
    public float currect;
    public bool _bisCurrect;

    public override void Interaction()
    {
        base.Interaction();
        if (!_bisCurrect)
        {
            StartCoroutine(RotateOverYAxis());
        }
    }

    private IEnumerator RotateOverYAxis()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, angle, 0f);

        float elapsed = 0f;

        while (elapsed < 0.3f)
        {
            float t = elapsed / 0.3f;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        if(Mathf.Abs(Mathf.DeltaAngle(endRotation.eulerAngles.y, currect)) < 10f)
        {
            Manager._bisCheck[n] = true;
            _bisCurrect = true;
            Manager.Checking();
        }
    }
}
