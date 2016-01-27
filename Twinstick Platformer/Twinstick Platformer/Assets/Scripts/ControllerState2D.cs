using UnityEngine;
using System.Collections;

public class ControllerState2D 
{
    public bool IsCollidingRight { get; set; }
    public bool IsCollidingLeft { get; set; }
    public bool IsCollidingAbove { get; set; }
    public bool IsCollidingBelow { get; set; }
    /*
    public bool IsMovingDownSlope { get; set; }
    public bool IsMovingUpSlope { get; set; }
    */
    public bool IsGrounded { get { return IsCollidingBelow; } }
    public float SlopeAngle { get; set; }

    public bool HasCollisions { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; } }

    public void Reset()
    {
        /*
        IsMovingDownSlope =
        IsMovingUpSlope =
        */
        IsCollidingRight =
        IsCollidingLeft =
        IsCollidingAbove =
        IsCollidingBelow = false;

        SlopeAngle = 0;

    }
    public override string ToString()
    {
        return string.Format("(controller: r:{0} l:{1} a:{2} b: {3} angle {4})",
            IsCollidingRight,
            IsCollidingLeft,
            IsCollidingAbove,
            IsCollidingBelow,
            SlopeAngle);

    }
}
