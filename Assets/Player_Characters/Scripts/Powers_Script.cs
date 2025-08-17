using UnityEngine;
using UnityEngine.InputSystem;

public class Powers_Script : MonoBehaviour 
{
    public int damage;
    public int stamina;
    public string powerName;

    virtual public void Initialize()
    {}


    virtual public void activatePower(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Initialize();
            Debug.Log($"Class type: {this.GetType()}");
            Debug.Log($"{powerName}\n");
            Debug.Log($" Damage = {damage}\n");
            Debug.Log($" Stamina = {stamina}\n");
        }
    }



}
