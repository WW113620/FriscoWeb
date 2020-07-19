ALTER TABLE [PMD] DROP CONSTRAINT DF__PMD__Address__147C05D0
ALTER TABLE [PMD] ALTER COLUMN [Address] NVARCHAR(500)


http://abilene.azurewebsites.net
friscoadmin@gmail.com   a1b2c3

http://stalkerfrisco.azurewebsites.net
friscoadmin     a1b2c3
lidar           a1b2c3


UPDATE [PMD] SET Location='31.61260391119538,-95.37847992023775' WHERE IMSI='12345678901234567890'

<add name="ConnectionString"
 connectionString="Data Source=tcp:pxvajz11qz.database.windows.net;Initial Catalog=PMGDATABASE;User ID=REACT@pxvajz11qz;Password=AciFrisco@123"
 providerName="System.Data.EntityClient"/>

 Type (tinyint)

(1  Integer)
(2  String)
(3  Binary)

-- James
UPDATE [PMD] SET Connection=1 WHERE IMSI='89148000004701184353'


  CREATE TABLE [dbo].[CustomerAccount](
	[Id] [bigint] IDENTITY(1,1) NOT NULL primary key,
	[Email] nvarchar(200) NOT NULL,
	[PoliceDeptName] nvarchar(200) NULL,
	[Address] nvarchar(400) NULL,
	[ContactOffice] nvarchar(100) NULL,
	[ContactPhone] nvarchar(20) NULL,
	[AddTime] [datetime] NULL,
) ON [PRIMARY]