using UnityEngine;

public class MeleeZombie : navmeshtestscript
{
    int number;

    protected override void Start()
    {
        base.Start();
        number = 0;
    }


    private void ResetAttack()
    {
        number++;

        if (number < 3) return;
        canAttack = true;
        number = 0;
    }
}
