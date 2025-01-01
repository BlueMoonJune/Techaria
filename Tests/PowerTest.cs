namespace Techaria.Tests;

public class PowerTest : ModSystem
{
    public bool testsDone = false;
    public override void PostUpdateEverything()
    {
        if (testsDone) return;
        testsDone = true;

        var slot = new Power(900, 1000);
        var transfer = new Power(500, 0);

        var result = slot.Insert(transfer);

        if (slot.power != 1000)
        {
            Main.NewText($"[c/FF1919:Power Test failed, slot.power == {slot.power}, expected 1000]");
        } else if (transfer.power != 400)
        {
            Main.NewText($"[c/FF1919:Power Test failed, transfer.power == {transfer.power}, expected 400]");
        }
        else
        {
            Main.NewText("[c/32ff82:Power Test successful!");
        }

    }
}