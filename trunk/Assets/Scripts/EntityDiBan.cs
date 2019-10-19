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
        if (curScale.x == 0)
        {
            curScale.x = 1;
        }
        if (curScale.y == 0)
        {
            curScale.y = 1;
        }
        l.localScale = new Vector3(1 / curScale.x, scale.y, scale.z);
        l.localPosition = new Vector3(-(curScale.x * 0.5f/2 - 0.1f)/ curScale.x, 0, pos.z);

        scale = r.localScale;
        pos = l.localPosition;
        r.localScale = new Vector3(1 / curScale.x, scale.y, scale.z);
        r.localPosition = new Vector3((curScale.x * 0.5f / 2 - 0.1f) / curScale.x, 0, pos.z);

        scale = t.localScale;
        pos = l.localPosition;
        t.localScale = new Vector3(scale.x, 1 / curScale.y, scale.z);
        t.localPosition = new Vector3(0,(curScale.y * 0.5f / 2 - 0.1f) / curScale.y, pos.z);

        scale = b.localScale;
        pos = l.localPosition;
        b.localScale = new Vector3(scale.x, 1 / curScale.y, scale.z);
        b.localPosition = new Vector3(0, -(curScale.y * 0.5f / 2 - 0.1f) / curScale.y, pos.z);


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
