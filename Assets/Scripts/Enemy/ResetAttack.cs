using UnityEngine;

namespace Enemy
{
    public class ResetAttack : StateMachineBehaviour
    {
        private EnemyManager _enemyManager;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            if (_enemyManager is null)
            {
                _enemyManager = animator.GetComponent<EnemyManager>();
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            if (_enemyManager is null)
                return;

            _enemyManager.canAttack = true;
            _enemyManager.ActivateMovement();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }
    }
}