# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "CountdownApi.dll"]