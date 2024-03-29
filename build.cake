#addin "wk.StartProcess"
#addin "wk.ProjectParser"

using PS = StartProcess.Processor;
using ProjectParser;

var name = "Circle";
var publishDir = ".publish/W";
var version = DateTime.Now.ToString("yy.MM.dd.HHmm");

Task("Publish").Does(() => {
    var settings = new DotNetCoreMSBuildSettings();
    settings.Properties["Version"] = new string[] { version };

    CleanDirectory(publishDir);
    DotNetCorePublish($"src/{name}", new DotNetCorePublishSettings {
        OutputDirectory = publishDir,
        MSBuildSettings = settings
    });
});

Task("Zip")
    .IsDependentOn("Publish")
    .Does(() => {
        CreateDirectory(".publish/Z");
        Zip(publishDir, $".publish/Z/circle-{version}.zip");
    });

var target = Argument("target", "Pack");
RunTarget(target);
