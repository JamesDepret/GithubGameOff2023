namespace Ability;
public  abstract partial class BaseAbilityController : Node
{
    public string SubName { get; set; } = "";
    public BaseUpgrade Upgrade { get; set; }
    public virtual void Init() {}
    public virtual void DoEffect() {}
}