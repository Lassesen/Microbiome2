CREATE TABLE [dbo].[CategoryContinuousAssociation] (
    [ContinuousId] INT NOT NULL,
    [CategoryId]   INT NOT NULL,
    CONSTRAINT [PK_CategoryContinuousAssociation] PRIMARY KEY CLUSTERED ([ContinuousId] ASC, [CategoryId] ASC),
    CONSTRAINT [FK_CategoryContinuousAssociation_CategoryReference] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[CategoryReference] ([CategoryId]),
    CONSTRAINT [FK_CategoryContinuousAssociation_ContinuousReference] FOREIGN KEY ([ContinuousId]) REFERENCES [dbo].[ContinuousReference] ([ContinuousId])
);

