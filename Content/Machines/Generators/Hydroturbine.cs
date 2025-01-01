namespace Techaria.Content.Machines.Generators;

using static Hydroturbine;
public class Hydroturbine : Machine<Tile, Item, Entity>
{
    public class Tile : BaseTile
    {
        public override int width => 2;
        public override int height => 2;
		
        public override void MouseOver(int i, int j)
		{
			var player = Main.LocalPlayer;
			
			player.cursorItemIconEnabled = true;
			player.cursorItemIconText =
					(GetTileEntity(i, j) as IContain<Power>).GetOutputSlotsForConnector(null)[0].ToString();
			player.noThrow = 2;
		}

		public override void ModifyTileObjectData()
		{
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
		}
    }
    
    public class Item : BaseItem
    {
        
    }
    
    public class Entity : BaseEntity, IContain<Power>
    {
	    private Power power = new(0, 1000);
	    public Power[] Slots => [];
	    
	    public Power[] OutputSlots => [power];

	    public override void Update()
	    {
		    for (int x = Position.X; x < Position.X + 2; x++)
		    {
			    var top = Main.tile[x, Position.Y];
			    var bot = Main.tile[x, Position.Y + 1];
			    
			    if (bot.LiquidAmount > 0 && bot.LiquidType != top.LiquidType) continue;
			    var amt = (byte)Math.Min(255 - bot.LiquidAmount, top.LiquidAmount);
			    power.Insert(amt * 0.075f);

			    WorldGen.PlaceLiquid(x, Position.Y + 1, (byte)top.LiquidType, amt);
			    top.LiquidAmount -= amt;
		    }
			
		    if (power.power > 0)
			{
				PowerSystem.PushPower(power, Position.X, Position.Y, new Tile().width, new Tile().height);
			}
	    }
    }
}