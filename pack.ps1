$outputDir = ".\.package\"
$version = "1.0.3"

dotnet build --configuration Release /p:Version=$version
dotnet pack  --configuration Release --output $outputDir /p:Version=$version --no-build