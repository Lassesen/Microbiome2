CREATE TABLE [dbo].[ReportCategory] (
    [ReportId]   INT NOT NULL,
    [CategoryId] INT NOT NULL,
    CONSTRAINT [PK_ReportCategory] PRIMARY KEY CLUSTERED ([ReportId] ASC, [CategoryId] ASC),
    CONSTRAINT [FK_ReportCategory_CategoryReference] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[CategoryReference] ([CategoryId]),
    CONSTRAINT [FK_ReportCategory_OwnerReport] FOREIGN KEY ([ReportId]) REFERENCES [dbo].[OwnerReport] ([ReportId])
);

