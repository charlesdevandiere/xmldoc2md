FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY . ./

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM publish AS final
WORKDIR /app
COPY --from=publish /app/publish .