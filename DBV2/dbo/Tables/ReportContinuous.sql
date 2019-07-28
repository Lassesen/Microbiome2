CREATE TABLE [dbo].[ReportContinuous] (
    [ReportId]     INT        NOT NULL,
    [ContinuousId] INT        NOT NULL,
    [Reading]      FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ReportContinuous] PRIMARY KEY CLUSTERED ([ReportId] ASC, [ContinuousId] ASC),
    CONSTRAINT [FK_ReportContinuous_ContinuousReference] FOREIGN KEY ([ContinuousId]) REFERENCES [dbo].[ContinuousReference] ([ContinuousId]),
    CONSTRAINT [FK_ReportContinuous_OwnerReport] FOREIGN KEY ([ReportId]) REFERENCES [dbo].[OwnerReport] ([ReportId])
);

