
// using UnityEngine;

// public class MixingManager : MonoBehaviour
// {
//     public void RedistributeColors()
//     {
//         List<ColorVector> allColors = new List<ColorVector>();

//         foreach (var tile in allTiles)
//         {
//             while (tile.HasColors())
//             {
//                 allColors.Add(tile.PopTopColor());
//             }
//         }

//         Shuffle(allColors);

//         int index = 0;
//         int stackSizePerTile = allColors.Count / allTiles.Count;

//         foreach (var tile in allTiles)
//         {
//             for (int i = 0; i < stackSizePerTile && index < allColors.Count; i++)
//             {
//                 tile.PushColor(allColors[index++]);
//                 tile.UpdateVisual();
//             }
//         }


//     }

    
// }