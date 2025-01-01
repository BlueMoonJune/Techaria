namespace Techaria;

public static class HelperMethods
{
   
    //absoluteAquarian being a god once again

    /// <summary>
    /// Atttempts to find the top-left corner of a multitile at location (<paramref name="x"/>, <paramref name="y"/>)
    /// </summary>
    /// <param name="x">The tile X-coordinate</param>
    /// <param name="y">The tile Y-coordinate</param>
    /// <returns>The tile location of the multitile's top-left corner, or the input location if no tile is present or the tile is not part of a multitile</returns>
    public static Point GetTopLeftTileInMultitile(int x, int y, out int width, out int height)
    {
        Tile tile = Main.tile[x, y];

        int frameX = 0;
        int frameY = 0;
        width = 1;
        height = 1;

        if (tile.HasTile)
        {
            int style = 0, alt = 0;
            TileObjectData.GetTileInfo(tile, ref style, ref alt);
            TileObjectData data = TileObjectData.GetTileData(tile.TileType, style, alt);

            if (data != null)
            {
                int size = 16 + data.CoordinatePadding;

                frameX = tile.TileFrameX % (size * data.Width) / size;
                frameY = tile.TileFrameY % (size * data.Height) / size;
                width = data.Width;
                height = data.Height;
            }
        }

        return new Point(x - frameX, y - frameY);
    } 
}