CREATE TABLE [dbo].[DisplayGroup] (
    [DisplayGroupId] INT          IDENTITY (1, 1) NOT NULL,
    [DisplayName]    VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DisplayGroup] PRIMARY KEY CLUSTERED ([DisplayGroupId] ASC)
);

