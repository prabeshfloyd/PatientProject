An experimental start to an RESTful API with RPC search for Test Patient Project.

Development Setup
=================

Create/Update Database
----------------------

The script located in ..\PatientProject\Scripts\Database.sql needs to run on the SQL server that runs the database.
The connection string would follow this formatting

`<add connectionString="Initial Catalog=PatientProject;Data Source=prabeshAILocalDB;trusted_connection=yes" name="PatientProject" providerName="System.Data.SqlClient" />`.

Getting Started
---------------
Once everything is configured how you want it, set the `PatientProject` project as your startup project and run it.  

The API should be available at: `http://localhost:[port]/api/`.

This project is also using [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle) to add a [SwaggerUI](http://swagger.io/swagger-ui/) page.  This can be accessed at: `http://localhost:[port]/swagger/`.  

The Swagger UI can be accessed at `http://localhost:[port]/Swagger`.

Tests
=====
There are test projects in this solution: 

* `These can be accessed by running the tests from the respective projects or by having a continous integration or a build engine run it for you.` 

Libraries
=========

For the complete list of NuGet packages installed, see the [WebApi project's packages.config].


Architecture
============
It's very easy for software architecture to get out of control and turn what could be a simple product into a giant, complicated mess.  So this section is here to look at some decisions that have to be made and provide justifications for them.  Maybe taking the time to justify decisions will help avoid bad ones?  It's worth a try anyway.

This is a WebApi project that has a RPC for performing a patient search.


.NET Framework
---------------

At this point, I think it's best to stick with the more stable ASP.NET Web API 2.  But if most of the real logic is in a class library instead of in the Web API, then it probably wouldn't be too difficult to make the transition to ASP.NET Core in the future.


Code Standards
--------------
The Microsoft standards for naming conventions in C# are pretty universally followed: 

* [Capitalization Conventions](https://msdn.microsoft.com/en-us/library/ms229043(v=vs.110).aspx)

All of the .NET standard libraries follow those conventions, so you probably want your code to follow them as well so your code doesn't look out of place or misleading.  A quick summary:

* **pascalCase** for non-public fields, local variables, parameters: `length`, `maxLength`
* **CamelCase** for pretty much everything else: `StringBuilder`, `ToString()`, `Length`

Authentication
--------------

This does not use authentication at this point.

Logging
--------------

The actual logging work is abstracted away behind an interface: [ILogger](PatientProject/Logger.cs).
