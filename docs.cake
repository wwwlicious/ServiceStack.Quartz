
var docFile = "./docs.csproj";

Task("RunDocs")
.Does(() => {
    DotNetCoreRestore(docFile);
    DotNetCoreTool(docFile, "stdocs", "run");
});

Task("UpdateDocs")
.Does(() => {
    DotNetCoreRestore(docFile);
    DotNetCoreTool(docFile, "stdocs", "export ./docs ProjectWebsite -p ServiceStack.Quartz");
});
