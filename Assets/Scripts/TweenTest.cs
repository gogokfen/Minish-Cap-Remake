using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Sequence work = DOTween.Sequence();

        work.Append(transform.DOMoveY(3, 2));
        work.AppendInterval(3);
        work.Append(transform.DOMoveY(-3, 2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
