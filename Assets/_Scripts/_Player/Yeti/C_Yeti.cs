

public class C_Yeti : PlayerController
{


    void Update()
    {
        if (isPaused || isDie) return;


        base.InputMove();
    }

    private void FixedUpdate() => base.Move(false);
    

}
