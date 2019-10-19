using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EntityDiBan : MonoBehaviour
{

    public void ResetChildScale()
    {
        Vector3 curScale = _cur.localScale;
        Transform l = _cur.Find("l");
        Transform r = _cur.Find("r");
        Transform t = _cur.Find("t");
        Transform b = _cur.Find("b");

        Vector3 scale = l.localScale;
        Vector3 pos = l.localPosition;
        l.localScale = new Vector3(1 / curScale.x, scale.y, scale.z);

     /*   scale = r.localScale;
        r.localScale = new Vector3(1 / curScale.x, scale.y, scale.z);

        scale = t.localScale;
        t.localScale = new Vector3(scale.x, 1 / curScale.y, scale.z);

        scale = b.localScale;
        b.localScale = new Vector3(scale.x, 1 / curScale.y, scale.z);*/


    }
    Transform _cur;
    // Start is called before the first frame update
    void Start()
    {
        _cur = transform;
        ResetChildScale();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
