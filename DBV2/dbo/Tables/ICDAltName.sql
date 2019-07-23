CREATE TABLE [dbo].[ICDAltName] (
    [Code]    VARCHAR (16)   NOT NULL,
    [AltName] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_ICDAltName] PRIMARY KEY CLUSTERED ([Code] ASC, [AltName] ASC),
    CONSTRAINT [FK_ICDAltName_ICDCode] FOREIGN KEY ([Code]) REFERENCES [dbo].[ICDCode] ([Code])
);

