CREATE TABLE [dbo].[OwnerReport] (
    [ReportId] INT              IDENTITY (1, 1) NOT NULL,
    [OwnerId]  INT              NOT NULL,
    [AsOfDate] DATE             NOT NULL,
    [Notes]    NVARCHAR (MAX)   NULL,
    [SyncGuid] UNIQUEIDENTIFIER CONSTRAINT [DF_OwnerReport_SyncGuid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_OwnerReport] PRIMARY KEY CLUSTERED ([ReportId] ASC),
    CONSTRAINT [FK_OwnerReport_Owner] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Owner] ([OwnerId])
);




GO
CREATE NONCLUSTERED INDEX [IX_OwnerReport_AsOf]
    ON [dbo].[OwnerReport]([OwnerId] ASC, [AsOfDate] ASC);

