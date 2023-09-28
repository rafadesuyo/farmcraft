using System;

public interface IAbilityRestriction
{
    event Action<bool> OnRestrictionChange;
    bool IsRestricted();
}
