using System;
using System.Linq;
using SharpDX.DXGI;

namespace OsrStudio.Windows
{
    public class HardwareInfoService : IHardwareInfoService
    {
        public GpuVendor GpuVendor { get; }
        public string GpuName { get; }
        public bool HasAmdEncoder { get; }
        public bool HasNvidiaEncoder { get; }
        public bool HasIntelQuickSync { get; }

        public HardwareInfoService()
        {
            try
            {
                using var factory = new Factory1();
                var adapter = factory.Adapters1.FirstOrDefault();
                
                if (adapter != null)
                {
                    GpuName = adapter.Description.Description ?? "Unknown GPU";
                    GpuVendor = DetectVendor(adapter);
                    
                    // Set encoder availability based on vendor
                    HasAmdEncoder = GpuVendor == GpuVendor.AMD;
                    HasNvidiaEncoder = GpuVendor == GpuVendor.NVIDIA;
                    HasIntelQuickSync = GpuVendor == GpuVendor.Intel;
                }
                else
                {
                    GpuName = "No GPU detected";
                    GpuVendor = GpuVendor.Unknown;
                    HasAmdEncoder = false;
                    HasNvidiaEncoder = false;
                    HasIntelQuickSync = false;
                }
            }
            catch (Exception)
            {
                // If detection fails, default to unknown and allow all encoders
                // (better to show too many options than too few)
                GpuName = "Detection failed";
                GpuVendor = GpuVendor.Unknown;
                HasAmdEncoder = true; // Allow all when detection fails
                HasNvidiaEncoder = true;
                HasIntelQuickSync = true;
            }
        }

        private static GpuVendor DetectVendor(Adapter1 adapter)
        {
            var vendorId = adapter.Description.VendorId;
            
            // Common GPU vendor IDs
            // AMD: 0x1002
            // NVIDIA: 0x10DE
            // Intel: 0x8086
            
            return vendorId switch
            {
                0x1002 => GpuVendor.AMD,
                0x10DE => GpuVendor.NVIDIA,
                0x8086 => GpuVendor.Intel,
                _ => DetermineVendorFromName(adapter.Description.Description)
            };
        }

        private static GpuVendor DetermineVendorFromName(string gpuName)
        {
            if (string.IsNullOrEmpty(gpuName))
                return GpuVendor.Unknown;

            var lowerName = gpuName.ToLowerInvariant();
            
            if (lowerName.Contains("amd") || lowerName.Contains("radeon") || lowerName.Contains("ati"))
                return GpuVendor.AMD;
                
            if (lowerName.Contains("nvidia") || lowerName.Contains("geforce") || lowerName.Contains("quadro") || lowerName.Contains("tesla"))
                return GpuVendor.NVIDIA;
                
            if (lowerName.Contains("intel") || lowerName.Contains("hd graphics") || lowerName.Contains("uhd graphics") || lowerName.Contains("iris"))
                return GpuVendor.Intel;
            
            return GpuVendor.Unknown;
        }
    }
}
