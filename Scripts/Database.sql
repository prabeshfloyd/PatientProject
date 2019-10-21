
USE MASTER;

DECLARE @DBName varchar(50) = 'Patient';

IF EXISTS (SELECT
		1
	FROM sys.databases
	WHERE NAME = @DBName)
BEGIN
DECLARE @SQL VARCHAR(MAX);

SELECT
	@SQL = COALESCE(@SQL, '') + 'Kill ' + CONVERT(VARCHAR, SPId) + ';'
FROM MASTER..SysProcesses
WHERE DBId = DB_ID(@DBName)
AND SPId <> @@SPId

EXEC (@SQL);
EXEC ('DROP DATABASE ' + @DBName);
END

EXEC ('CREATE DATABASE ' + @DBName);
GO

USE Patient;
GO

IF NOT EXISTS (SELECT
		schema_name
	FROM information_schema.schemata
	WHERE schema_name = 'Patient')
BEGIN
EXEC sp_executesql N'CREATE SCHEMA Patient';
END
GO

IF NOT EXISTS (SELECT
		schema_name
	FROM information_schema.schemata
	WHERE schema_name = 'Audit')
BEGIN
EXEC sp_executesql N'CREATE SCHEMA Audit';
END
GO

IF NOT EXISTS (SELECT
		TABLE_NAME
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_NAME = 'PatientMemberRecord'
	AND TABLE_SCHEMA = 'Patient')
BEGIN
CREATE TABLE Patient.PatientMemberRecord (
	[EnterpriseId] VARCHAR(506) PRIMARY KEY NOT NULL
   ,[Patient_ID] UNIQUEIDENTIFIER NOT NULL
   ,[Active] BIT NOT NULL CONSTRAINT [DF_Patient_PatientMemberRecord_Active] DEFAULT (1)
   ,[CreatedOn] [datetime] NULL CONSTRAINT [DF_Patient_PatientMemberRecord_CreatedOn] DEFAULT (GETUTCDATE())
   ,[ModifiedOn] [datetime] NULL CONSTRAINT [DF_Patient_PatientMemberRecord_ModifiedOn] DEFAULT (GETUTCDATE())
);
END
GO

IF NOT EXISTS (SELECT
		TABLE_NAME
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_NAME = 'Patient'
	AND TABLE_SCHEMA = 'Patient')
BEGIN
CREATE TABLE Patient.Patient (
	[ID] [uniqueidentifier] PRIMARY KEY NOT NULL
   ,[Patient_AddressID] [uniqueidentifier] NOT NULL
   ,[Source] VARCHAR(250) NULL
   ,[MedicalRecordNumber] VARCHAR(250) NOT NULL
   ,[FirstName] VARCHAR(250) NULL
   ,[LastName] VARCHAR(250) NULL
   ,[SocialSecurityNumber] VARCHAR(11) NULL
   ,[Active] BIT NOT NULL CONSTRAINT [DF_Patient_Patient_Active] DEFAULT (1)
   ,[CreatedOn] [datetime] NULL CONSTRAINT [DF_Patient_Patient_CreatedOn] DEFAULT (GETUTCDATE())
   ,[ModifiedOn] [datetime] NULL CONSTRAINT [DF_Patient_Patient_ModifiedOn] DEFAULT (GETUTCDATE())
);
END
GO

IF NOT EXISTS (SELECT
		TABLE_NAME
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_NAME = 'Address'
	AND TABLE_SCHEMA = 'Patient')
BEGIN
CREATE TABLE Patient.[Address] (
	[ID] [uniqueidentifier] PRIMARY KEY NOT NULL
   ,[Address1] VARCHAR(250) NULL
   ,[Address2] VARCHAR(250) NULL
   ,[City] VARCHAR(250) NULL
   ,[State] VARCHAR(50) NULL
   ,[ZipCode] VARCHAR(11) NULL
   ,[Active] BIT NOT NULL CONSTRAINT [DF_Patient_Address_Active] DEFAULT (1)
   ,[CreatedOn] [datetime] NULL CONSTRAINT [DF_Patient_Address_CreatedOn] DEFAULT (GETUTCDATE())
   ,[ModifiedOn] [datetime] NULL CONSTRAINT [DF_Patient_Address_ModifiedOn] DEFAULT (GETUTCDATE())

);
END
GO

IF OBJECT_ID('[Patient].[usp_searchPatient]', 'P') IS NOT NULL
DROP PROCEDURE [Patient].[usp_searchPatient];
GO

CREATE PROCEDURE [Patient].[usp_searchPatient] (
	@source VARCHAR(250)
	,@mrn VARCHAR(250)
	)
AS
BEGIN
SELECT
	*
FROM Patient.PatientMemberRecord pmr
INNER JOIN Patient.Patient pt
	ON pt.ID = pmr.Patient_ID
INNER JOIN Patient.[Address] adr
	ON adr.ID = pt.Patient_AddressID
WHERE pt.Source = @source
AND pt.MedicalRecordNumber = @mrn
AND pt.Active = 1
AND adr.Active = 1
AND pmr.Active = 1;

END
GO


IF OBJECT_ID('[Patient].[usp_getPatients]', 'P') IS NOT NULL
DROP PROCEDURE [Patient].[usp_getPatients];
GO

CREATE PROCEDURE [Patient].[usp_getPatients] 
AS
BEGIN
SELECT
	*
FROM Patient.PatientMemberRecord pmr
INNER JOIN Patient.Patient pt
	ON pt.ID = pmr.Patient_ID
INNER JOIN Patient.[Address] adr
	ON adr.ID = pt.Patient_AddressID
WHERE pt.Active = 1
AND adr.Active = 1
AND pmr.Active = 1;

END
GO


IF OBJECT_ID('[Patient].[usp_getPatient]', 'P') IS NOT NULL
DROP PROCEDURE [Patient].[usp_getPatient];
GO

CREATE PROCEDURE [Patient].[usp_getPatient] (
	@patientID VARCHAR(250)
	)
AS
BEGIN
SELECT
	*
FROM Patient.PatientMemberRecord pmr
INNER JOIN Patient.Patient pt
	ON pt.ID = pmr.Patient_ID
INNER JOIN Patient.[Address] adr
	ON adr.ID = pt.Patient_AddressID
WHERE pt.ID = @patientID
AND pt.Active = 1
AND adr.Active = 1
AND pmr.Active = 1;
END
GO

IF OBJECT_ID('[Patient].[usp_addPatient]', 'P') IS NOT NULL
DROP PROCEDURE [Patient].[usp_addPatient];
GO

CREATE PROCEDURE [Patient].[usp_addPatient]	(
	@Source VARCHAR(250)
	,@MedicalRecordNumber VARCHAR(250)
	,@FirstName VARCHAR(250)
	,@LastName VARCHAR(250)
	,@SocialSecurityNumber VARCHAR(11)
	,@Address1 VARCHAR(250)
	,@Address2 VARCHAR(250)
	,@City VARCHAR(250)
	,@State VARCHAR(50)
	,@ZipCode VARCHAR(250)
	)
AS
BEGIN

	DECLARE @AddressID UNIQUEIDENTIFIER; SET @AddressID = (SELECT NEWID());
	DECLARE @PatientID UNIQUEIDENTIFIER; SET @PatientID = (SELECT NEWID());
	DECLARE @EnterpriseID VARCHAR(506); SET @EnterpriseID = @source+'^EMPI^'+@MedicalRecordNumber;

INSERT INTO Patient.[Address]
	SELECT
		@AddressID
	   ,@Address1
	   ,@Address2
	   ,@City
	   ,@State
	   ,@ZipCode
	   ,'1'
	   ,GETUTCDATE()
	   ,NULL

INSERT INTO Patient.Patient
	SELECT
		@PatientID
	   ,@AddressID
	   ,@Source
	   ,@MedicalRecordNumber
	   ,@FirstName
	   ,@LastName
	   ,@SocialSecurityNumber
	   ,'1'
	   ,GETUTCDATE()
	   ,NULL

INSERT INTO Patient.PatientMemberRecord
	SELECT
		@EnterpriseID
	   ,@PatientID
	   ,'1'
	   ,GETUTCDATE()
	   ,NULL

END
GO

IF OBJECT_ID('[Patient].[usp_updatePatient]', 'P') IS NOT NULL
DROP PROCEDURE [Patient].[usp_updatePatient];
GO

CREATE PROCEDURE [Patient].[usp_updatePatient]
(
	@Source VARCHAR(250)
	,@MedicalRecordNumber VARCHAR(250)
	,@FirstName VARCHAR(250)
	,@LastName VARCHAR(250)
	,@SocialSecurityNumber VARCHAR(250)
	,@Address1 VARCHAR(250)
	,@Address2 VARCHAR(250)
	,@City VARCHAR(250)
	,@State VARCHAR(50)
	,@ZipCode VARCHAR(250)
	)
AS
BEGIN

	DECLARE @EnterpriseInternalId VARCHAR(506); SET @EnterpriseInternalId = @source+'^EMPI^'+@MedicalRecordNumber;
	DECLARE @PatientInternalID VARCHAR(250); SET @PatientInternalID = 'A';

SET @PatientInternalID = (SELECT
		Patient_ID
	FROM Patient.PatientMemberRecord
	WHERE EnterpriseId = @EnterpriseInternalId);

	IF (@PatientInternalID != 'A')
	BEGIN
UPDATE addr
SET Address1 = @Address1
   ,addr.Address2 = @Address2
   ,addr.City = @City
   ,addr.State = @State
   ,addr.ZipCode = @ZipCode
   ,addr.ModifiedOn = GETUTCDATE()
FROM Patient.Address addr
INNER JOIN Patient.Patient pt
	ON pt.Patient_AddressID = addr.ID
WHERE pt.ID = @PatientInternalID;

UPDATE pat
SET pat.FirstName = @FirstName
   ,pat.LastName = @LastName
   ,pat.SocialSecurityNumber = @SocialSecurityNumber
   ,pat.ModifiedOn = GETUTCDATE()
FROM Patient.Patient pat
WHERE pat.ID = @PatientInternalID;
END
ELSE
RETURN
END
GO


IF OBJECT_ID('[Patient].[usp_deletePatient]', 'P') IS NOT NULL
DROP PROCEDURE [Patient].[usp_deletePatient];
GO

CREATE PROCEDURE [Patient].[usp_deletePatient]
(
	@Source VARCHAR(250)
	,@MedicalRecordNumber VARCHAR(250)
	,@FirstName VARCHAR(250)
	,@LastName VARCHAR(250)
	,@SocialSecurityNumber VARCHAR(11)
	,@Address1 VARCHAR(250)
	,@Address2 VARCHAR(250)
	,@City VARCHAR(250)
	,@State VARCHAR(50)
	,@ZipCode VARCHAR(250)
	)
AS
BEGIN

	DECLARE @EnterpriseInternalId VARCHAR(506); SET @EnterpriseInternalId = @Source+'^EMPI^'+@MedicalRecordNumber;
	DECLARE @PatientInternalID VARCHAR(250); SET @PatientInternalID = 'A';

   SELECT @PatientInternalID = (SELECT
		Patient_ID
	FROM Patient.PatientMemberRecord
	WHERE EnterpriseId = @EnterpriseInternalId);

	IF (@PatientInternalID != 'A')
	BEGIN

UPDATE rcd
SET rcd.Active = 0
   ,rcd.ModifiedOn = GETUTCDATE()
FROM Patient.Patient.PatientMemberRecord rcd
WHERE rcd.EnterpriseId = @EnterpriseInternalId;

UPDATE addr
SET addr.Active = 0
   ,addr.ModifiedOn = GETUTCDATE()
FROM Patient.Address addr
INNER JOIN Patient.Patient pt
	ON pt.Patient_AddressID = addr.ID
WHERE pt.ID = @PatientInternalID;

UPDATE pat
SET pat.Active = 0
   ,pat.ModifiedOn = GETUTCDATE()
FROM Patient.Patient pat
WHERE pat.ID = @PatientInternalID;
END
ELSE
RETURN
END
GO

/*
Patient.usp_addPatient 
'HospitalA'
,'123456'
,'John'
,'SMith'
,'333445555'
,'123 Main Street'
,'apt 2'
,'Columbia'
,'MD'
,'66666';


GO
Patient.usp_updatePatient 
'HospitalA'
,'123456'
,'John'
,'SMithsss'
,'333445555'
,'123 Main Streetingness'
,'apt 2'
,'Columbia'
,'MD'
,'66666';
GO

select *
from patient.patientmemberrecord pmr 
join Patient.Patient on pmr.Patient_ID = ID
JOIN Patient.Address on Address.ID = Patient_AddressID

GO
Patient.usp_deletePatient 
'HospitalA'
,'123456'
,'John'
,'SMitfffffhsss'
,'333445555'
,'123 Main Streetingness'
,'apt 2'
,'Columbidddddda'
,'MD'
,'66666';

patient.usp_searchpatient 'HospitalA','123456'

GO
*/

