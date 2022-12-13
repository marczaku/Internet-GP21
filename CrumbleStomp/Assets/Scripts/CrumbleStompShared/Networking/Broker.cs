using System;
using System.Collections.Generic;

namespace CrumbleStompShared.Networking
{
    public class Broker
    {
        private readonly Dictionary<Type, Delegate> subscriptions = new();
        public event Action<MessageBase> AnyMessageReceived;
        
        public void Subscribe<TMessage>(Action<TMessage> onMessageReceived)
            where TMessage : MessageBase
        {
            if (subscriptions.TryGetValue(typeof(TMessage), out var del))
                subscriptions[typeof(TMessage)] = Delegate.Combine(del, onMessageReceived);
            else
                subscriptions[typeof(TMessage)] = onMessageReceived;
        }

        public void Unsubscribe<TMessage>(Action<TMessage> onMessageReceived)
            where TMessage : MessageBase
        {
            if (subscriptions.TryGetValue(typeof(TMessage), out var del))
                subscriptions[typeof(TMessage)] = Delegate.Remove(del, onMessageReceived);
        }

        public void Publish(Type type, MessageBase data)
        {
            if (subscriptions.TryGetValue(type, out var listener))
            {
                listener.DynamicInvoke(data);
            }
            this.AnyMessageReceived?.Invoke(data);
        }
    }
}



public class ItemSO : ScriptableObject
{
    
}

// CONFIG: ItemSO || Configured by Designer through ScriptableObject
// MODEL: ItemInInventory{Id:ItemSOId, Rarity: Rarity, Level: level} || Serialized in SaveGame / State
// VIEW-MODEL: Factory.Create(ItemInInventory) -> Item || GameObject / Prefab / Class / Used in Gameplay

public class ItemFactory
{
    private IStatModifierApplier[] modifierrappliers = new
    {
        new WeaponBonusDamageSMA(),
        // ...
    };
    
    public Item CreateItem(ItemSO item, Rarity rarity)
    {
        
    }
}

public interface IStatModifierApplier
{
    void Apply(Item item, StatModifierSO modifier);
}


public class WeaponBonusDamageSMA : IStatModifierApplier
{
    public void Apply(Item item, StatModifierSO modifier)
    {
        if (item is Weapon weapon)
        {
            if (modifier is DamageModifier damageModifier)
            {
                weapon.attackDamage *= damageMutiplier.factor;
            }
        }
    }
}

public class WeaponRarityBonusSMA : IStatModifierApplier
{
    public void Apply(Item item, StatModifierSO modifier)
    {
        if (item is Weapon weapon)
        {
            if (modifier is DamageModifier damageModifier)
            {
                weapon.attackDamage *= damageMutiplier.factor;
            }
        }
    }
}