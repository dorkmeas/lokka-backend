$ErrorActionPreference = "Stop"
Write-Host "Checking .NET SDK..." -ForegroundColor Cyan
dotnet --version
if ($LASTEXITCODE -ne 0) { throw ".NET 10 SDK is required." }

Write-Host "Starting PostgreSQL..." -ForegroundColor Cyan
docker compose up -d postgres
if ($LASTEXITCODE -ne 0) { throw "Could not start PostgreSQL. Check Docker Desktop." }

Write-Host "Restoring packages..." -ForegroundColor Cyan
dotnet restore .\SocialNetwork.sln
if ($LASTEXITCODE -ne 0) { throw "dotnet restore failed." }

Write-Host "Building solution..." -ForegroundColor Cyan
dotnet build .\SocialNetwork.sln --no-restore
if ($LASTEXITCODE -ne 0) { throw "dotnet build failed." }

Write-Host "Initialization complete. Run: dotnet run --project .\src\SocialNetwork.Api" -ForegroundColor Green
