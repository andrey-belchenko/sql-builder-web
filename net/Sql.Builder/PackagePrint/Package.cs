using System.Xml.Linq;

namespace sql.builder.PackagePrint
{
    public class Package
    {
        string Name { get; set; }
        PackageVisibility Visibility { get; set; }

        PackageReport[] Reports { get; set; }
    }

    public class PackageReport
    {
        string RepName { get; set; }
        string Template { get; set; }
        XElement Settings { get; set; }
    }

    public enum PackageVisibility
    {
        ForUser,
        ForAll
    }
}