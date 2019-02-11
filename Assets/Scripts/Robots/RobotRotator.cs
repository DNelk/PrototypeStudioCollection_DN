using System.Collections;
using System.Collections.Generic;
using RavingBots.MultiInput;
using UnityEngine;

public class RobotRotator : MonoBehaviour
{
    public enum RotationAxes {MouseX = 0, MouseY = 1,} 

    public RotationAxes axes = RotationAxes.MouseX; 
    
    public Transform target;
    private Vector2 lastPos;
    public float speed;
    public float max = 360;
    public float min = -360;
    private float rotation = 0f;
    private Quaternion originalRotation;
	
    // Use this for initialization
    void Start ()
    {
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
        originalRotation = transform.localRotation;
    }
    
    public void Rotate(float x, float y)
    {
        Vector2 newPos = new Vector2(x,y);
        //Vector2 delta = newPos - lastPos;
        //Debug.Log(delta);
        if (axes == RotationAxes.MouseX) //Looking in x
        {
            rotation += newPos.x * speed;
            rotation = ClampAngle(rotation, min, max);
//            Debug.Log(rotation);
            //transform.RotateAround(target.up, rotation);
            if (rotation <= max && rotation >= min)
            {
                Quaternion xQuaternion = Quaternion.AngleAxis(rotation, target.up);

                transform.localRotation = originalRotation * xQuaternion;
                transform.localRotation.Set(transform.localRotation.x, 0f, 0f, transform.localRotation.w);
            }
        }
        else //Must be looking in y i guess
        {
            rotation += newPos.y * speed;
            rotation = ClampAngle(rotation, min, max);
         //   Debug.Log(rotation);
            //transform.RotateAround(target.right, rotation);
            if (rotation <= max && rotation >= min)
            {
                Quaternion yQuaternion = Quaternion.AngleAxis(rotation, target.right);

                transform.localRotation = originalRotation * yQuaternion;
                transform.localRotation.Set(0f, transform.localRotation.y, 0f, transform.localRotation.w);
            }
        }
        lastPos = newPos;
    }
    
    //keeps our numbers in readble range
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
