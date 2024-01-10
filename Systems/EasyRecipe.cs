using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCAttacker.Systems
{
    public class EasyRecipe : ModSystem
    {
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ItemID.KingStatue);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.AddIngredient(ItemID.StoneBlock, 99);
            recipe.Register();

            recipe = Recipe.Create(ItemID.QueenStatue);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.AddIngredient(ItemID.StoneBlock, 99);
            recipe.Register();
        }
    }
}
