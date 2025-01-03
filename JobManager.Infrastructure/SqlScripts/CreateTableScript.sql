DO $$
BEGIN
    DROP SCHEMA public CASCADE;
    CREATE SCHEMA public;
    GRANT ALL ON SCHEMA public TO postgres;
    GRANT ALL ON SCHEMA public TO public;
END $$;

DO $$
BEGIN
    CREATE TYPE "JobType" AS ENUM ('None','Onetime','Recurring');
END $$;

DO $$
BEGIN
    CREATE TYPE "RecurringType" AS ENUM (
        'None',
        'EveryMinute',
        'EverySecond',
        'Daily',
        'Weekly',
        'Monthly');
END $$;

DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'job_config') THEN
        DROP TABLE "job_config";
    END IF;
    CREATE TABLE "job_config" (
        "id" BIGSERIAL PRIMARY KEY,
        "name" VARCHAR(200) NOT NULL,
        "active" BOOLEAN NOT NULL,
        "created_time" TIMESTAMPTZ NOT NULL,
        "updated_time" TIMESTAMPTZ NULL,
        "created_by_id" BIGINT NOT NULL,
        "updated_by_id" BIGINT NULL,
        "row_version" bytea NOT NULL DEFAULT gen_random_bytes(16)
    );
END $$;

DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'job') THEN
        DROP TABLE "job";
    END IF;
    CREATE TABLE "job" (
        "id" BIGSERIAL PRIMARY KEY,
        "effective_date_time" TIMESTAMP NOT NULL,
        "description" TEXT,
        "type" VARCHAR(50) NOT NULL,
        "cron_expression" VARCHAR(50),
        "active" BOOLEAN NOT NULL,
        "created_time" TIMESTAMPTZ NOT NULL,
        "updated_time" TIMESTAMPTZ NULL,
        "created_by_id" BIGINT NOT NULL,
        "updated_by_id" BIGINT NULL,
        "row_version" bytea NOT NULL DEFAULT gen_random_bytes(16)
    );
END $$;

DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'job_step') THEN
        DROP TABLE "job_step";
    END IF;
    CREATE TABLE "job_step" (
        "id" BIGSERIAL PRIMARY KEY,
        "job_id" BIGINT NOT NULL,
        "job_config_id" BIGINT NOT NULL,
        "json_parameter" TEXT NOT NULL,
        "active" BOOLEAN NOT NULL,
        "created_time" TIMESTAMPTZ NOT NULL,
        "updated_time" TIMESTAMPTZ NULL,
        "created_by_id" BIGINT NOT NULL,
        "updated_by_id" BIGINT NULL,
        "row_version" bytea NOT NULL DEFAULT gen_random_bytes(16),
        FOREIGN KEY ("job_id") REFERENCES "job"("id"),
        FOREIGN KEY ("job_config_id") REFERENCES "job_config"("id")
    );
END $$;

DO $$
BEGIN
    CREATE TYPE "Status" AS ENUM ('NotStarted','Running','Completed','CompletedWithErrors','Faulted');
END $$;

DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'job_instance') THEN
        DROP TABLE "job_instance";
    END IF;
    CREATE TABLE "job_instance" (
        "id" BIGSERIAL PRIMARY KEY,
        "status" VARCHAR(50) NOT NULL,
        "job_id" BIGINT NOT NULL,
        "active" BOOLEAN NOT NULL,
        "created_time" TIMESTAMPTZ NOT NULL,
        "updated_time" TIMESTAMPTZ NULL,
        "created_by_id" BIGINT NOT NULL,
        "updated_by_id" BIGINT NULL,
        "row_version" bytea NOT NULL DEFAULT gen_random_bytes(16),
        FOREIGN KEY ("job_id") REFERENCES "job"("id")
    );
END $$;

DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'job_step_instance') THEN
        DROP TABLE "job_step_instance";
    END IF;
    CREATE TABLE "job_step_instance" (
        "id" BIGSERIAL PRIMARY KEY,
        "job_instance_id" BIGINT NOT NULL,
        "job_step_id" BIGINT NOT NULL,
        "status" VARCHAR(50) NOT NULL,
        "start_time" TIMESTAMPTZ NOT NULL,
        "end_time" TIMESTAMPTZ,
        "active" BOOLEAN NOT NULL,
        "created_time" TIMESTAMPTZ NOT NULL,
        "updated_time" TIMESTAMPTZ NULL,
        "created_by_id" BIGINT NOT NULL,
        "updated_by_id" BIGINT NULL,
        "row_version" bytea NOT NULL DEFAULT gen_random_bytes(16),
        FOREIGN KEY ("job_instance_id") REFERENCES "job_instance"("id")
    );
END $$;

DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'job_step_instance_log') THEN
        DROP TABLE "job_step_instance_log";
    END IF;
    CREATE TABLE "job_step_instance_log" (
        "id" BIGSERIAL PRIMARY KEY,
        "job_step_instance_id" BIGINT NOT NULL,
        "log" TEXT NOT NULL,
        "active" BOOLEAN NOT NULL,
        "created_time" TIMESTAMPTZ NOT NULL,
        "updated_time" TIMESTAMPTZ NULL,
        "created_by_id" BIGINT NOT NULL,
        "updated_by_id" BIGINT NULL,
        "row_version" bytea NOT NULL DEFAULT gen_random_bytes(16),
        FOREIGN KEY ("job_step_instance_id") REFERENCES "job_step_instance"("id")
    );
END $$;

INSERT INTO public."job_config"(
  "name", "active", "created_time", "created_by_id")
    VALUES ('Job.ProjectLayer.Test,Job.ProjectLayer', true, CURRENT_TIMESTAMP, 1);