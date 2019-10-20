using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public const float CameraScale = 10f;//屏幕分辨率 与世界尺寸比例
    public static MainGame Instance;
    public Transform StartPointTran;//起始点
    public Camera MainCamera;//主摄像机
    public EntityHero Hero;//主角
    private  Transform _heroTran;
    private Transform _mainCameraTran;
    public Point ScreenSize;
    private void Awake()
    {
        Instance = this;
        _heroTran = Hero.transform;
        ScreenSize = new Point(Screen.width,Screen.height);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 heropos = _heroTran.position;
    }
}
