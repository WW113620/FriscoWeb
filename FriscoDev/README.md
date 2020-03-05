### Data Base

#### Frisco
"data source=tcp:pxvajz11qz.database.windows.net,1433;initial catalog=PMGDATABASE;user id=REACT@pxvajz11qz;pwd=AciFrisco@123"

#### Abilene
"Data Source=tcp:pxvajz11qz.database.windows.net,1433;Initial Catalog=Abilene;User ID=REACT@pxvajz11qz;Password=AciFrisco@123"

### FTP Address

#### Frisco

``` xml
<publishData>
  <publishProfile profileName="StalkerFrisco - Web Deploy"
                  publishMethod="MSDeploy"
                  publishUrl="stalkerfrisco.scm.azurewebsites.net:443"
                  msdeploySite="StalkerFrisco"
                  userName="$StalkerFrisco"
                  userPWD="pGDtKdXYBxYHXlZjgXSK9GbDJdlLxeKrXcA36nBidlaxlLgXDkjFmos8duaL"
                  destinationAppUrl="http://stalkerfrisco.azurewebsites.net"
                  SQLServerDBConnectionString=""
                  mySQLDBConnectionString=""
                  hostingProviderForumLink=""
                  controlPanelLink="http://windows.azure.com"
                  webSystem="WebSites">
    <databases/>
  </publishProfile>
  <publishProfile profileName="StalkerFrisco - FTP"
                  publishMethod="FTP"
                  publishUrl="ftp://waws-prod-dm1-001.ftp.azurewebsites.windows.net/site/wwwroot"
                  ftpPassiveMode="True"
                  userName="StalkerFrisco\$StalkerFrisco"
                  userPWD="pGDtKdXYBxYHXlZjgXSK9GbDJdlLxeKrXcA36nBidlaxlLgXDkjFmos8duaL"
                  destinationAppUrl="http://stalkerfrisco.azurewebsites.net"
                  SQLServerDBConnectionString=""
                  mySQLDBConnectionString=""
                  hostingProviderForumLink=""
                  controlPanelLink="http://windows.azure.com"
                  webSystem="WebSites">
    <databases/>
  </publishProfile>
</publishData>
```

#### Abilence

```xml
<publishData>
   <publishProfile profileName="StalkerAbliene - Web Deploy"
                   publishMethod="MSDeploy"
                   publishUrl="stalkerabliene.scm.azurewebsites.net:443"
                   msdeploySite="StalkerAbliene"
                   userName="$StalkerAbliene"
                   userPWD="9RfubWhLPDQdAnHx6E5yABf1oE24buor96lRPnJ2JcKyyHl9fClyBbeRtyhD"
                   destinationAppUrl="http://stalkerabliene.azurewebsites.net"
                   SQLServerDBConnectionString="" 
                   mySQLDBConnectionString=""
                   hostingProviderForumLink=""
                   controlPanelLink="http://windows.azure.com"
                   webSystem="WebSites">
      <databases />
   </publishProfile>
   <publishProfile profileName="StalkerAbliene - FTP"
                   publishMethod="FTP"
                   publishUrl="ftp://waws-prod-dm1-001.ftp.azurewebsites.windows.net/site/wwwroot"
                   ftpPassiveMode="True"
                   userName="StalkerAbliene\$StalkerAbliene"
                   userPWD="9RfubWhLPDQdAnHx6E5yABf1oE24buor96lRPnJ2JcKyyHl9fClyBbeRtyhD"
                   destinationAppUrl="http://stalkerabliene.azurewebsites.net"
                   SQLServerDBConnectionString=""
                   mySQLDBConnectionString=""
                   hostingProviderForumLink=""
                   controlPanelLink="http://windows.azure.com"
                   webSystem="WebSites">
      <databases />
   </publishProfile>
</publishData>
```

### Site

#### Frisco

Address:http://stalkerfrisco.azurewebsites.net/

UserName:friscoadmin

UserPwd:a1b2c3

#### Abilence

Address:http://stalkerabliene.azurewebsites.net

UserName:friscoadmin@gmail.com

UserPwd:a1b2c3
