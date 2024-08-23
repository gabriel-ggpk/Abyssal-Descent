using UnityEngine;

public class GameHandler : MonoBehaviour {


    [SerializeField] private SpriteAnimator spriteAnimator;
    [SerializeField] private Sprite[] idleAnimationFrameArray;
    [SerializeField] private Sprite[] walkAnimationFrameArray;

    public HealthBar healthBar;

    private enum AnimationType {
        Idle,
        Walk,
    }
    private AnimationType activeAnimationType;

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

        if (isMoving) {
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
            }
        }
    }

}
