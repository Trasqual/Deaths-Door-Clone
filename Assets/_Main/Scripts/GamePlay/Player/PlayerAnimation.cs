
namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerAnimation : AnimationBase
    {
        public void PlayRollAnim()
        {
            anim.SetTrigger("roll");
        }
    }
}