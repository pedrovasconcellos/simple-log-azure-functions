create table SimpleLog (
	Id uniqueidentifier PRIMARY KEY,
	AppName VARCHAR(150) NOT NULL,
	Description TEXT NOT NULL,
	Json TEXT NOT NULL,
	Created DATETIME NOT NULL
)