// using UnityEngine;
//
// namespace ClientKit.Handlers
// {
//     public static class ScreenResolution
//     {
//           /// <summary>
//         /// Return vector 2 resolution based on what you put as the preset
//         /// </summary>
//         /// <param name="screenshotResolution"></param>
//         /// <returns></returns>
//         public static Vector2 GetScreenResolution(GaiaConstants.ScreenshotResolution screenshotResolution, int customWidth = 1920, int customHeight = 1080)
//         {
//             Vector2 screenResolution = new Vector2(800, 600);
//             switch (screenshotResolution)
//             {
//                 case GaiaConstants.ScreenshotResolution.Resolution640X480:
//                     screenResolution.x = 640;
//                     screenResolution.y = 480;
//                     break;
//                 case GaiaConstants.ScreenshotResolution.Resolution800X600:
//                     screenResolution.x = 800;
//                     screenResolution.y = 600;
//                     break;
//                 case GaiaConstants.ScreenshotResolution.Resolution1280X720:
//                     screenResolution.x = 1280;
//                     screenResolution.y = 720;
//                     break;
//                 case GaiaConstants.ScreenshotResolution.Resolution1366X768:
//                     screenResolution.x = 1366;
//                     screenResolution.y = 768;
//                     break;
//                 case GaiaConstants.ScreenshotResolution.Resolution1600X900:
//                     screenResolution.x = 1600;
//                     screenResolution.y = 900;
//                     break;
//                 case GaiaConstants.ScreenshotResolution.Resolution1920X1080:
//                     screenResolution.x = 1920;
//                     screenResolution.y = 1080;
//                     break;
//                 case GaiaConstants.ScreenshotResolution.Resolution2560X1440:
//                     screenResolution.x = 2560;
//                     screenResolution.y = 1440;
//                     break;
//                 case GaiaConstants.ScreenshotResolution.Resolution3840X2160:
//                     screenResolution.x = 3840;
//                     screenResolution.y = 2160;
//                     break;
//                 case GaiaConstants.ScreenshotResolution.Resolution7680X4320:
//                     screenResolution.x = 7680;
//                     screenResolution.y = 4320;
//                     break;
//                 case GaiaConstants.ScreenshotResolution.Custom:
//                     screenResolution.x = customWidth;
//                     screenResolution.y = customHeight;
//                     break;
//             }
//
//             return screenResolution;
//         }
//     }
// }
