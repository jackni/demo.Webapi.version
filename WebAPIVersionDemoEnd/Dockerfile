# define the base image, smaller is better.
FROM microsoft/aspnetcore:2.0.5 AS base

WORKDIR /app
EXPOSE 80
LABEL description="This a demo"
# define build image, image contains sdk and all the stuff we need.
# this is big image
FROM microsoft/aspnetcore-build:2.0.5-2.1.4 AS build 
# specify the dir inside container. eg. if the current code are in root folder called src then /src
# WORKDIR is container context swith..
WORKDIR /src
RUN ls -a
COPY . ./src
# after the copy the current content is copyed into container's ./src folder.
RUN ls -a
# let's inspect
RUN ls ./src -a
# swith container context to the build target.
WORKDIR ./src/Demo.WebAPIVersion
RUN dotnet build -c Debug -o /app

FROM build AS publish
RUN dotnet restore
RUN dotnet build -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# inspect new workdir
RUN ls -a

ENTRYPOINT ["dotnet", "Demo.WebAPIVersion.dll"]