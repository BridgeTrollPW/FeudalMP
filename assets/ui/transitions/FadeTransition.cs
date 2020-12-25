using Godot;
public class FadeTransition : ColorRect
{
    private AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public async void FadeIn()
    {
        animationPlayer.PlayBackwards("Fade");
        await ToSignal(animationPlayer, "animation_finished");
    }

    public async void FadeOut()
    {
        animationPlayer.Play("Fade");
        await ToSignal(animationPlayer, "animation_finished");
    }
}
