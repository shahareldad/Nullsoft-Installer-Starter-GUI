namespace InstallerGUI.Contracts
{
    public interface IHoldGeneralInformation
    {
        string ApplcationName { get; set; }

        string OutputFilename { get; set; }

        string DestinationFolder { get; set; }

        string FileVersion { get; set; }

        string ProductVersion { get; set; }

        string ProductName { get; set; }

        string CompanyName { get; set; }

        string FileDescription { get; set; }

        string LegalCopyright { get; set; }
    }
}