FROM microsoft/dotnet:2.2.0-runtime
WORKDIR /dotnetapp
COPY ./bin/Docker .
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT dotnet Fibon.Api.dll --urls "http://*:5000"