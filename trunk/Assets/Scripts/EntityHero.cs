using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHero : MonoBehaviour
{
    // we need a clock for time record.
    Clock d_clock = new Clock();

    // weak ref for Rigidbody Component.
    Rigidbody d_rigidbody = null;

    double d_JumpXImpulse = 5.6;
    // impulse = kg * s / s^2   
    double d_JumpYImpulse = 49;

    // default is 250us = 250000
    System.UInt64 d_JumpLimtTimeUS = 300000;

    System.Int32 d_move_y = 0;
    System.Int32 d_move_x = 0;

    // m / s
    double d_XVelocity = 2.5;

    double d_JumpDirectionDegree = 72.0;

    System.Int32 d_direction_x = 0;

    private bool d_isFlying;

    // 最大蓄力事件，默认是250000微秒
    public void SetJumpLimtTimeUS(System.UInt64 _microsecond)
    {
        this.d_JumpLimtTimeUS = _microsecond;
    }

    public void SetJumpXImpulse(double _yImpulse)
    {
        this.d_JumpXImpulse = _yImpulse;
    }
    // y轴冲量蓄力效率，默认是65
    public void SetJumpYImpulse(double _yImpulse)
    {
        this.d_JumpYImpulse = _yImpulse;
    }

    // x轴水平移动速度，默认是200米每秒。
    public void SetXVelocity(double _xVelocity)
    {
        this.d_XVelocity = _xVelocity;
    }

    // 起跳固定夹角，默认是75度。
    public void SetJumpDirectionDegree(double _dDegree)
    {
        this.d_JumpDirectionDegree = _dDegree;
    }

    
    public bool IsFlying
    {
        get
        {
            return this.d_isFlying;
        }
    }


    public void JumpByImpulse(System.UInt64 _microseconds)
    {
        if(this.d_isFlying)
        {
            return;
        }
        if (1 == this.d_move_y)
        {
            double _x = this.d_move_x * this.d_JumpXImpulse;
            double _y = this.d_JumpYImpulse * _microseconds / 1000000.0;
            double _z = 0.0;

            // direction degree.
            // _x = (this.d_move_x * _y) / Mathf.Tan((float)(this.d_JumpDirectionDegree * 3.1415926 / 180.0));

            Vector3 _impulse = new Vector3((float)_x, (float)_y, (float)_z);

            // this.d_rigidbody.drag = 0;
            // 飞起来的话，就受到物理效果
            //this.d_rigidbody.isKinematic = false;
            // add a Impulse to rigidbody.
            this.d_rigidbody.AddForce(_impulse, ForceMode.Impulse);

            Debug.Log("us_dt: " + _microseconds + " impulse:" + _impulse);

            // reset the move impulse.
            this.d_move_x = 0;
            this.d_move_y = 0;

            this.d_direction_x = 0;
        }
       
    }

    public void ProcessMove()
    {
        if(this.d_isFlying)
        {
            return;
        }
        if (0 == this.d_move_y && 0 != this.d_move_x)
        {
            double _t = UnityEngine.Time.fixedDeltaTime;

            Vector3 _position = this.d_rigidbody.position;

            Vector3 _velocity = new Vector3();
            Vector3 _s = new Vector3();

            // s = v * t.
            _velocity.x = (float)(this.d_direction_x * this.d_XVelocity);
            _velocity.y = 0.0f;
            _velocity.z = 0.0f;

            _s = _velocity * (float)_t;

            //this.d_rigidbody.isKinematic = false;

            this.d_rigidbody.MovePosition(_position + _s);

            this.d_rigidbody.velocity = _velocity;

            Debug.Log("移动");
        }
    }

    public void ProcessControl(System.UInt64 _microseconds)
    {
        // Input.GetKeyXXX 只会触发单次

        // 按下 跳跃 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("按下 跳跃");

            this.d_clock.Reset();

            this.d_move_y = 1;
        }
        // 松开 跳跃 
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("松开 跳跃");

            this.JumpByImpulse(_microseconds);

            // reset always.
            this.d_move_x = 0;
            this.d_move_y = 0;
        }

        // 按下 左箭头
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("按下 左箭头");

            this.d_move_x = -1;

            this.d_direction_x = -1;
        }
        // 松开 左箭头
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Debug.Log("松开 左箭头");

            this.d_move_x = (0 == this.d_move_y) ? 0 : this.d_move_x;

            this.d_direction_x = 0;
        }

        // 按下 右箭头
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("按下 右箭头");

            this.d_move_x = +1;

            this.d_direction_x = +1;
        }
        // 松开 右箭头
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Debug.Log("松开 右箭头");

            this.d_move_x = (0 == this.d_move_y) ? 0 : this.d_move_x;

            this.d_direction_x = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // reset the clock.
        this.d_clock.Reset();

        // Assignment the weak ref.
        this.d_rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        System.UInt64 _microseconds = this.d_clock.Microseconds();

        if (1 == this.d_move_y && _microseconds >= this.d_JumpLimtTimeUS)
        {
            this.JumpByImpulse(this.d_JumpLimtTimeUS);

            this.d_move_y = 0;
        }
        else
        {
            this.ProcessControl(_microseconds);

            this.ProcessMove();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //// Vector3 _normalFloor = new Vector3(0, 1, 0);
        //ContactPoint cp = collision.GetContact(0);
        //float th = Mathf.Atan2(cp.normal.y, cp.normal.x);
        //float d = th * 180.0f / 3.1415926f;
        //// 法线约为竖直线，表明这是地板
        //if (Mathf.Abs(d - 90.0f) < 0.001f)
        //{
        //    // 速度归0
        //    this.d_rigidbody.velocity = new Vector3(0, 0, 0);
        //    //this.d_rigidbody.angularVelocity = new Vector3(0, 0, 0);
        //    //this.d_rigidbody.isKinematic = true;
        //    //this.d_rigidbody.drag = float.PositiveInfinity;

        //    // 碰到了地板，站在地板上
        //    this.d_isFlying = false;

        //    Debug.Log("落到地板上");
        //}

        //if (Mathf.Abs(d) < 0.001f || Mathf.Abs(d - 180) < 0.001f || Mathf.Abs(d + 180) < 0.001f)
        //{
        //    // 碰到了竖直墙
        //    this.d_rigidbody.velocity = new Vector3(0, 0, 0);
        //}

        GameObject obj = collision.gameObject;
        if (obj != null && obj.name.Equals(ConfigWord.Entity_ner_t))
        {
            Vector3 _velocity = this.d_rigidbody.velocity;

            if (_velocity.y >= 12.0)
            {
                Debug.Log("坠落");
            }
            else
            {
                Debug.Log("站稳");
            }

            // 碰到了地板，站在地板上
            this.d_isFlying = false;

            // 水平速度归0
            this.d_rigidbody.velocity = new Vector3(0, 0, 0);
        }
        if (obj != null && (obj.name.Equals(ConfigWord.Entity_ner_l) || obj.name.Equals(ConfigWord.Entity_ner_l)))
        {
            // 碰到左边和右边的墙，水平速度归0
            // 不归0的话会在边界颤抖

            Vector3 _velocity = this.d_rigidbody.velocity;

            // 水平速度归0
            this.d_rigidbody.velocity = new Vector3(0, 0, 0);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        //Vector3 _velocity = new Vector3();

        //// s = v * t.
        //_velocity.x = (float)(this.d_direction_x * this.d_XVelocity);
        //_velocity.y = 0.0f;
        //_velocity.z = 0.0f;

        //this.d_rigidbody.velocity = _velocity;


        //// 离开了地板，漂浮在空中
        //this.d_isFlying = true;

        GameObject obj = collision.gameObject;
        if (obj != null && obj.name.Equals(ConfigWord.Entity_ner_t))
        {
            // 离开了地板
            this.d_isFlying = true;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        //ContactPoint cp = collision.GetContact(0);
        //float th = Mathf.Atan2(cp.normal.y, cp.normal.x);
        //float d = th * 180.0f / 3.1415926f;
        //// 法线约为竖直线，表明这是地板
        //if (Mathf.Abs(d - 90.0f) < 0.001f)
        //{
        //    // Vector3 _velocity = this.d_rigidbody.velocity;
        //    // 速度归0
        //    // this.d_rigidbody.velocity = new Vector3(0, _velocity.y, 0);
        //    //this.d_rigidbody.angularVelocity = new Vector3(0, 0, 0);

        //    // 碰到了地板，站在地板上
        //    this.d_isFlying = false;

        //    // Debug.Log("落到地板上");
        //}
        //// 碰到了地板，站在地板上
        //this.d_isFlying = false;

        GameObject obj = collision.gameObject;
        if (obj != null && obj.name.Equals(ConfigWord.Entity_ner_t))
        {
            // 碰到了地板，站在地板上
            this.d_isFlying = false;
        }
    }

    //
    private void OnTriggerEnter(Collider other)
    {

    }
    private void OnTriggerExit(Collider other)
    {

    }
    private void OnTriggerStay(Collider other)
    {

    }

}
