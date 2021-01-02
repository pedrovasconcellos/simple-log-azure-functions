﻿CREATE DATABASE VasconcellosSolutions
GO
USE VasconcellosSolutions
GO
CREATE TABLE SimpleLog (
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	OurClientId UNIQUEIDENTIFIER,
	ApplicationId UNIQUEIDENTIFIER,
	ApplicationName VARCHAR(150) NOT NULL,
	Description TEXT NOT NULL,
	Json TEXT NOT NULL,
	Created DATETIME NOT NULL
)