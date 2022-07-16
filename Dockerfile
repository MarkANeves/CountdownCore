# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY /out .
EXPOSE 80
ENTRYPOINT ["dotnet", "CountdownApi.dll"]