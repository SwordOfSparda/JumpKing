
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EntityDiBan))]
public class EntityDiBanEditor : Editor
{
    private EntityDiBan mDiBan;
    // Start is called before the first frame update
    void Start()
    {
        mDiBan = target as EntityDiBan;
    }
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        mDiBan = target as EntityDiBan;
        mDiBan.ResetChildScale();
    }
    // Update is called once per frame
    void Update()
    {
        mDiBan.ResetChildScale();
    }
}