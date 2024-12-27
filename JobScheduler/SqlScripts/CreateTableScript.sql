DO $$
BEGIN
DROP SCHEMA public CASCADE;
CREATE SCHEMA public;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO public;
END $$;

DO $$ 
BEGIN
CREATE TYPE JobType AS ENUM ('None','Onetime','Recurring');
END $$;

DO $$ 
BEGIN
CREATE TYPE RecurringType AS ENUM (
    'None',
    'EveryMinute',
    'EverySecond',
    'Daily',
    'Weekly',
    'Monthly');
END $$;
	
DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'JobConfig') THEN
        DROP TABLE JobConfig;
    END IF;
    CREATE TABLE JobConfig (
        Id BIGSERIAL  PRIMARY KEY ,
        Name VARCHAR(200) NOT NULL,
        Active BOOLEAN NOT NULL,
        CreatedTime TIMESTAMPTZ NOT NULL,
        UpdatedTime TIMESTAMPTZ NULL,
        CreatedById BIGINT NOT NULL,
        UpdatedById BIGINT NULL
    );
END $$;

DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'Job') THEN
        DROP TABLE Job;
    END IF;
    CREATE TABLE Job (
        Id BIGSERIAL PRIMARY KEY,
        EffectiveDateTime TIMESTAMP NOT NULL,
        Description TEXT,
        Type Varchar(50) NOT NULL,
        RecurringType Varchar(50),
        Active BOOLEAN NOT NULL,
        CreatedTime TIMESTAMPTZ NOT NULL,
        UpdatedTime TIMESTAMPTZ NULL,
        CreatedById BIGINT NOT NULL,
        UpdatedById BIGINT NULL
    );
END $$;

DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'JobStep') THEN
        DROP TABLE JobStep;
    END IF;
    CREATE TABLE JobStep (
        Id BIGSERIAL PRIMARY KEY,
        JobId BIGINT NOT NULL,
        JobConfigId BIGINT NOT NULL,
        JsonParameter TEXT NOT NULL,
        Active BOOLEAN NOT NULL,
        CreatedTime TIMESTAMPTZ NOT NULL,
        UpdatedTime TIMESTAMPTZ NULL,
        CreatedById BIGINT NOT NULL,
        UpdatedById BIGINT NULL,
        FOREIGN KEY (JobId) REFERENCES Job(Id),
        FOREIGN KEY (JobConfigId) REFERENCES JobConfig(Id)
    );
END $$;

DO $$ 
BEGIN
CREATE TYPE Status AS ENUM ('NotStarted','Running','Completed','CompletedWithErrors','Faulted');
END $$;

DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'JobInstance') THEN
        DROP TABLE JobInstance;
    END IF;
    CREATE TABLE JobInstance (
        Id BIGSERIAL PRIMARY KEY,
        JobStatus Varchar(50) NOT NULL,
        JobId BIGINT NOT NULL,
        Active BOOLEAN NOT NULL,
        CreatedTime TIMESTAMPTZ NOT NULL,
        UpdatedTime TIMESTAMPTZ NULL,
        CreatedById BIGINT NOT NULL,
        UpdatedById BIGINT NULL,
        FOREIGN KEY (JobId) REFERENCES Job(Id)
    );
END $$;

DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'JobStepInstance') THEN
        DROP TABLE JobStepInstance;
    END IF;
    CREATE TABLE JobStepInstance (
        Id BIGSERIAL PRIMARY KEY,
        JobInstanceId BIGINT NOT NULL,
        JobStatus Varchar(50) NOT NULL,
        StartTime TIMESTAMPTZ NOT NULL,
        EndTime TIMESTAMPTZ,
        Active BOOLEAN NOT NULL,
        CreatedTime TIMESTAMPTZ NOT NULL,
        UpdatedTime TIMESTAMPTZ NULL,
        CreatedById BIGINT NOT NULL,
        UpdatedById BIGINT NULL,
        FOREIGN KEY (JobInstanceId) REFERENCES JobInstance(Id)
    );
END $$;

DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'JobStepInstanceLog') THEN
        DROP TABLE JobStepInstanceLog;
    END IF;
    CREATE TABLE JobStepInstanceLog (
        Id BIGSERIAL PRIMARY KEY,
        JobStepInstanceId BIGINT NOT NULL,
        Log TEXT NOT NULL,
        Active BOOLEAN NOT NULL,
        CreatedTime TIMESTAMPTZ NOT NULL,
        UpdatedTime TIMESTAMPTZ NULL,
        CreatedById BIGINT NOT NULL,
        UpdatedById BIGINT NULL,
        FOREIGN KEY (JobStepInstanceId) REFERENCES JobStepInstance(Id)
    );
END $$;

INSERT INTO public.JobConfig(
  name, active, createdtime, createdbyid)
	VALUES ('Test', true, CURRENT_TIMESTAMP, 1);