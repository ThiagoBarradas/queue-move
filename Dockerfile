## APP BUILDER
FROM microsoft/dotnet:2.2-runtime

# Default Environment 
ENV CURRENT_VERSION="__[Version]__"

# Args
ARG distFolder=QueueMove/bin/Release/netcoreapp2.2
ARG appFile=QueueMove.dll
 
# Copy files to /app
RUN ls
COPY ${distFolder} /app

# Run application
WORKDIR /app
RUN ls
ENV appFile=$appFile
ENTRYPOINT dotnet $appFile