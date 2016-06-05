   <This software is meant to test storage access speeds in Azure, all
	associated costs of using the requisite azure supplies are covered by
	the end user.>
    Copyright (C) <2016>  <Jonathan Godbout>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
	-----------------------------Setup-------------------------------------------
	To use this software you will have to do some pre-setup on the azure platform
	as well as copying the related config items in the App.config file. Please
	note we show you where in the App.config below, don't actually put them below.

	First you will need to set up an Azure storage account and set the storage 
	account name and key:
	<add key="tableStorage" value="" />
    <add key="tableKey" value="" />

	Next you will have to set up a redis store and put the connection string in:
    <add key="redis" value="" />

	Finally you will have to make a sql database in Azure and put the connection string:
	<add name="testDBEntities" connectionString="" providerName="" />

	For ease we use the Entity Framework. 

	This will create the specified table in your database with the following command:
    
	ctx.Database.ExecuteSqlCommand(
    IF  NOT EXISTS (SELECT * FROM sys.objects" +
    WHERE object_id = OBJECT_ID(N'[dbo].[YourTable]') AND type in (N'U'))" +
    BEGIN" +
    CREATE TABLE[dbo].[YourTable](" +
    RowKey VARCHAR(255) NOT NULL PRIMARY KEY,
    Guid1 UNIQUEIDENTIFIER,
    Guid2 UNIQUEIDENTIFIER,
    String1 VARCHAR(255),
    String2 VARCHAR(255),
    Int1 INT,
    Int2 INT
    )
    END