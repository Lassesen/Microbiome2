CREATE TABLE [dbo].[ContinuousRanges] (
    [ContinuousId] INT        NOT NULL,
    [LabelId]      INT        NOT NULL,
    [FromReading]  FLOAT (53) NOT NULL,
    [ToReading]    FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ContinuousRanges] PRIMARY KEY CLUSTERED ([ContinuousId] ASC, [LabelId] ASC),
    CONSTRAINT [FK_ContinuousRanges_ContinuousReference] FOREIGN KEY ([ContinuousId]) REFERENCES [dbo].[ContinuousReference] ([ContinuousId]),
    CONSTRAINT [FK_ContinuousRanges_Labels] FOREIGN KEY ([LabelId]) REFERENCES [dbo].[Labels] ([LabelId])
);

