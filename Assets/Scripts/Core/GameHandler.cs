using UnityEngine;

public class GameHandler : MonoBehaviour {

    [SerializeField] private Player player;
    [SerializeField] private SpriteAnimator spriteAnimator;
    [SerializeField] private Sprite[] idleAnimationFrameArray;
    [SerializeField] private Sprite[] walkAnimationFrameArray;
    [SerializeField] private Sprite[] jumpAnimationFrameArray;
    [SerializeField] private Sprite[] flyAnimationFrameArray;


    public HealthBar healthBar;

    private enum AnimationType {
        Idle,
        Walk,
        Jump,
        Fly,
    }

    private AnimationType activeAnimationType;
    private bool isJumping = false;
    private bool isFlying = false;

    private void Start() {
        HealthSystem healthSystem = new HealthSystem(100);
        healthBar.Setup(healthSystem);
        PlayAnimation(AnimationType.Idle);
    }
 
    private void Update() {
        bool isMoving = false;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) {
            isMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded() && !isJumping && !isFlying) {
            isJumping = true;
            PlayAnimation(AnimationType.Jump);
        }

        if (isJumping && !player.IsGrounded() && !isFlying) { 
            isFlying = true;
            PlayAnimation(AnimationType.Fly);
        }

        if (isFlying) {
            if (player.IsGrounded()) {
                isFlying = false;
                isJumping = false;
                PlayAnimation(isMoving ? AnimationType.Walk : AnimationType.Idle);
            }    
        } else if (isJumping) {
            if (!player.IsGrounded()) {
                isJumping = false;
                PlayAnimation(isMoving ? AnimationType.Walk : AnimationType.Idle);
            }
        } else if (isMoving && player.IsGrounded()) {
            PlayAnimation(AnimationType.Walk);
        } else {
            PlayAnimation(AnimationType.Idle);
        }

    }
 
    private void PlayAnimation(AnimationType animationType) {
        if (animationType != activeAnimationType) { 
            activeAnimationType = animationType;

            switch (animationType) {
                case AnimationType.Idle:
                    spriteAnimator.PlayAnimation(idleAnimationFrameArray, .2f);
                    break;
                case AnimationType.Walk:
                    spriteAnimator.PlayAnimation(walkAnimationFrameArray, .1f);
                    break;
                case AnimationType.Jump:
                    spriteAnimator.PlayAnimation(jumpAnimationFrameArray, .3f);
                    break;
                case AnimationType.Fly:
                    spriteAnimator.PlayAnimation(flyAnimationFrameArray, .3f);
                    break;
            }
        }
    }

}
