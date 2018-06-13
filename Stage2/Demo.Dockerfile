FROM microsoft/aspnetcore:2.0.8 AS base
WORKDIR /app
EXPOSE 80
LABEL description="DevOpsDemo"

FROM microsoft/aspnetcore-build:2.0.5-2.1.4 AS build 

ENV http_proxy='http://10.62.1.22:3128'
ENV https_proxy='http://10.62.1.22:3128'

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
WORKDIR ./src/ProducerConsumerExample
# FROM build as debug
# RUN dotnet build -c Debug -o /app

FROM build AS publish
RUN dotnet restore
RUN dotnet build -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# inspect new workdir
RUN ls -a

ENTRYPOINT ["dotnet", "Example.ProducerConsumer.WebApi.dll"]