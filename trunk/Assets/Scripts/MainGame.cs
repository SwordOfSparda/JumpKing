using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public const int CameraScale = 100;//屏幕分辨率 与世界尺寸比例
    public static MainGame Instance;
    public Transform StartPointTran;//起始点
    public Camera MainCamera;//主摄像机
    public EntityHero Hero;//主角
    private  Transform _heroTran;
    private Transform _mainCameraTran;
    private Vector3 _startpos;
    public Point ScreenSize;
    private void Awake()
    {
        Instance = this;
        _heroTran = Hero.transform;
        _mainCameraTran = MainCamera.transform;
        ScreenSize = new Point(Screen.width,Screen.height);
        _startpos = StartPointTran.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    int _cameraDis = 0;//摄像机在第几屏的位置
    // Update is called once per frame
    void Update()
    {
        Vector3 heropos = _heroTran.position;
        float fDist = heropos.y - _startpos.y;

        int iDis = (int)fDist * CameraScale / ScreenSize.Y;
        if(iDis != _cameraDis)
        {
            _cameraDis = iDis;
            _mainCameraTran.position = new Vector3(0,iDis * (ScreenSize.Y/ CameraScale),-100);
        }
    }
}
