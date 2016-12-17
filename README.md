IDEAL Furniture
===============

IDEAL Furniture is a prototype Windows Desktop application that displays charts for a Furniture store.
The application goal is to visually represent the current status of the accounts using charts. 
The information of the accounts will come from a SQL database.
All components are intended to use free Technology.


Notes
-----
### Technology
- The application was developed using Microsoft Visual Sudio Community 2015.
- The application uses C# and Windows Presentation Fundation (WPF).
- The database use Microsoft SQL Server 2016 Express or a local database.

### Menu of pages
- Menu of pages is based on project  https://github.com/Microsoft/WPF-Samples Graphics/Brushes.

### Connection to the database
- Connection to Microsoft SQL Server 2016 is using ADO.NET Entity Framework
  as described in Beginning C# 6.0 Programming with Visual Studio 2015.pdf book pj 670.
```
  a) Add the Entity Framework using NuGet.
     Go to Tools ➪ NuGet Package Manager ➪ Manage NuGetPackages for Solution.
  b) In the NuGet Package Manager, choose the Entity Framework, 
     uncheck the Include Prerelease checkbox and get the Entity Framework latest stable release. 
	 Click the Install button. Click OK on the Preview Changes and the I Accept button for the 
	 License Acceptance dialog.
  c) Add a connection to the database
     Project ➪ Add New Item. 
	 Choose ADO.NET Entity Data Model in the Add New Item dialog and change the name from Model1 to IdealContext
	 Code First from database -> New connection to ideal.dbo -> Select all Tables and Views. 
	 This will generate classes for each item. 
	 Create a folder 'DatabaseModel' and move all items there.

    NOTES: To connect to SQL Server Express add a new connection using
           .\sqlexpress in the server name the select ideal.db usign Windows Authentication.

  d) Add the using libraries to the *.cs file
     using System.Data.Entity;
     using System.ComponentModel.DataAnnotations;
``` 

- To handle DATABASE MIGRATIONS usign the Entity Framework.
```
  "An unhandled exception of type 'System.InvalidOperationException' occurred in EntityFramework.dll
   Additional information: The model backing the 'BookContext' has changed since the database was created.
   Consider using Code First Migrations to update the database (http://go.microsoft.com/fwlink/?LinkId=238269)."

  As the error message suggests, you need to add the Code First Migrations package to your program.

  a) go to Tools ➪ NuGet Package Manager ➪ Package Manager Console
  b) To enable automatic migration of your database to your updated class structure, enter this command
     in the Package Manager Console at the PM> prompt:
      Enable-Migrations –EnableAutomaticMigrations

  This adds a Migrations class to your project. Migration/Configuration.cs
  The Entity Framework will compare the timestamp of the database to your program and advise you
  when the database is out of sync with your classes. To update the database, simply enter this command
  in the Package Manager Console at the  PM> prompt:  Update-Database
```



License
-------
Copyright 2016 Angel Garcia

Licensed to the Apache Software Foundation (ASF) under one or more contributor
license agreements.  See the NOTICE file distributed with this work for
additional information regarding copyright ownership.  The ASF licenses this
file to you under the Apache License, Version 2.0 (the "License"); you may not
use this file except in compliance with the License.  You may obtain a copy of
the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the
License for the specific language governing permissions and limitations under
the License. 