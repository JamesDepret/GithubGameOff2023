namespace Ability;
public  abstract partial class BaseAbilityController : Node
{
    public string SubName { get; set; } = "";
    public BaseUpgrade Upgrade { get; set; }
    public int TotalPrice { get; set; } = 0;
    public int TotalSupply { get; set; } = 0;
    public virtual void Init() {}
    public virtual void DoEffect() {}
}