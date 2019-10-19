using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroAction
{
    HA_MOVE_L = 1,
    HA_MOVE_R = 2,
    HA_JUMP_L = 3,
    HA_JUMP_R = 4,
    HA_JUMP_C = 5,
};

public enum HeroControlHotKey
{
    HCH_L = 1,// 向左
    HCH_R = 2,// 向右
    HCH_J = 3,// 起跳
};

static class Time
{
    // return the current timecode - 1970/1/1 00:00:00.
    static public System.UInt64 CurrentUsec()
    {
        // 1970/1/1 00:00:00 = 621355968000000000 Ticks
        return ((System.UInt64)System.DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10;
    }
}

public class Clock
{
    System.UInt64 d_start = 0;

    /** Resets clock */
    public void Reset()
    {
        this.d_start = (System.UInt64)System.DateTime.Now.ToUniversalTime().Ticks;
    }

    /** Returns milliseconds since initialisation or last reset */
    public System.UInt64 Milliseconds()
    {
        // 1970/1/1 00:00:00 = 621355968000000000 Ticks
        return ((System.UInt64)System.DateTime.Now.ToUniversalTime().Ticks - this.d_start) / 10000;
    }
    /** Returns microseconds since initialisation or last reset */
    public System.UInt64 Microseconds()
    {
        // 1970/1/1 00:00:00 = 621355968000000000 Ticks
        return ((System.UInt64)System.DateTime.Now.ToUniversalTime().Ticks - this.d_start) / 10;
    }
}

public class EntityHero : MonoBehaviour
{
    // we need a clock for time record.
    Clock d_clock = new Clock();

    // weak ref for Rigidbody Component.
    Rigidbody d_rigidbody = null;

    // hotkey dictionary.
    Dictionary<System.UInt32, System.UInt32> d_dic = new Dictionary<System.UInt32, System.UInt32>();

    double d_move_x_impulse = 0.5;

    // impulse = kg * s / s^2   
    double d_move_y_impulse = 50;

    // default is 2s = 2000000
    System.UInt64 d_jump_limt_time_us = 300000;

    System.Int32 d_move_y = 0;
    System.Int32 d_move_x = 0;

    // m / s
    double d_direction_x_velocity = 200.0;

    double d_direction_degree = 60.0;

    System.Int32 d_direction_x = 0;

    public void SetHotKey(System.UInt32 type, System.UInt32 keycode)
    {
        this.d_dic.Add(type, keycode);
    }

    private bool _isFlying;
    public bool IsFlying
    {
        get
        {
            return _isFlying;
        }
    }


    public void JumpByImpulse(System.UInt64 _microseconds)
    {
        if(_isFlying)
        {
            return;
        }
        if (1 == this.d_move_y)
        {
            double _x = this.d_move_x * this.d_move_x_impulse;
            double _y = this.d_move_y_impulse * _microseconds / 1000000.0;
            double _z = 0.0;

            // direction degree.
            _x = (_x * _y) / Mathf.Tan((float)(this.d_direction_degree * 3.1415926 / 180.0));

            Vector3 _impulse = new Vector3((float)_x, (float)_y, (float)_z);

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
        if (0 == this.d_move_y && 0 != this.d_move_x)
        {
            double _x = this.d_direction_x * this.d_direction_x_velocity * 0.001;
            double _y = 0.0;
            double _z = 0.0;

            Vector3 _velocity = new Vector3((float)_x, (float)_y, (float)_z);

            Vector3 _position = this.d_rigidbody.position;

            this.d_rigidbody.MovePosition(_position + _velocity);

            Debug.Log("移动");
        }
    }

    public void ProcessControl(System.UInt64 _microseconds)
    {
        if(_isFlying)
        {
            return;
        }
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
    void Update()
    {
        System.UInt64 _microseconds = this.d_clock.Microseconds();

        if (1 == this.d_move_y && _microseconds >= this.d_jump_limt_time_us)
        {
            this.JumpByImpulse(this.d_jump_limt_time_us);
        }
        else
        {
            this.ProcessControl(_microseconds);

            this.ProcessMove();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if(obj!= null && obj.name.Equals(ConfigWord.Entity_ner_t))
        {
            _isFlying = false;
        }

    }
    void OnCollisionExit(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj != null && obj.name.Equals(ConfigWord.Entity_ner_t))
        {
            _isFlying = true;
        }

    }
    void OnCollisionStay(Collision collision)
    {


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
