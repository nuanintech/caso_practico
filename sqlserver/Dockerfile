FROM mcr.microsoft.com/mssql/server:2022-latest

USER root

RUN apt-get update && \
    apt-get install -y curl gnupg apt-transport-https && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/22.04/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools && \
    ln -sfn /opt/mssql-tools/bin/sqlcmd /usr/bin/sqlcmd && \
    ln -sfn /opt/mssql-tools/bin/bcp /usr/bin/bcp && \
    apt-get clean && rm -rf /var/lib/apt/lists

ENV PATH="$PATH:/opt/mssql-tools/bin"

COPY entrypoint.sh /entrypoint.sh
COPY init.sql /init.sql

RUN chmod +x /entrypoint.sh

USER mssql

ENTRYPOINT ["/bin/bash", "/entrypoint.sh"]