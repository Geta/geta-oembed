$outputDir = ".\.package\"
$version = "2.1.0"

dotnet build --configuration Release /p:Version=$version
dotnet pack  --configuration Release --output $outputDir /p:Version=$version --no-build