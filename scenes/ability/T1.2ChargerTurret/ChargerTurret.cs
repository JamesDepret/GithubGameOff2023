namespace Ability;
public partial class ChargerTurret : BulletAbilityController
{
	[Export] float chargeDownTime = 10f;
	[Export] float chargeDuration = 1f;
	[Export] float attackSpeedIncrease = 10f;
	private double normalBulletWaitTime;

	Godot.Timer ChargeDownTime;
	Godot.Timer ChargeDuration;
	public override void _Ready()
	{
		base._Ready();
		normalBulletWaitTime = baseWaitTime;
		ChargeDownTime = GetNode<Godot.Timer>("ChargeDownTime");
		ChargeDuration = GetNode<Godot.Timer>("ChargeDuration");

		ChargeDownTime.WaitTime = chargeDownTime;
		ChargeDuration.WaitTime = chargeDuration;
		ChargeDownTime.Stop();
		ChargeDuration.Start();

		ChargeDownTime.Timeout += ChargedGunEffect;
		ChargeDuration.Timeout += ChargedGunDowntime;
	}
	public override void _ExitTree()
    {
		ChargeDownTime.Timeout -= ChargedGunEffect;
		ChargeDuration.Timeout -= ChargedGunDowntime;
    }

	private void ChargedGunDowntime()
	{
		baseWaitTime = normalBulletWaitTime;
		cooldownTimer.WaitTime = normalBulletWaitTime;
		ChargeDownTime.Start();
		ChargeDuration.Stop();
	}

	private void ChargedGunEffect()
	{
		baseWaitTime = normalBulletWaitTime / attackSpeedIncrease;

		// Reset the cooldown timer by stop and starting else we might be in the middle of a shot
		cooldownTimer.WaitTime = baseWaitTime / attackSpeedIncrease;
		cooldownTimer.Stop();
		cooldownTimer.Start();


		ChargeDownTime.Stop();
		ChargeDuration.Start();
	}
}
