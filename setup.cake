#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            nuGetSources: new string[] { "http://myget.org/f/servicestack","https://www.nuget.org/api/v2","https://api.nuget.org/v3/index.json" },
                            title: "ServiceStack.Quartz",
                            repositoryOwner: "wwwlicious",
                            repositoryName: "ServiceStack.Quartz",
                            appVeyorAccountName: "wwwlicious",
                            shouldPostToGitter: false,
                            shouldPostToSlack: false,
                            shouldPostToTwitter: false,
                            shouldRunDupFinder: false
                            );

BuildParameters.PrintParameters(Context);


ToolSettings.SetToolSettings(context: Context,
                        dupFinderExcludePattern: new string[] { 
                            BuildParameters.RootDirectoryPath + "**/bin/**/*.*",
                            BuildParameters.RootDirectoryPath + "**/obj/**/*.*",
                        },
                        testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* -[FakeItEasy]* -[FluentAssertions]* -[FluentAssertions.Core]*",
                        testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
                        testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");
Build.RunDotNetCore();