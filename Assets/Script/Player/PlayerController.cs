using UnityEngine;

public class PlayerController : Actor
{
    public override void MoveRight()
    {
        Debug.Log($"Move Right !");
    }

    public override void MoveLeft()
    {
        Debug.Log($"MoveLeft !");
    }

    public override void Search()
    {
        Debug.Log($"Search !");
    }

    public override void TakeStair()
    {
        Debug.Log($"TakeStair !");
    }
}
