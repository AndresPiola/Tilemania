
using System.Globalization;
using Google;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Voodoo.Sauce.Internal.Editor
{
    public class BasePrebuild : IPreprocessBuildWithReport
    {
        private const float MinIosVersion = 9.0f;
        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            BuildTarget target = report.summary.platform;

            if (target == BuildTarget.iOS)
            {
                // force Play Services Resolver to generate Xcode project and not workspace
                IOSResolver.CocoapodsIntegrationMethodPref = IOSResolver.CocoapodsIntegrationMethod.Project;
                //set iOS CPU Architecture to Universal 
                PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 2);
                // set iOS Min Version
                float iosMinVersion;
                var changeMinVersion = true;
                if (float.TryParse(PlayerSettings.iOS.targetOSVersionString, out iosMinVersion))
                {
                    if (iosMinVersion >= MinIosVersion)
                    {
                        changeMinVersion = false;  
                    }
                       
                }
                if (changeMinVersion)
                {
                    PlayerSettings.iOS.targetOSVersionString = MinIosVersion.ToString(CultureInfo.InvariantCulture);
                }
            }

        }
    }
}
