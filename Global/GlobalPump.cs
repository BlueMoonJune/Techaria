namespace Techaria.Global;

public class GlobalPump : GlobalTile
{
    private int width = 2;
    private int height = 2;
    
    public Point16 GetTopLeft(int i, int j)
    {
        Tile tile = Framing.GetTileSafely(i, j);
        i -= tile.TileFrameX / 18 % width;
        j -= tile.TileFrameY / 18 % height;
        return new Point16(i, j);
    }
    
    public PumpTE GetTileEntity(int i, int j)
    {
        TileEntity.ByPosition.TryGetValue(GetTopLeft(i, j), out TileEntity te);
        return te as PumpTE;
    }

    public override void PlaceInWorld(int i, int j, int type, Item item)
    {
        if (type == TileID.InletPump)
        {
            Point16 p = GetTopLeft(i, j);
            ModContent.GetInstance<PumpTE>().Place(p.X, p.Y);
        }
    }

    public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (!fail && !effectOnly && type == TileID.InletPump)
        {
            Point16 p = GetTopLeft(i, j);
            ModContent.GetInstance<PumpTE>().Kill(p.X, p.Y);
        }
    }

    public override bool PreHitWire(int i, int j, int type)
    {
        if (type != TileID.InletPump) return true;
        var te = GetTileEntity(i, j);
        if (te != null && te.power.power >= 100)
        {
            te.power.Remove(100);
            return true;
        }
        return false;
    }

    public override void MouseOver(int i, int j, int type)
    {
        if (type != TileID.InletPump) return;
        var te = GetTileEntity(i, j);
        Main.LocalPlayer.cursorItemIconEnabled = true;
        Main.LocalPlayer.cursorItemIconText = te.power.ToString();
    }
}

public class PumpTE : ModTileEntity, IContain<Power>
{
    public Power power = new Power(0, 1000);
    
    public override bool IsTileValidForEntity(int x, int y)
    {
        return Main.tile[x, y].TileType == TileID.InletPump;
    }

    public Power[] Slots => [power];
}