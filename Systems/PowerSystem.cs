using Techarria;

namespace Techaria.Systems;

public static class PowerSystem
{
    public static bool HasWire(byte wire, Tile tile) => wire switch
    {
        0 => tile.RedWire,
        1 => tile.BlueWire,
        2 => tile.GreenWire,
        3 => tile.YellowWire,
        _ => throw new ArgumentOutOfRangeException(nameof(wire), wire, null)
    };
    
    public static void TravelToPoint(byte wire, Point16 point, Queue<Point16> frontier, HashSet<Point16> visited)
    {
        if (visited.Contains(point) || !HasWire(wire, Main.tile[point])) return;
        frontier.Enqueue(point);
        visited.Add(point);
    }

    public static HashSet<IContain<Power>> ScanForConsumers(int x, int y, int width, int height)
    {
        var set = ScanForConsumers(0, x, y, width, height);
        set.UnionWith(ScanForConsumers(1, x, y, width, height));
        set.UnionWith(ScanForConsumers(2, x, y, width, height));
        set.UnionWith(ScanForConsumers(3, x, y, width, height));
        return set;
    }
    
    public static HashSet<IContain<Power>> ScanForConsumers(byte wire, int x, int y, int width, int height)
    {
        Queue<Point16> frontier = new();
        HashSet<Point16> visited = new();
        for (int j = y; j < y + height; j++)
        {
            for (int i = x; i < x + width; i++)
            {
                TravelToPoint(wire, new Point16(i, j), frontier, visited);
            }
        }

        while (frontier.Count > 0)
        {
            var point = frontier.Dequeue();
            foreach (var dir in Direction.directions())
            {
                var next = point + dir;
                TravelToPoint(wire, next, frontier, visited);
            }
        }
        
        for (int j = y; j < y + height; j++)
        {
            for (int i = x; i < x + width; i++)
            {
                visited.Remove(new Point16(i, j));
            }
        }
        
        HashSet<IContain<Power>> ret = new();

        foreach (var point in visited)
        {
    //        var dust = Dust.NewDustDirect(new(point.X * 16 + 4, point.Y * 16 + 4), 0, 0, DustID.TreasureSparkle);
    //        dust.velocity = Vector2.Zero;
            var tl = HelperMethods.GetTopLeftTileInMultitile(point.X, point.Y, out int w, out int h);
            TileEntity.ByPosition.TryGetValue(new(tl), out TileEntity te);
            if (te is IContain<Power> container)
            {
                ret.Add(container);
            }
        }
        return ret;
    }

    public static Power PushPower(Power power, int x, int y, int w, int h)
    {
        var outputs = ScanForConsumers(x, y, w, h);
        var powerPer = power.power / outputs.Count;
        foreach (var output in outputs)
        {
            var thisPower = power.Remove(powerPer);
            Main.NewText($"Removed power: {thisPower}");
            foreach (var slot in output.GetInputSlotsForConnector(null))
            {
                slot.Insert(thisPower);
                Main.NewText($"Inserted power {thisPower} to power {slot}");
                if (thisPower.IsEmpty()) break;
            }
            if (thisPower.IsEmpty()) continue;
            Main.NewText($"Power was not fully inserted: {thisPower}");
            power.Insert(thisPower);
        }
        return power;
    }
}