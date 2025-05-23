// using UnityEngine;

// [CreateAssetMenu(fileName = "NewColorData", menuName = "ColorFuse/Color Data")]
// public class ColorDataSO : ScriptableObject
// {
//     [Tooltip("0 veya 1 kullanın")]
//     public int R;
//     public int G;
//     public int B;

//     public string colorName;

//     public Color ToUnityColor()
//     {
//         return new Color(R, G, B);
//     }

//     public bool IsWhite()
//     {
//         return R == 1 && G == 1 && B == 1;
//     }

//     public bool Equals(ColorDataSO other)
//     {
//         if (other == null) return false;
//         return R == other.R && G == other.G && B == other.B;
//     }
// }
