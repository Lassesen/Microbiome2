CREATE TYPE [dbo].[tbi_Table3] AS TABLE (
    [LabResultGuid] UNIQUEIDENTIFIER NULL,
    [SampleDate]    DATETIME         NULL,
    [OwnerGuid]     UNIQUEIDENTIFIER NULL,
    [LabTestGuid]   UNIQUEIDENTIFIER NULL,
    [OtherNotes]    VARCHAR (100)    NULL,
    [labresultid]   INT              NULL,
    [ownerid]       INT              NULL,
    [labtestid]     INT              NULL);

