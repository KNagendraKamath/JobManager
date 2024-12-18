﻿
DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'jobconfig') THEN
        DROP TABLE JobConfig;
    END IF;
    CREATE TABLE JobConfig (
        Id BIGINT PRIMARY KEY,
        Name VARCHAR(200) NOT NULL,
        Active BOOLEAN NOT NULL,
        CreatedTime TIMESTAMPTZ NOT NULL,
        UpdatedTime TIMESTAMPTZ NOT NULL,
        CreatedById BIGINT NOT NULL,
        UpdatedById BIGINT NOT NULL
    );
END $$;

DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'job') THEN
        DROP TABLE Job;
    END IF;
    CREATE TABLE Job (
        Id BIGINT PRIMARY KEY,
        EffectiveDateTime TIMESTAMP NOT NULL,
        Description TEXT,
        Type INT NOT NULL,
        RecurringType INT,
        Active BOOLEAN NOT NULL,
        CreatedTime TIMESTAMPTZ NOT NULL,
        UpdatedTime TIMESTAMPTZ NOT NULL,
        CreatedById BIGINT NOT NULL,
        UpdatedById BIGINT NOT NULL
    );
END $$;

DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'jobstep') THEN
        DROP TABLE JobStep;
    END IF;
    CREATE TABLE JobStep (
        Id BIGINT PRIMARY KEY,
        JobId BIGINT NOT NULL,
        JobConfigId BIGINT NOT NULL,
        Parameter TEXT NOT NULL,
        Active BOOLEAN NOT NULL,
        CreatedTime TIMESTAMPTZ NOT NULL,
        UpdatedTime TIMESTAMPTZ NOT NULL,
        CreatedById BIGINT NOT NULL,
        UpdatedById BIGINT NOT NULL,
        FOREIGN KEY (JobId) REFERENCES Job(Id),
        FOREIGN KEY (JobConfigId) REFERENCES JobConfig(Id)
    );
END $$;

DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'jobinstance') THEN
        DROP TABLE JobInstance;
    END IF;
    CREATE TABLE JobInstance (
        Id BIGINT PRIMARY KEY,
        JobId BIGINT NOT NULL,
        Active BOOLEAN NOT NULL,
        CreatedTime TIMESTAMPTZ NOT NULL,
        UpdatedTime TIMESTAMPTZ NOT NULL,
        CreatedById BIGINT NOT NULL,
        UpdatedById BIGINT NOT NULL,
        FOREIGN KEY (JobId) REFERENCES Job(Id)
    );
END $$;

DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'jobstepinstance') THEN
        DROP TABLE JobStepInstance;
    END IF;
    CREATE TABLE JobStepInstance (
        Id BIGINT PRIMARY KEY,
        JobInstanceId BIGINT NOT NULL,
        JobStatus INT NOT NULL,
        StartTime TIMESTAMPTZ NOT NULL,
        EndTime TIMESTAMPTZ,
        Active BOOLEAN NOT NULL,
        CreatedTime TIMESTAMPTZ NOT NULL,
        UpdatedTime TIMESTAMPTZ NOT NULL,
        CreatedById BIGINT NOT NULL,
        UpdatedById BIGINT NOT NULL,
        FOREIGN KEY (JobInstanceId) REFERENCES JobInstance(Id)
    );
END $$;

DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'jobstepinstancelog') THEN
        DROP TABLE JobStepInstanceLog;
    END IF;
    CREATE TABLE JobStepInstanceLog (
        Id BIGINT PRIMARY KEY,
        JobStepInstanceId BIGINT NOT NULL,
        Log TEXT NOT NULL,
        Active BOOLEAN NOT NULL,
        CreatedTime TIMESTAMPTZ NOT NULL,
        UpdatedTime TIMESTAMPTZ NOT NULL,
        CreatedById BIGINT NOT NULL,
        UpdatedById BIGINT NOT NULL,
        FOREIGN KEY (JobStepInstanceId) REFERENCES JobStepInstance(Id)
    );
END $$;