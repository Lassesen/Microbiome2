CREATE TABLE [dbo].[ContinuousDisplay] (
    [DisplayGroupId] INT        NOT NULL,
    [ContinuousId]   INT        NOT NULL,
    [DisplayOrder]   FLOAT (53) CONSTRAINT [DF_ContinuousDisplay_DisplayOrder] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ContinuousDisplay] PRIMARY KEY CLUSTERED ([DisplayGroupId] ASC, [ContinuousId] ASC),
    CONSTRAINT [FK_ContinuousDisplay_ContinuousReference] FOREIGN KEY ([ContinuousId]) REFERENCES [dbo].[ContinuousReference] ([ContinuousId]),
    CONSTRAINT [FK_ContinuousDisplay_DisplayGroup] FOREIGN KEY ([DisplayGroupId]) REFERENCES [dbo].[DisplayGroup] ([DisplayGroupId])
);

