@echo off

::create nuget_pub
if not exist nuget_pub (
    md nuget_pub
)

::clear nuget_pub
for /R "nuget_pub" %%s in (*) do (
    del %%s
)

::Standard
dotnet pack src/Cosmos -c Release -o nuget_pub
dotnet pack src/Cosmos.Extensions -c Release -o nuget_pub
dotnet pack src/Cosmos.Extensions.Asyncs -c Release -o nuget_pub
dotnet pack src/Cosmos.Extensions.Domain -c Release -o nuget_pub
dotnet pack src/Cosmos.Extensions.DateTime -c Release -o nuget_pub
dotnet pack src/Cosmos.Extensions.Collections -c Release -o nuget_pub
dotnet pack src/Cosmos.Extensions.Optionals -c Release -o nuget_pub
dotnet pack src/Cosmos.Extensions.Preconditions -c Release -o nuget_pub
dotnet pack src/Cosmos.Extensions.Reflection -c Release -o nuget_pub
dotnet pack src/Cosmos.Abstractions -c Release -o nuget_pub
dotnet pack src/Cosmos.Standard -c Release -o nuget_pub

for /R "nuget_pub" %%s in (*symbols.nupkg) do (
    del %%s
)

echo.
echo.

set /p key=input key:
set source=https://www.myget.org/F/alexinea/api/v2/package

for /R "nuget_pub" %%s in (*.nupkg) do ( 
    call nuget push %%s %key% -Source %source%	
	echo.
)

pause