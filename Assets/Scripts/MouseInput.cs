using System.Collections;
using System.Collections.Generic;
using RavingBots.MultiInput;
using UnityEngine;
using System.Linq;
using System.Text;
using UnityEngine.UI;
public class MouseInput : MonoBehaviour
{
    private static readonly IList<InputCode> InterestingAxes;

    public InputState inputState;
    
    private List<IDevice> _mice;
    public int playerNum = 0;

    //Rotation script for the robot game
    private RobotRotator rotator;
    // Update is called once per frame

    private void Start()
    {
        rotator = gameObject.GetComponent<RobotRotator>();
    }

    void Update()
    {
        
        //doing this in update, because it's easier than making sure that this script runs 
        //after InputState
        if (_mice == null)
        {
            _mice = new List<IDevice>();
            foreach (var device in inputState.Devices)
            {
            //    if (device.SupportedAxes.Contains(InputCode.MouseX))
            //    {
                    if (device.Id % 2 == playerNum)
                    {
                        _mice.Add(device);
                    }
                
            //  }
            }

        }
        
        var xCode = InputCode.MouseX;
        var yCode = InputCode.MouseY;

        float mouseX=0;
        float mouseY = 0;
        
        //getting the biggest mouse movement for odd and even numbered devices
        //note: I haven't bothered to check if the devices are actual mice.
        foreach (IDevice mouse in _mice)
        {
            if (mouse[xCode] == null) continue;
            float Xvalue = mouse[xCode].Value;
            if (Mathf.Abs(Xvalue) > mouseX)
            {
                mouseX = Xvalue;
            }
            float Yvalue = mouse[yCode].Value;
            if (Mathf.Abs(Yvalue) > mouseY)
            {
                mouseY = Yvalue;
            }
        }

       //transform.position += new Vector3(mouseX*Time.deltaTime, mouseY*Time.deltaTime, 0);
        rotator.Rotate(mouseX*Time.deltaTime, mouseY*Time.deltaTime);
    }

    public List<IDevice> Mice
    {
        get { return _mice; }
    }

}
