///////////////////////////////////////////////////////////////////////////////
// ENVIRONMENT VARIABLES
///////////////////////////////////////////////////////////////////////////////

var envNugetServer = EnvironmentVariable("NUGET_SERVER");
var envNugetApiKey = EnvironmentVariable("NUGET_APIKEY");
var envBuildNumber = EnvironmentVariable<int>("build", 0);

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target          = Argument<string>("target", "Default");
var configuration   = Argument<string>("configuration", "Debug");
var nugetServer     = Argument<string>("NugetSource", envNugetServer);
var nugetApiKey     = Argument<string>("NugetApiKey", envNugetApiKey);
var verbosity       = Argument<string>("Verbosity", "Verbose");
var buildNumber     = Argument<int>("build", envBuildNumber);

//////////////////////////////////////////////////////////////////////
// TOOLS AND ADDINS
//////////////////////////////////////////////////////////////////////

#tool "nuget:?package=GitVersion.CommandLine&version=3.6.5"
#addin "nuget:?package=Cake.Incubator&version=1.6.0"
#addin "nuget:?package=Cake.Git&version=0.16.1"

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var gitVersionResults = GitVersion(new GitVersionSettings { UpdateAssemblyInfo = false });
var semVersion = gitVersionResults.MajorMinorPatch + "." + buildNumber;
var solutionFile = new FilePath("ServiceStack.Quartz.sln");
var solution = ParseSolution(solutionFile);

Information("Semver Version -> {0}", semVersion);

// folders
var srcDir              = Directory("./src");
var artifactsDir        = Directory("./artifacts");
var nupkgDestDir        = artifactsDir + Directory("nuget-package");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
 
 Task("__Clean")
    .Does(() => {
    CleanDirectories(new DirectoryPath[] {
        artifactsDir,
        nupkgDestDir
    });

    Information("Cleaning {0}", srcDir);
    CleanDirectories(srcDir.ToString() + "/**/bin");
    CleanDirectories(srcDir.ToString() + "/**/obj");
});

Task("__Build")
    .IsDependentOn("__Clean")
    .Does(() => {
    
    Information("Restoring Packages {0}", solutionFile);
    DotNetCoreRestore(solutionFile.FullPath);

    // build the solution with msbuild task
    Information("Building {0}", solutionFile);
    var msbuildBinaryLogFile = artifactsDir + new FilePath(solutionFile.GetFilenameWithoutExtension() + ".binlog");
    
    // TODO Try netcore build instead.
    var settings = new DotNetCoreMSBuildSettings();
    settings.SetConfiguration(configuration);
    settings.SetVersion(semVersion);
    settings.SetWarningCodeAsError("3884");
    settings.ShowDetailedSummary();
    settings.WithProperty("PackageOutputPath",MakeAbsolute(nupkgDestDir).FullPath);
    settings.ArgumentCustomization = arguments => {
                arguments.Append(string.Format("/bl:{0}", msbuildBinaryLogFile));
                return arguments;
    };

    DotNetCoreMSBuild(solutionFile.ToString(), settings);
});

Task("__Test")
    .Does(() => {
        foreach(var project in solution.Projects){
            if(project.Name.EndsWith("test", StringComparison.OrdinalIgnoreCase) ||
                project.Name.EndsWith("tests", StringComparison.OrdinalIgnoreCase)){
                DotNetCoreTest(project.Path.ToString());
            }
        }
});   

Task("Build")
    .IsDependentOn("__Clean")
    .IsDependentOn("__Build")    
    .IsDependentOn("__Test");

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);