namespace Ability;
public partial class ChargerTurret : BulletAbilityController
{
	[Export] float chargeDownTime = 10f;
	[Export] float chargeDuration = 1f;
	[Export] float attackSpeedIncrease = 10f;

	Godot.Timer ChargeDownTime;
	Godot.Timer ChargeDuration;
	public override void _Ready()
	{
		base._Ready();
		ChargeDownTime = GetNode<Godot.Timer>("ChargeDownTime");
		ChargeDuration = GetNode<Godot.Timer>("ChargeDuration");

		ChargeDownTime.WaitTime = chargeDownTime;
		ChargeDuration.WaitTime = chargeDuration;

		ChargeDownTime.Start();
		ChargeDuration.Stop();

		ChargeDownTime.Timeout += ChargedGunDowntime;
		ChargeDuration.Timeout += ChargedGunEffect;
	}

	private void ChargedGunDowntime()
	{
		GD.Print("ChargedGunDowntime");
		cooldownTimer.WaitTime = baseWaitTime;
		ChargeDuration.Start();
		ChargeDownTime.Stop();
	}

	private void ChargedGunEffect()
	{
		GD.Print("ChargedGunEffect");
		cooldownTimer.WaitTime = baseWaitTime / attackSpeedIncrease;
		ChargeDownTime.Start();
		ChargeDuration.Stop();
	}
}
