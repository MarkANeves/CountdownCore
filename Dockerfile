FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /

RUN ls

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Debug -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
#WORKDIR /
COPY --from=build-env /out .
EXPOSE 80
ENTRYPOINT ["dotnet", "CountdownApi.dll"]