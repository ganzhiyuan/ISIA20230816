IIS MIME Types Setting
.config test/xml
.db application/x-msdownload
.licenses application/x-msdownload
.mdb application/vnd.ms-access

IIS Request Filtering Setting
.mdb delete

web.config 에 추가
<configuration>
  <system.web>
    <globalization fileEncoding="utf-8" responseEncoding="utf-8" />
  </system.web>
</configuration>