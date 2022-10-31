$apiPath = "$pwd/Interviews.RetailInMotion.WebApi/Interviews.RetailInMotion.WebApi.csproj"
$releaseFolder = "$pwd/Interviews.RetailInMotion.WebApi/release"

dotnet build $apiPath
dotnet publish $apiPath --output $releaseFolder --sc true --runtime "win-x64"

Start-Process "$releaseFolder/Interviews.RetailInMotion.WebApi.exe"