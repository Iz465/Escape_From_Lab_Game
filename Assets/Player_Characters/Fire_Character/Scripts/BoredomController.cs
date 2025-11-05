using UnityEngine;

public class BoredomControler : StateMachineBehaviour
{

    [SerializeField]
    private float _TimeTillBoredom_;
    [SerializeField]
    private int _BoredAnimationCount_;

    private bool _isBored_;
    private float _IdleTime;
    private int _boredAnimIndex;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetBoredom();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_isBored_ == false)
        {
            _IdleTime += Time.deltaTime;
            if(_IdleTime >= _TimeTillBoredom_ && stateInfo.normalizedTime % 1 < 0.02)
            {
                _boredAnimIndex = Random.Range(1, _BoredAnimationCount_ + 1);
                _boredAnimIndex = _boredAnimIndex * 2 - 1;

                _isBored_ = true;
                animator.SetFloat("BoredAnimation", _boredAnimIndex -1);
            }
        }
        else if(stateInfo.normalizedTime % 1 > 0.98 )
        {
           ResetBoredom();
        }
        animator.SetFloat("BoredAnimation", _boredAnimIndex,0.2f,Time.deltaTime);
    }
    private void ResetBoredom()
    {
        if (_isBored_)
        {
            _boredAnimIndex--;
        }
        _isBored_ = false;
        _IdleTime = 0.0f;
       
    }
}
