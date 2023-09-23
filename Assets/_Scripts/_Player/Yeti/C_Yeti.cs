

public class C_Yeti : PlayerController
{


    void Update()
    {
        if (isPaused || isDie) return;


        base.InputMove();


    }
    void FixedUpdate() => base.Move(false);
    

}
