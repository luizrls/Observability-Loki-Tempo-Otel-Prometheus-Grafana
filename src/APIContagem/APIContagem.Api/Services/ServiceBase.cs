using APIContagem.Api.Configurations.Tracings;
using System.Runtime.InteropServices;

namespace APIContagem.Api.Services
{
    public class ServiceBase
    {
        private static readonly string _FRAMEWORK;
        private static readonly string _KERNEL;
        private static readonly string _LOCAL;

        static ServiceBase()
        {
            _LOCAL = OpenTelemetryExtensions.ServiceName;
            _KERNEL = Environment.OSVersion.VersionString;
            _FRAMEWORK = RuntimeInformation.FrameworkDescription;
        }

        public string Framework { get => _FRAMEWORK; }
        public string Kernel { get => _KERNEL; }
        public string Local { get => _LOCAL; }
    }
}