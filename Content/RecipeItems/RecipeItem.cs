﻿using Terraria;
using Terraria.ModLoader;

namespace Techaria.Content.RecipeItems
{
    public abstract class RecipeItem : ModItem
    {
        public override void UpdateInventory(Player player)
        {
            Item.TurnToAir();
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Item.TurnToAir();
        }

        public override void HoldItem(Player player)
        {
            Item.TurnToAir();
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99999;
        }
    }
}