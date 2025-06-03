using NUnit.Framework;
using UnityEngine;

public class ColorManagerTests
{
    private ColorVector red;
    private ColorVector green;
    private ColorVector blue;
    private ColorVector yellow;

    [SetUp]
    public void Setup()
    {
        red = new ColorVector(1, 0, 0);
        green = new ColorVector(0, 1, 0);
        blue = new ColorVector(0, 0, 1);
        yellow = new ColorVector(1, 1, 0);
    }

    // [TestCase(1, 0, 0, 0, 1, 0, 1, 1, 0)]  // Red + Green = Yellow
    // [TestCase(1, 1, 0, 0, 0, 1, 1, 1, 1)]  // Yellow + Blue = White (white = 1,1,1)
    // [TestCase(1, 0, 0, 1, 0, 0, 1, 0, 0)]  // Red + Red = Red
    // [TestCase(0, 0, 0, 0, 0, 0, 0, 0, 0)]  // Black + Black = Invalid/Black
    // public void CombineColors_ShouldReturnExpected(
    //     int r1, int g1, int b1,
    //     int r2, int g2, int b2,
    //     int expectedR, int expectedG, int expectedB)
    // {
    //     var color1 = new ColorVector(r1, g1, b1);
    //     var color2 = new ColorVector(r2, g2, b2);
    //     var expected = new ColorVector(expectedR, expectedG, expectedB);

    //     var result = ColorManager.CombineColors(color1, color2);

    //     Assert.AreEqual(expected, result);
    // }

    // [Test]
    // public void ColorCombineHandler_WhiteColor_DeletesColor()
    // {
    //     var goA = new GameObject("TileA");
    //     var goB = new GameObject("TileB");
    //     var tileA = goA.AddComponent<Tile>();
    //     var tileB = goB.AddComponent<Tile>();

    //     tileA.PushColor(new ColorVector(1, 0, 0));
    //     tileB.PushColor(new ColorVector(0, 1, 0));

    //     var handler = new ColorCombineHandler();

    //     var combinedColor = ColorManager.CombineColors(tileA.PeekColor(), tileB.PeekColor());

    //     if (combinedColor.IsWhite)
    //     {
    //         handler.OnWhiteCombine(new TileEvents.WhiteColorFormedEvent(tileB));
    //         Assert.AreEqual(0, tileB.ColorStack.Count);
    //     }
    //     }


    // [Test]
    // public void WhiteColorEvent_Dispatches_And_Handler_Reacts()
    // {
    //     // Tile MonoBehaviour olduğu için GameObject üzerinden oluşturulmalı
    //     var tileGO = new GameObject("Tile");
    //     var tile = tileGO.AddComponent<Tile>();
    //     tile.PushColor(new ColorVector(1, 1, 1)); // Beyaz

    //     // ColorCombineHandler MonoBehaviour olarak eklenmeli
    //     var handlerGO = new GameObject("Handler");
    //     var handler = handlerGO.AddComponent<ColorCombineHandler>();

    //     // Initialize() çağrılmazsa Subscribe çalışmaz
    //     handler.Initialize();

    //     // Event tetikleniyor
    //     EventBus.Publish(new TileEvents.WhiteColorFormedEvent(tile));

    //     // Handler beyaz rengi sildiyse, color count sıfır olmalı

    //     Assert.AreEqual(0, tile.ColorStack.Count);
    // }
    // [Test]
    // public void ColorCombinedEvent_WithIntermediateColor_AddsColorAndRemovesSourceColors()
    // {
    //     // Hazırla: Tile objeleri ve handler
    //     var sourceGO = new GameObject("SourceTile");
    //     var targetGO = new GameObject("TargetTile");

    //     var sourceTile = sourceGO.AddComponent<Tile>();
    //     var targetTile = targetGO.AddComponent<Tile>();

    //     // Örnek renkler (kırmızı + mavi = ara mor gibi düşün)
    //     var red = new ColorVector(1, 0, 0);
    //     var blue = new ColorVector(0, 0, 1);
    //     var intermediate = new ColorVector(1, 0, 1); // Ara renk örneği

    //     sourceTile.PushColor(red);
    //     targetTile.PushColor(blue);

    //     var handlerGO = new GameObject("Handler");
    //     var handler = handlerGO.AddComponent<ColorCombineHandler>();
    //     handler.Initialize();

    //     // Event'i elle tetikle (Unity EventBus ile)
    //     EventBus.Publish(new TileEvents.ColorCombinedEvent(
    //         sourceTile, targetTile, intermediate
    //     ));

    //     // Testler:
    //     // 1. Kaynak ve hedeften birer renk çıkarılmış olmalı
    //     Assert.AreEqual(0, sourceTile.ColorStack.Count, "Source tile should have no colors after combination");
    //     Assert.AreEqual(0, targetTile.ColorStack.Count, "Target tile should have no colors before pushing intermediate");

    //     // 2. Hedef tile’a ara renk eklenmiş olmalı
    //     Assert.AreEqual(1, targetTile.ColorStack.Count, "Target tile should have 1 color (the intermediate) after combination");
    //     Assert.AreEqual(intermediate, targetTile.PeekColor(), "Target tile's top color should be the intermediate color");
    // }

    [Test]
    public void ColorCombinedEvent_RemovesColorsFromSourceAndTarget()
    {
        var sourceGO = new GameObject("SourceTile");
        var targetGO = new GameObject("TargetTile");

        var sourceTile = sourceGO.AddComponent<Tile>();
        var targetTile = targetGO.AddComponent<Tile>();

        var red = new ColorVector(1, 0, 0);
        var blue = new ColorVector(0, 0, 1);

        sourceTile.PushColor(red);
        targetTile.PushColor(blue);

        var intermediate = new ColorVector(1, 0, 1);

        var handlerGO = new GameObject("Handler");
        var handler = handlerGO.AddComponent<ColorCombineHandler>();
        handler.Initialize();

        // Event yayınla
        EventBus.Publish(new TileEvents.ColorCombinedEvent(sourceTile, targetTile, intermediate));

        // Renkler çıkarılmış olmalı (pushColor ara renk eklemeden önce)
        Assert.AreEqual(0, sourceTile.ColorStack.Count, "Source tile should have no colors after combination");
        Assert.AreEqual(1, targetTile.ColorStack.Count, "Target tile should have no colors before pushing intermediate color");
    }

    // [Test]
    // public void ColorCombinedEvent_AddsIntermediateColorToTarget()
    // {
    //     var sourceGO = new GameObject("SourceTile");
    //     var targetGO = new GameObject("TargetTile");

    //     var sourceTile = sourceGO.AddComponent<Tile>();
    //     var targetTile = targetGO.AddComponent<Tile>();

    //     var red = new ColorVector(1, 0, 0);
    //     var blue = new ColorVector(0, 0, 1);

    //     sourceTile.PushColor(red);
    //     targetTile.PushColor(blue);

    //     var intermediate = new ColorVector(1, 0, 1);

    //     var handlerGO = new GameObject("Handler");
    //     var handler = handlerGO.AddComponent<ColorCombineHandler>();
    //     handler.Initialize();

    //     // Event yayınla
    //     EventBus.Publish(new TileEvents.ColorCombinedEvent(sourceTile, targetTile, intermediate));

    //     // Ara renk eklenmeli
    //     Assert.AreEqual(1, targetTile.ColorStack.Count, "Target tile should have 1 color (the intermediate) after combination");
    //     Assert.AreEqual(intermediate, targetTile.PeekColor(), "Target tile's top color should be the intermediate color");
    // }


}
